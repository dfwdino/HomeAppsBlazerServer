using HomeAppsBlazerServer.Models;
using HomeAppsBlazerServer.Models.Shopping;
using HomeAppsBlazerServer.Models.Shopping.Interface;

namespace HomeAppsBlazerServer.Tests;

/// <summary>
/// Tests the in-memory join/aggregation logic from GetAllNeedItemsAsync.
/// These mirror the LINQ logic in ShoppingServices.NeedList.cs without needing a DB.
/// </summary>
public class ShoppingResultLogicTests
{
    // ---- Helpers to build test data ----

    private static List<ShoppingItem> MakeItems(params (int id, string name, bool deleted)[] items) =>
        items.Select(i => new ShoppingItem { ShoppingItemID = i.id, ItemName = i.name, IsDeleted = i.deleted }).ToList();

    private static List<ShoppingStore> MakeStores(params (int id, string name)[] stores) =>
        stores.Select(s => new ShoppingStore { ShoppingStoreID = s.id, StoreName = s.name }).ToList();

    private static List<ShoppingItemList> MakeListItems(params (int listId, int itemId, int? storeId, bool gotItem)[] entries) =>
        entries.Select(e => new ShoppingItemList
        {
            ShoppingItemListID = e.listId,
            ShoppingItemID = e.itemId,
            ShoppingStoreID = e.storeId,
            GotItem = e.gotItem
        }).ToList();

    private static List<PriceHistory> MakePrices(params (int itemId, decimal amount, DateTime date)[] prices) =>
        prices.Select(p => new PriceHistory { ItemID = p.itemId, Amount = p.amount, PriceDate = p.date }).ToList();

    // Mirrors the result-building LINQ from GetAllNeedItemsAsync
    private static List<ShoppingItemResult> BuildResults(
        List<ShoppingItemList> listItems,
        List<ShoppingItem> allItems,
        List<ShoppingStore> stores,
        List<PriceHistory> priceHistory)
    {
        var results = listItems.Select(item => new ShoppingItemResult
        {
            ItemID = item.ShoppingItemID,
            ShoppingItemListID = item.ShoppingItemListID,
            ItemName = allItems.Where(mm => mm.ShoppingItemID == item.ShoppingItemID).Select(mm => mm.ItemName).FirstOrDefault(),
            storename = stores.Where(mm => mm.ShoppingStoreID == item.ShoppingStoreID).FirstOrDefault()?.StoreName,
            Price = priceHistory.Where(mm => mm.ItemID == item.ShoppingItemID).OrderByDescending(mm => mm.PriceDate).Select(mm => mm.Amount).FirstOrDefault(),
            NeedDate = item.NeedDate,
            Notes = item.Notes,
            NumberOfItems = item.NumberOfItems,
        }).ToList();

        return results.OrderBy(mm => mm.ItemName).ToList();
    }

    // ---- Tests ----

    [Fact]
    public void Results_OrderedAlphabeticallyByItemName()
    {
        var items = MakeItems((1, "Zebra Fish", false), (2, "Apples", false), (3, "Milk", false));
        var stores = MakeStores((10, "Target"));
        var listItems = MakeListItems((1, 1, 10, false), (2, 2, 10, false), (3, 3, 10, false));
        var prices = MakePrices();

        var results = BuildResults(listItems, items, stores, prices);

        Assert.Equal("Apples", results[0].ItemName);
        Assert.Equal("Milk", results[1].ItemName);
        Assert.Equal("Zebra Fish", results[2].ItemName);
    }

    [Fact]
    public void Results_MostRecentPrice_IsUsed_NotOldest()
    {
        var items = MakeItems((1, "Milk", false));
        var stores = MakeStores((10, "Costco"));
        var listItems = MakeListItems((1, 1, 10, false));
        var prices = MakePrices(
            (1, 3.99m, new DateTime(2025, 1, 1)),   // older
            (1, 4.49m, new DateTime(2026, 1, 1)),   // newest
            (1, 4.19m, new DateTime(2025, 6, 1))    // middle
        );

        var results = BuildResults(listItems, items, stores, prices);

        Assert.Equal(4.49m, results[0].Price);
    }

    [Fact]
    public void Results_ItemWithNoPriceHistory_PriceIsNull()
    {
        var items = MakeItems((1, "Eggs", false));
        var stores = MakeStores((10, "Aldi"));
        var listItems = MakeListItems((1, 1, 10, false));
        var prices = MakePrices(); // no prices

        var results = BuildResults(listItems, items, stores, prices);

        Assert.Null(results[0].Price);
    }

    [Fact]
    public void Results_StoreNameMatchedByStoreID()
    {
        var items = MakeItems((1, "Bread", false));
        var stores = MakeStores((10, "Walmart"), (20, "Costco"));
        var listItems = MakeListItems((1, 1, 20, false)); // store 20 = Costco

        var results = BuildResults(listItems, items, stores, MakePrices());

        Assert.Equal("Costco", results[0].storename);
    }

    [Fact]
    public void Results_ItemWithNoMatchingStore_StoreNameIsNull()
    {
        var items = MakeItems((1, "Juice", false));
        var stores = MakeStores((10, "Target"));
        var listItems = MakeListItems((1, 1, 99, false)); // store 99 doesn't exist

        var results = BuildResults(listItems, items, stores, MakePrices());

        Assert.Null(results[0].storename);
    }

    [Fact]
    public void Results_ItemNameMatchedByItemID()
    {
        var items = MakeItems((1, "Cheese", false), (2, "Butter", false));
        var stores = MakeStores((10, "Aldi"));
        var listItems = MakeListItems((1, 2, 10, false)); // item 2 = Butter

        var results = BuildResults(listItems, items, stores, MakePrices());

        Assert.Equal("Butter", results[0].ItemName);
    }

    [Fact]
    public void Results_OnlyIncludesGotItem_False_Entries()
    {
        // The caller filters GotItem==false before passing to BuildResults,
        // so here we verify that if we pass only un-got items, all appear
        var items = MakeItems((1, "Apples", false), (2, "Bananas", false));
        var stores = MakeStores((10, "Target"));
        // Simulate what the service does: filter GotItem==false before building
        var allListItems = MakeListItems((1, 1, 10, false), (2, 2, 10, true));
        var filteredListItems = allListItems.Where(mm => mm.GotItem == false).ToList();

        var results = BuildResults(filteredListItems, items, stores, MakePrices());

        Assert.Single(results);
        Assert.Equal("Apples", results[0].ItemName);
    }

    [Fact]
    public void Results_NeedDate_PropagatesToResult()
    {
        var items = MakeItems((1, "Yogurt", false));
        var stores = MakeStores((10, "Kroger"));
        var needDate = new DateTime(2026, 4, 18);
        var listItems = new List<ShoppingItemList>
        {
            new ShoppingItemList { ShoppingItemListID = 1, ShoppingItemID = 1, ShoppingStoreID = 10, GotItem = false, NeedDate = needDate }
        };

        var results = BuildResults(listItems, items, stores, MakePrices());

        Assert.Equal(needDate, results[0].NeedDate);
    }

    [Fact]
    public void Results_EmptyList_ReturnsEmpty()
    {
        var results = BuildResults(new List<ShoppingItemList>(), new List<ShoppingItem>(), new List<ShoppingStore>(), new List<PriceHistory>());
        Assert.Empty(results);
    }
}
