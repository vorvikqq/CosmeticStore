﻿using Microsoft.AspNetCore.Identity;
using System.Data;
using CosmeticStore.Constants;

namespace CosmeticStore.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            // adding some roles to db
            //await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            //await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Seller.ToString()));

            // create admin user
            //var admin = new IdentityUser
            //{
            //    UserName = "vorvik",
            //    Email = "vorvik@gmail.com",
            //    EmailConfirmed = true
            //};

            //var userInDb = await userMgr.FindByEmailAsync(admin.Email);

            //if (userInDb is null)
            //{
            //    await userMgr.CreateAsync(admin, "Admin@123");
            //    await userMgr.AddToRoleAsync(admin, Roles   .Admin.ToString());
            //}
        }
    }
}
