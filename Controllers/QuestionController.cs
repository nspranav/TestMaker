using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {

                #region Restful conventions methods
        /// <summary>
        /// Retrieves the Question with the given {id}
        /// </summary>
        /// <paramas name="id"> The id of the existing Question</params>
        /// <returns> The Question with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            return Content("Not implemented yet");
        }

        /// <summary>
        /// Inserts new Question to the Database
        /// </summary>
        /// <params name="m"> The Question view model containing the data t insert </params>
        [HttpPut]
        public IActionResult Put(QuestionViewModel m){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the Question with the given ID
        /// </summary>
        /// <params name="m">Question view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post(QuestionViewModel m){
            throw new NotImplementedException();
        }

        /// <sumary>
        /// Deletes the Question with the given id
        /// </summary>
        /// <params name="id"> The id of the Question to be deleted </params>
        [HttpDelete]
        public IActionResult Delete(int id){
            throw new NotImplementedException();
        } 
        #endregion
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId){
            var sampleQuestions = new List<QuestionViewModel>();
            
            sampleQuestions.Add(new QuestionViewModel{
                Id = 1,
                QuizId = quizId,
                Text ="What do you value most in yur life?",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            });

            for (int i = 2; i <= 5; i++)
            {
                sampleQuestions.Add(new QuestionViewModel{
                    Id = i,
                    QuizId = quizId,
                    Text = $"sample question {i}",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                });
            }

            return new JsonResult(sampleQuestions, new JsonSerializerSettings{ Formatting = Formatting.Indented});
        }
    }
}