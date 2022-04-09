using Amazon.Data.Static;
using Amazon.Models;
using Microsoft.AspNetCore.Identity;
using static Amazon.Data.Enums.Enums;

namespace Amazon.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AmazonDbContext>();

                context.Database.EnsureCreated();
            }

        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Client))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Client));
                if (!await roleManager.RoleExistsAsync(UserRoles.Vendor))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Vendor));

                //Users
                //------ Admin
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "admin@Amazon.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        Name = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        UserType = UserType.Admin

                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                //------ User
                string appUserEmail = "user@Amazon.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        Name = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        UserType = UserType.Client

                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.Client);
                }

                //----- Vendor
                string appVendorEmail = "Vendor@Amazon.com";

                var appVendor = await userManager.FindByEmailAsync(appVendorEmail);
                if (appVendor == null)
                {
                    var newAppVendor = new User()
                    {
                        Name = "Application Vendor",
                        UserName = "app-Vendor",
                        Email = appVendorEmail,
                        EmailConfirmed = true,
                        UserType= UserType.Vendor
                    };
                    await userManager.CreateAsync(newAppVendor, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppVendor, UserRoles.Vendor);
                }

            }
        }
    }
}
