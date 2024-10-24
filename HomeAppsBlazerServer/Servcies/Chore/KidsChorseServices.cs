﻿using HomeAppsBlazerServer.Components;
using HomeAppsBlazerServer.Data;
using HomeAppsBlazerServer.Models;
using Microsoft.Identity.Client;

namespace HomeAppsBlazerServer.Servcies.Chore
{
    public class KidsChorseKidsServices
    {

        private readonly MyDbContext myDbContext;
        private readonly ILogger<ShoppingServices> _logger;

        public KidsChorseKidsServices(MyDbContext myDbContext, ILogger<ShoppingServices> logger)
        {
            this.myDbContext = myDbContext;
            _logger = logger;
        }

        public async Task<List<KidsNameModel>> GetKids()
        {
            List<KidsNameModel> Kids = myDbContext.KidsName.Where(mm => mm.IsDeleted == false).ToList();

            return Kids;
        }

        public async void AddKid(KidsNameModel kidsNameModel)
        {
            if (kidsNameModel is null)
            {
                return;
            }
            else if (kidsNameModel.KidName == null)
            {
                return;
            }

            //kidsNameModel.KidName = kidsNameModel.KidName.ToTileCase();
            myDbContext.KidsName.Add(kidsNameModel);

            myDbContext.SaveChanges();
        }

        public async Task<KidsNameModel> GetKid(int id)
        {
            return myDbContext.KidsName.Where(mm => mm.IDKidsName == id).FirstOrDefault();
        }

        public async void UpdateKid(KidsNameModel kidsNameModel)
        {
            myDbContext.KidsName.Update(kidsNameModel);
            myDbContext.SaveChanges();
        }

        public async void DeleteKid(KidsNameModel kidsNameModel)
        {
            
            myDbContext.KidsName.Remove(kidsNameModel);
        }

    }
}
