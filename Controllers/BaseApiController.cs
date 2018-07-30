using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.Data;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class BaseApiController : Controller
    {
        #region Shared Properties
            protected ApplicationDbContext DbContext{get; private set;}
            protected JsonSerializerSettings JsonSettings{get; private set;}
        #endregion
        #region Constructor
            public BaseApiController(ApplicationDbContext dbContext)
            {
                //Instantiate through DI
                DbContext = dbContext;
                JsonSettings = new JsonSerializerSettings{
                    Formatting = Formatting.Indented
                };
            }
        #endregion
    }
}