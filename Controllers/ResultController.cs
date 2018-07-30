using System;
using System.Collections.Generic;
using System.Linq;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.Data;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
        #region Private Members
            private ApplicationDbContext DbContext;
        #endregion
        #region Constructor
            public ResultController(ApplicationDbContext dbContext)
            {
                DbContext = dbContext;
            }
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
        public IActionResult All(int questionId){
            var sampleResults = new List<ResultViewModel>();
            
            sampleResults.Add(new ResultViewModel{
                Id = 1,
                QuizId = questionId,
                Text ="Friends and Family",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            for (int i = 2; i <= 5; i++)
            {
                sampleResults.Add(new ResultViewModel{
                    Id = i,
                    QuizId = questionId,
                    Text = $"sample Result {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            return new JsonResult(sampleResults, new JsonSerializerSettings{ Formatting = Formatting.Indented});
        }
    }
}