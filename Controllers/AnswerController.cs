using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        #region Restful conventions methods
        /// <summary>
        /// Retrieves the answer with the given {id}
        /// </summary>
        /// <paramas name="id"> The id of the existing answer</params>
        /// <returns> The answer with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            return Content("Not implemented yet");
        }

        /// <summary>
        /// Inserts new Answer to the Database
        /// </summary>
        /// <params name="m"> The answer view model containing the data t insert </params>
        [HttpPut]
        public IActionResult Put(AnswerViewModel m){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the answer with the given ID
        /// </summary>
        /// <params name="m">Answer view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post(AnswerViewModel m){
            throw new NotImplementedException();
        }

        /// <sumary>
        /// Deletes the answer with the given id
        /// </summary>
        /// <params name="id"> The id of the answer to be deleted </params>
        [HttpDelete]
        public IActionResult Delete(int id){
            throw new NotImplementedException();
        } 
        #endregion
        
        [HttpGet("All/{quizId}")]
        public IActionResult All(int questionId){
            var sampleAnswers = new List<AnswerViewModel>();
            
            sampleAnswers.Add(new AnswerViewModel{
                Id = 1,
                QuestionId = questionId,
                Text ="Friends and Family",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            for (int i = 2; i <= 5; i++)
            {
                sampleAnswers.Add(new AnswerViewModel{
                    Id = i,
                    QuestionId = questionId,
                    Text = $"sample answer {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            return new JsonResult(sampleAnswers, new JsonSerializerSettings{ Formatting = Formatting.Indented});
        }
    }
}