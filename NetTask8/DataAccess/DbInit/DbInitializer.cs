using Microsoft.AspNetCore.Identity;
using NetTask8.Data;
using NetTask8.Models;
using NetTask8.Utility;

namespace NetTask8.DataAccess.DbInit
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(StaticDetails.AprovalRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.AprovalRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.UserRole)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "User1@site.com",
                    Email = "User1@site.com",

                }, "Admin@123").GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "User2@site.com",
                    Email = "User2@site.com",

                }, "Admin@123").GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "User3@site.com",
                    Email = "User3@site.com",

                }, "Admin@123").GetAwaiter().GetResult();

                var user1 = _db.Users.FirstOrDefault(u => u.Email == "User1@site.com");
                if (user1 is not null) _userManager.AddToRoleAsync(user1, StaticDetails.UserRole).GetAwaiter().GetResult();

                var user2 = _db.Users.FirstOrDefault(u => u.Email == "User2@site.com");
                if (user2 is not null) _userManager.AddToRoleAsync(user2, StaticDetails.AprovalRole).GetAwaiter().GetResult();

                var user3 = _db.Users.FirstOrDefault(u => u.Email == "User3@site.com");
                if (user3 is not null) _userManager.AddToRoleAsync(user3, StaticDetails.AprovalRole).GetAwaiter().GetResult();


                Category c1 = new() { Name = "Public" };
                Category c2 = new() { Name = "Internal" };
                Category c3 = new() { Name = "Confidential" };

                _db.Categories.Add(c1);
                _db.Categories.Add(c2);
                _db.Categories.Add(c3);
                _db.SaveChanges();
            }
        }
    }
}
