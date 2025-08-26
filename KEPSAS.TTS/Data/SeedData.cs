using Microsoft.AspNetCore.Identity;

namespace KEPSAS.TTS.Data
{
    public static class SeedData
    {
        public static async Task RunAsync(IServiceProvider sp)
        {
            var roleMgr = sp.GetRequiredService<RoleManager<IdentityRole>>();
            var userMgr = sp.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var r in new[] { "Admin", "User" })
                if (!await roleMgr.RoleExistsAsync(r))
                    await roleMgr.CreateAsync(new IdentityRole(r));

            var adminEmail = "admin@kepsas.com";
            var admin = await userMgr.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Ad = "Admin",
                    Soyad = "User",
                    DisplayName = "Administrator"
                };
                await userMgr.CreateAsync(admin, "Admin!234");
            }

            if (!await userMgr.IsInRoleAsync(admin, "Admin"))
                await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
