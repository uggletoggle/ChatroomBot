using External.IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace External.IdentityServer.Data
{
    public static class InitDb
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using( var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AccountDbContext>();
                var configContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();

                SeedUsersData(context, userManager);
                SeedConfigData(configContext);
            }
        }

        private static void SeedConfigData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach(var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var scope in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }

                context.SaveChanges();
            }
        }

        private static void SeedUsersData(AccountDbContext context, UserManager<AppUser> userManager)
        {
            if(!context.Users.Any())
            {
                var testUsers = new List<AppUser>()
                {
                    new AppUser
                    {
                        UserName = "boya",
                        Password = "123456"
                    },
                    new AppUser
                    {
                        UserName = "nuno",
                        Password = "654321"
                    },
                };

                testUsers.ForEach(async user =>
                {
                    await userManager.CreateAsync(user, user.Password);
                });

            }
        }
    }
}
