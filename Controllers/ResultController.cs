using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TestMakerFree.Data;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    public class ResultController : BaseApiController
    {
        //     #region Private Members
        //         private ApplicationDbContext DbContext;
        //     #endregion
        #region Constructor
            public ResultController(ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration) :base(dbContext, roleManager, userManager, configuration){}
        #endregion
        
        #region Restful conventions methods
        /// <summary>
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <paramas name="id"> The id of the existing Result</params>
        /// <returns> The Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            var result = DbContext.Results.Where(r => r.Id == id ).FirstOrDefault();

            if(result == null) return NotFound(new{
                Error = $"Result with the id {id} is not found"
            });

            return new JsonResult(result.Adapt<ResultViewModel>(), new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });
        }

        /// <summary>
        /// Inserts new Result to the Database
        /// </summary>
        /// <params name="m"> The Result view model containing the data t insert </params>
        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel m){
            if(m == null){
                return new StatusCodeResult(500);
            }
            var result = m.Adapt<Result>();

            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;

            DbContext.Results.Add(result);
            DbContext.SaveChanges();

            return new JsonResult(result.Adapt<ResultViewModel>(),new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });
        }

        /// <summary>
        /// Edit the Result with the given ID
        /// </summary>
        /// <params name="m">Result view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel m){
            if(m == null) return new StatusCodeResult(500);

            var result = DbContext.Results.Where( r => r.Id == m.Id).FirstOrDefault();

            if(result == null){
                return NotFound(new{
                    Error = $"The rwsult with id {m.Id} is missing"
                });
            }

            result.QuizId = m.QuizId;
            result.Text = m.Text;
            result.MaxValue = m.MaxValue;
            result.MinValue = m.MinValue;
            result.Notes = m.Notes;

            result.LastModifiedDate = result.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(result.Adapt<ResultViewModel>(), new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });
        }

        /// <sumary>
        /// Deletes the Result with the given id
        /// </summary>
        /// <params name="id"> The id of the Result to be deleted </params>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            var result = DbContext.Results.Where( r => r.Id == id).FirstOrDefault();

            if(result == null) return NotFound(new{
                Error = $"the result with id {id} is not found"
            });

            DbContext.Results.Remove(result);
            DbContext.SaveChanges();

            return new OkResult();
        } 
        #endregion
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId){
            var results = DbContext.Results.Where( r => r.QuizId == quizId).ToArray();

            return new JsonResult(results.Adapt<ResultViewModel[]>(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
        }
    }
}