using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using HomeAppsBlazerServer.Servcies.Shopping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;

namespace HomeAppsBlazerServer.Tests;

public class ShoppingServicesTests : IDisposable
{
    private readonly MyDbContext _db;
    private readonly ShoppingServices _svc;

    public ShoppingServicesTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // fresh DB per test
            .Options;

        _db = new MyDbContext(options);
        _svc = new ShoppingServices(_db, NullLogger<ShoppingServices>.Instance, new MemoryCache(new MemoryCacheOptions()));
    }

    public void Dispose() => _db.Dispose();

    // ---- Helpers ----

    private ItemBrand AddBrand(string name)
    {
        var brand = new ItemBrand { BrandName = name };
        _db.ItemBrands.Add(brand);
        _db.SaveChanges();
        return brand;
    }

    private ShoppingItem AddItem(string name, int? brandId = null, int? storeId = null, bool isOneTimeOnly = false)
    {
        var item = new ShoppingItem
        {
            ItemName = name,
            ItemBrandID = brandId,
            StoreID = storeId,
            IsOneTimeOnly = isOneTimeOnly
        };
        _db.ShoppingItems.Add(item);
        _db.SaveChanges();
        return item;
    }

    // ========== AddShoppingItemAsyn ==========

    [Fact]
    public async Task AddShoppingItem_NewItem_IsAddedSuccessfully()
    {
        var brand = AddBrand("Kirkland");
        var item = new ShoppingItem { ItemName = "butter", ItemBrandID = brand.ItemBrandID };

        var result = await _svc.AddShoppingItemAsyn(item, CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(_db.ShoppingItems.Any(x => x.ItemName == "Butter")); // title-cased
    }

    [Fact]
    public async Task AddShoppingItem_NewItem_ItemNameIsTitleCased()
    {
        var brand = AddBrand("Organic");
        var item = new ShoppingItem { ItemName = "whole milk", ItemBrandID = brand.ItemBrandID };

        var result = await _svc.AddShoppingItemAsyn(item, CancellationToken.None);

        Assert.Equal("Whole Milk", result!.ItemName);
    }

    [Fact]
    public async Task AddShoppingItem_NewItem_AutoAddsToNeedList()
    {
        var brand = AddBrand("Generic");
        var item = new ShoppingItem { ItemName = "eggs", ItemBrandID = brand.ItemBrandID };

        var result = await _svc.AddShoppingItemAsyn(item, CancellationToken.None);

        Assert.True(_db.ShoppingItemList.Any(x => x.ShoppingItemID == result!.ShoppingItemID));
    }

    [Fact]
    public async Task AddShoppingItem_DuplicateItemAndBrand_ReturnsNull()
    {
        var brand = AddBrand("Dole");

        // Add the item once
        var first = new ShoppingItem { ItemName = "bananas", ItemBrandID = brand.ItemBrandID };
        await _svc.AddShoppingItemAsyn(first, CancellationToken.None);

        // Try to add the same item+brand again
        var duplicate = new ShoppingItem { ItemName = "bananas", ItemBrandID = brand.ItemBrandID };
        var result = await _svc.AddShoppingItemAsyn(duplicate, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddShoppingItem_SameItemDifferentBrand_IsAllowed()
    {
        var brand1 = AddBrand("Store Brand");
        var brand2 = AddBrand("Name Brand");

        var first = new ShoppingItem { ItemName = "orange juice", ItemBrandID = brand1.ItemBrandID };
        await _svc.AddShoppingItemAsyn(first, CancellationToken.None);

        var second = new ShoppingItem { ItemName = "orange juice", ItemBrandID = brand2.ItemBrandID };
        var result = await _svc.AddShoppingItemAsyn(second, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, _db.ShoppingItems.Count());
    }

    // ========== AddItemToList ==========

    [Fact]
    public async Task AddItemToList_ItemNotOnList_AddsSuccessfully()
    {
        var item = AddItem("Milk");

        await _svc.AddItemToList(item.ShoppingItemID);

        Assert.True(_db.ShoppingItemList.Any(x => x.ShoppingItemID == item.ShoppingItemID && x.GotItem == false));
    }

    [Fact]
    public async Task AddItemToList_ItemAlreadyOnActiveList_DoesNotAddAgain()
    {
        var item = AddItem("Cheese");

        await _svc.AddItemToList(item.ShoppingItemID);
        await _svc.AddItemToList(item.ShoppingItemID); // second call

        Assert.Equal(1, _db.ShoppingItemList.Count(x => x.ShoppingItemID == item.ShoppingItemID && x.GotItem == false));
    }

    // ========== GotItem ==========

    [Fact]
    public async Task GotItem_MarksItemAsGotten()
    {
        var item = AddItem("Apples");
        var listEntry = new ShoppingItemList { ShoppingItemID = item.ShoppingItemID, GotItem = false };
        _db.ShoppingItemList.Add(listEntry);
        _db.SaveChanges();

        await _svc.GotItem(listEntry.ShoppingItemListID);

        var updated = _db.ShoppingItemList.Find(listEntry.ShoppingItemListID);
        Assert.True(updated!.GotItem);
        Assert.NotNull(updated.GotItemDate);
    }

    [Fact]
    public async Task GotItem_OneTimeOnlyItem_IsSoftDeleted()
    {
        var item = AddItem("Party Cups", isOneTimeOnly: true);
        var listEntry = new ShoppingItemList { ShoppingItemID = item.ShoppingItemID, GotItem = false };
        _db.ShoppingItemList.Add(listEntry);
        _db.SaveChanges();

        await _svc.GotItem(listEntry.ShoppingItemListID);

        var deletedItem = _db.ShoppingItems.Find(item.ShoppingItemID);
        Assert.True(deletedItem!.IsDeleted);
    }

    [Fact]
    public async Task GotItem_RegularItem_IsNotDeleted()
    {
        var item = AddItem("Bread", isOneTimeOnly: false);
        var listEntry = new ShoppingItemList { ShoppingItemID = item.ShoppingItemID, GotItem = false };
        _db.ShoppingItemList.Add(listEntry);
        _db.SaveChanges();

        await _svc.GotItem(listEntry.ShoppingItemListID);

        var notDeleted = _db.ShoppingItems.Find(item.ShoppingItemID);
        Assert.False(notDeleted!.IsDeleted);
    }

    // ========== Negative Tests — graceful handling of bad input ==========

    [Fact]
    public async Task GotItem_NonExistentListId_ReturnsFalse()
    {
        // GotItem now returns false instead of crashing when the list ID doesn't exist.
        var result = await _svc.GotItem(9999);
        Assert.False(result);
    }

    [Fact]
    public async Task AddItemToList_NonExistentItemId_DoesNotThrow()
    {
        // AddItemToList now exits gracefully instead of crashing when the item doesn't exist.
        var ex = await Record.ExceptionAsync(() => _svc.AddItemToList(9999));
        Assert.Null(ex);
    }

    // ========== GetLastestPrice ==========

    [Fact]
    public async Task GetLatestPrice_NoPriceHistory_ReturnsZero()
    {
        var item = AddItem("Yogurt");

        var result = await _svc.GetLastestPrice(item.ShoppingItemID, null);

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetLatestPrice_MultiplePrices_ReturnsLastInserted()
    {
        // Service orders by PriceHistoryID DESC (insertion order), not by PriceDate.
        // So the most recently added record wins, regardless of its date.
        var item = AddItem("Coffee");
        _db.PriceHistory.AddRange(
            new PriceHistory { ItemID = item.ShoppingItemID, Amount = 8.99m, PriceDate = new DateTime(2025, 1, 1) },
            new PriceHistory { ItemID = item.ShoppingItemID, Amount = 10.49m, PriceDate = new DateTime(2026, 1, 1) },
            new PriceHistory { ItemID = item.ShoppingItemID, Amount = 9.75m, PriceDate = new DateTime(2025, 6, 1) }  // last inserted
        );
        _db.SaveChanges();

        var result = await _svc.GetLastestPrice(item.ShoppingItemID, null);

        Assert.Equal(9.75m, result); // last inserted record wins
    }

    [Fact]
    public async Task GetLatestPrice_WithStoreFilter_ReturnsStoreSpecificPrice()
    {
        var item = AddItem("Butter");
        _db.PriceHistory.AddRange(
            new PriceHistory { ItemID = item.ShoppingItemID, Amount = 4.99m, StoreID = 1, PriceDate = new DateTime(2026, 1, 1), PriceHistoryID = 1 },
            new PriceHistory { ItemID = item.ShoppingItemID, Amount = 5.49m, StoreID = 2, PriceDate = new DateTime(2026, 1, 1), PriceHistoryID = 2 }
        );
        _db.SaveChanges();

        var result = await _svc.GetLastestPrice(item.ShoppingItemID, storeid: 1);

        Assert.Equal(4.99m, result);
    }
}
