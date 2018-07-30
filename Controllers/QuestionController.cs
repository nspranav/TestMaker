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
    public class QuestionController : Controller
    {
        #region Private Fields
        private ApplicationDbContext DbContext;
        #endregion

        #region Constructor
        public QuestionController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        #endregion
        #region Restful conventions methods
        /// <summary>
        /// Retrieves the Question with the given {id}
        /// </summary>
        /// <paramas name="id"> The id of the existing Question</params>
        /// <returns> The Question with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions.Where(q => q.Id == id).FirstOrDefault();

            if (question == null)
            {
                return NotFound(
                    new
                    {
                        Error = $"Question with id {id} is not found"
                    }
                );
            }

            return new JsonResult(question.Adapt<QuestionViewModel>(), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        /// <summary>
        /// Inserts new Question to the Database
        /// </summary>
        /// <params name="m"> The Question view model containing the data t insert </params>
        [HttpPut]
        public IActionResult Put([FromBody]QuestionViewModel m)
        {
            //return a generic http status 500 (Server Error)
            //if the payload is invalid
            if (m == null)
            {
                return new StatusCodeResult(500);
            }
            //map the ViewModel to Model
            var question = m.Adapt<Question>();
            //override those parameters that should be set from the payload
            question.QuizId = m.QuizId;
            question.Text = m.Text;
            question.Notes = m.Notes;

            //properties set from server side
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;

            //add the new question
            DbContext.Questions.Add(question);
            //persist the changes to the database
            DbContext.SaveChanges();

            //return newly created question to the client
            return new JsonResult(question.Adapt<QuestionViewModel>(), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        /// <summary>
        /// Edit the Question with the given ID
        /// </summary>
        /// <params name="m">Question view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel m)
        {
            //return a generic http status 500(Server Error) if the client payload is invalid
            if (m == null)
            {
                return new StatusCodeResult(500);
            }

            //retieve the question to edit
            var question = DbContext.Questions.Where(q => q.Id == m.Id).FirstOrDefault();

            //Handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = $"Question ID {m.Id} has not been found"
                });
            }

            //handle the update without object mapping by manually assigning the properties
            //we want to accept from the request
            question.QuizId = m.QuizId;
            question.Text = m.Text;
            question.Notes = m.Notes;

            //properties set from the server side
            question.LastModifiedDate = question.CreatedDate;

            //persist the changes to the database
            DbContext.SaveChanges();

            //return the updated quiz to the client.
            return new JsonResult(question.Adapt<QuestionViewModel>(), new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
        }

        /// <sumary>
        /// Deletes the Question with the given id
        /// </summary>
        /// <params name="id"> The id of the Question to be deleted </params>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //retrieve the  question
            var question = DbContext.Questions.Where( q => q.Id == id).FirstOrDefault();

            if(question == null){
                return NotFound(new{
                    Error = $"Question Id {id} does not exist"
                });
            }

            //remove qquiz from dbcontext
            DbContext.Questions.Remove(question);

            //save changes to the database
            DbContext.SaveChanges();

            //return an HTTP Status 200
            return new OkResult();
        }
        #endregion
        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = DbContext.Questions.Where(q => q.QuizId == quizId).ToArray();

            return new JsonResult(questions.Adapt<QuestionViewModel[]>(), new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}