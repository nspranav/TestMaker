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
    public class AnswerController : BaseApiController{

        #region Constructor
            public AnswerController(ApplicationDbContext dbContext,
                RoleManager<IdentityRole> roleManager,
                UserManager<ApplicationUser> userManager,
                IConfiguration configuration): base(dbContext,roleManager,userManager,configuration){}
        #endregion

        #region Restful conventions methods
        /// <summary>
        /// Retrieves the answer with the given {id}
        /// </summary>
        /// <paramas name="id"> The id of the existing answer</params>
        /// <returns> The answer with the given {id}</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            var answer = DbContext.Answers.Where( a => a.Id == id).FirstOrDefault();

            if(answer == null){
                return NotFound(new{
                    Error = $"Answer with id {id} does not exist"
                });
            }

            return new JsonResult(answer.Adapt<AnswerViewModel>(), new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });
        }

        /// <summary>
        /// Inserts new Answer to the Database
        /// </summary>
        /// <params name="m"> The answer view model containing the data t insert </params>
        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel m){
            if(m == null){
                return new StatusCodeResult(500);
            }

            var answer = m.Adapt<Answer>();

            answer.QuestionId = m.QuestionId;
            answer.Text = m.Text;
            answer.Notes = m.Notes;

            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;

            DbContext.Answers.Add(answer);
            DbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>(), new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });

        }

        /// <summary>
        /// Edit the answer with the given ID
        /// </summary>
        /// <params name="m">Answer view model containing the data to update </params>
        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel m){
            if(m ==null){
                return new StatusCodeResult(500);
            }

            var answer  = DbContext.Answers.Where(a => a.Id == m.Id).FirstOrDefault();

            if(answer == null){
                return NotFound(new{
                    Error = $"Answer with id {m.Id} is not found"
                });
            }
            
            answer.QuestionId = m.QuestionId;
            answer.Text = m.Text;
            answer.Value = m.Value;
            answer.Notes = m.Notes;

            answer.LastModifiedDate = answer.CreatedDate;

            DbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>(), new JsonSerializerSettings{
                Formatting = Formatting.Indented
            });
            
        }


        /// <sumary>
        /// Deletes the answer with the given id
        /// </summary>
        /// <params name="id"> The id of the answer to be deleted </params>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            var answer  = DbContext.Answers.Where(a => a.Id == id).FirstOrDefault();

            if(answer == null){
                return NotFound(new{
                    Error = $"Answer with id {id} is not found"
                });
            }
            DbContext.Answers.Remove(answer);
            DbContext.SaveChanges();
            return new OkResult();
        } 
        #endregion
        
        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId){
            var answers = DbContext.Answers.Where(a => a.QuestionId == questionId).ToArray();


            return new JsonResult(answers.Adapt<AnswerViewModel[]>(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
        }
    }
}