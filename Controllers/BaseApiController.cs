using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TestMakerFree.Data;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        #region Shared Properties
        protected ApplicationDbContext DbContext { get; private set; }
        protected RoleManager<IdentityRole> RoleManager { get; private set; }
        protected UserManager<ApplicationUser> UserManager { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        protected JsonSerializerSettings JsonSettings { get; private set; }
        #endregion
        #region Constructor
        public BaseApiController(ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
            )
        {
            //Instantiate through DI
            DbContext = dbContext;
            RoleManager = roleManager;
            UserManager = userManager;
            Configuration = configuration;
            JsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }
        #endregion
    }
}