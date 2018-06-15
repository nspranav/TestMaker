using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    [Route("/api/[controller]")]
    public class QuizController : Controller
    {
        #region Restful convention method
        /// <summary>
        /// GET: api/quiz/{}id
        /// Retrieves the quiz with given Id
        /// </summary>
        /// <param name="id"> The id of the quiz</param>
        /// <returns> quiz with the given id</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            var v = new QuizViewModel{
                Id = id,
                Title=$"Sample quiz with Id = {id}",
                Description = $"Not a real quiz",
                CreatedDate = DateTime.Now,
                LastModifiedDate =DateTime.Now
            };

            return new JsonResult(v, new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });
        }

         /// <summary>
        /// Inserts new Quiz to the Database
        /// </summary>
        /// <params name="m"> The Quiz view model containing the data to insert </params>
        [HttpPut]
        public IActionResult Put(QuizViewModel m){
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edit the Quiz with the given ID
        /// </summary>
        /// <params name="m">Quiz view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post(QuizViewModel m){
            throw new NotImplementedException();
        }

        /// <sumary>
        /// Deletes the Quiz with the given id
        /// </summary>
        /// <params name="id"> The id of the Quiz to be deleted </params>
        [HttpDelete]
        public IActionResult Delete(int id){
            throw new NotImplementedException();
        } 
        #endregion
        [HttpGet("Latest/{num}")]
        public IActionResult Latest(int num=10){
            var sampleQuizzes = new List<QuizViewModel>();

            sampleQuizzes.Add(
                new QuizViewModel(){
                    Id = 1,
                    Title ="How are you doing?",
                    Description="Anime related test",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate= DateTime.Now
                }
            );

            //addind other sample quizzes

            for (int i = 2; i < num; i++){
                sampleQuizzes.Add(
                    new QuizViewModel(){
                        Id = i,
                        Title = $"Sample quiz {i}",
                        Description = $"This is sample quiz",
                        CreatedDate= DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    }
                );
            }

            return new JsonResult(sampleQuizzes, new JsonSerializerSettings(){
                Formatting = Formatting.Indented,
            });
        }

        /// <summary>
        /// GET: api/quiz/ByTitle
        /// Retrieves the {num} Quizzes sorted by Title (A to Z)
        /// </summary>
        /// <param name="num">the number of quizzes to retrieve</param>
        /// <returns>{num} Quizzes sorted by Title</returns>

        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num =10){
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;

            return new JsonResult(
                sampleQuizzes.OrderBy(sampleQuizz => sampleQuizz.Title),
                new JsonSerializerSettings(){
                    Formatting = Formatting.Indented
                }
            );
        }

        /// <summary>
        /// GET: api/quiz/MostViewed
        /// Retrieves {num} randomQuizzes
        /// </summary>
        /// <param name="num">The number of quizzes to retrieve</param>
        /// <returns> {num} random Quizzes</returns>
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10){
            var sampleQuizzes = ((JsonResult)Latest(num)).Value as List<QuizViewModel>;

            return new JsonResult(
                sampleQuizzes.OrderBy(t => Guid.NewGuid()),
                new JsonSerializerSettings(){
                    Formatting = Formatting.Indented
                }
            );  
        }
    }
}