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
    public class QuizController : BaseApiController
    {
        #region Constructor
        public QuizController(ApplicationDbContext dbContext) : base(dbContext) { }
        #endregion Constructor

        #region Restful convention method
        /// <summary>
        /// GET: api/quiz/{}id
        /// Retrieves the quiz with given Id
        /// </summary>
        /// <param name="id"> The id of the quiz</param>
        /// <returns> quiz with the given id</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var quiz = DbContext.Quizzes.Where(i => i.Id == id).FirstOrDefault();

            //handling requests for non-existent quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz Id {id} is not found"
                });
            }
            return new JsonResult(quiz.Adapt<QuizViewModel>(), JsonSettings);
        }

        /// <summary>
        /// Inserts new Quiz to the Database
        /// </summary>
        /// <params name="m"> The Quiz view model containing the data to insert </params>
        [HttpPut]
        public IActionResult Put([FromBody]QuizViewModel m)
        {
            //return a generic HTTP Status 500 (server error)
            //if the client payload is invalid
            if (m == null) return new StatusCodeResult(500);

            //handle the insert (without object-mapping)
            var quiz = new Quiz();

            //properties taken from the request
            quiz.Title = m.Title;
            quiz.Description = m.Description;
            quiz.Text = m.Text;
            quiz.Notes = m.Notes;

            //properties set from server-side
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;

            //Set a temporary author using the Admin user's userId
            //as User login isn't supported yet
            //TODO: add user login
            quiz.UserId = DbContext.Users.Where(u => u.UserName == "Admin").FirstOrDefault().Id;

            //add the new quiz
            DbContext.Quizzes.Add(quiz);

            //persist the changes into the database
            DbContext.SaveChanges();

            //return newly created quiz to the client
            return new JsonResult(quiz.Adapt<QuizViewModel>(),
            JsonSettings);
        }

        /// <summary>
        /// Edit the Quiz with the given ID
        /// </summary>
        /// <params name="m">Quiz view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post([FromBody]QuizViewModel m)
        {
            //return a generic HTTP Status 500 (server Error)
            //if the client payload is invalid.
            if (m == null) new StatusCodeResult(500);

            //retrieve the quiz to edit
            var quiz = DbContext.Quizzes.Where(q => q.Id == m.Id).FirstOrDefault();

            //handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {m.Id} is not found"
                });
            }

            //handle the update (without object-mapping)
            // by manually assigning the properties, we want to
            //accept from the request
            quiz.Title = m.Title;
            quiz.Description = m.Description;
            quiz.Text = m.Text;
            quiz.Notes = m.Notes;

            //properties set from server side
            quiz.LastModifiedDate = quiz.CreatedDate;

            //persist the changes into the Database
            DbContext.SaveChanges();

            //return the updated quiz to the client
            return new JsonResult(quiz.Adapt<QuizViewModel>(),
            JsonSettings);
        }

        /// <sumary>
        /// Deletes the Quiz with the given id
        /// </summary>
        /// <params name="id"> The id of the Quiz to be deleted </params>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quiz = DbContext.Quizzes.Where(q => q.Id == id).FirstOrDefault();

            //handling requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = $"Quiz ID {id} is not found"
                });
            }

            //remove the quiz from the DbContext
            DbContext.Quizzes.Remove(quiz);
            //persist the changes to the Database
            DbContext.SaveChanges();

            //return anHTTP Status 200 (OK)
            return new JsonResult(new { Ok = "Ok" }, JsonSettings);
        }
        #endregion
        [HttpGet("Latest/{num?}")]
        public IActionResult Latest(int num = 10)
        {
            var latestQuizzes = DbContext.Quizzes.OrderByDescending(d => d.CreatedDate).Take(num).ToArray();

            return new JsonResult(latestQuizzes.Adapt<QuizViewModel[]>(), JsonSettings);
        }

        /// <summary>
        /// GET: api/quiz/ByTitle
        /// Retrieves the {num} Quizzes sorted by Title (A to Z)
        /// </summary>
        /// <param name="num">the number of quizzes to retrieve</param>
        /// <returns>{num} Quizzes sorted by Title</returns>

        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = DbContext.Quizzes.OrderBy(q => q.Title).Take(num).ToArray();

            return new JsonResult(
                byTitle.Adapt<QuizViewModel[]>(),JsonSettings);
        }

        /// <summary>
        /// GET: api/quiz/MostViewed
        /// Retrieves {num} randomQuizzes
        /// </summary>
        /// <param name="num">The number of quizzes to retrieve</param>
        /// <returns> {num} random Quizzes</returns>
        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = DbContext.Quizzes.OrderBy(q => Guid.NewGuid()).Take(num).ToArray();

            return new JsonResult(
                random.Adapt<QuizViewModel[]>(),
                JsonSettings
            );
        }
    }
}