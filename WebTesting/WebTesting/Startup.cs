using Microsoft.Owin;
using Owin;
using WebTesting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


[assembly: OwinStartupAttribute(typeof(WebTesting.Startup))]
namespace WebTesting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            if(CreateAdminRole())
            {
                CreateUserAdmin();
            }
        }

        private bool CreateAdminRole()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(db);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            if(!roleManager.RoleExists("admin"))
            {
                IdentityRole role = new IdentityRole("admin");
                IdentityResult result= roleManager.Create(role);
                if(result.Succeeded)
                {
                    return true;  
                }

                return false;
            }
            return false;
            // i've made bool type method becouse i am strting application
            //over and over and don't wont to create role every time i start
        }

        private void CreateUserAdmin()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            //doing this for testing purpose
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            ApplicationUser userAdmin = new ApplicationUser
            {
                UserName = "yollo123",
                Name = "Yollo",
                Surname = "Petr",
                Email = "yollo@gmail.com",
            };
            
            if (userManager.Create(userAdmin, "123").Succeeded)
            {
                userManager.AddToRole(userAdmin.Id, "admin");
            }

        }
    }
}
