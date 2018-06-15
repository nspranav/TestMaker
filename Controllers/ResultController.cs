using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : Controller
    {
                #region Restful conventions methods
        /// <summary>
        /// Retrieves the Result with the given {id}
        /// </summary>
        /// <paramas name="id"> The id of the existing Result</params>
        /// <returns> The Result with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            return Content("Not implemented yet");
        }

        /// <summary>
        /// Inserts new Result to the Database
        /// </summary>
        /// <params name="m"> The Result view model containing the data t insert </params>
        [HttpPut]
        public IActionResult Put(ResultViewModel m){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the Result with the given ID
        /// </summary>
        /// <params name="m">Result view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post(ResultViewModel m){
            throw new NotImplementedException();
        }

        /// <sumary>
        /// Deletes the Result with the given id
        /// </summary>
        /// <params name="id"> The id of the Result to be deleted </params>
        [HttpDelete]
        public IActionResult Delete(int id){
            throw new NotImplementedException();
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