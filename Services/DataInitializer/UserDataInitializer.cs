using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Common.Enums;
using Entities.UserManager;

namespace Services.DataInitializer
{
    public class UserDataInitializer : IDataInitializer
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public UserDataInitializer(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void InitializeData()
        {
            if (!roleManager.RoleExistsAsync(Permission.Manager).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new Role { Name = Permission.Manager }).GetAwaiter().GetResult();
            }            
            if (!roleManager.RoleExistsAsync(Permission.Dealer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new Role { Name = Permission.Dealer }).GetAwaiter().GetResult();
            }
            if (!roleManager.RoleExistsAsync(Permission.Customer).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new Role { Name = Permission.Customer }).GetAwaiter().GetResult();
            }
            if (!userManager.Users.AsNoTracking().Any(p => p.UserName == "admin"))
            {
                var user = new User
                {
                    Age = 23,
                    FName = "باربد",
                    LName = "صمدی نیا",
                    Address = "شهرک سعدی",
                    Gender = GenderType.Male,
                    UserName = "admin",
                    Email = "admin@site.com"
                };
                userManager.CreateAsync(user, "123456").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(user, Permission.Manager).GetAwaiter().GetResult();
            }
        }
    }
}