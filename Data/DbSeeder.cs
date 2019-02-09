using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TestMakerFree.Data
{

    public static class DbSeeder
    {
        #region Public Methods
        public static void Seed(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager,
                    UserManager<ApplicationUser> userManager)
        {
            //create default users if there are none
            if (!dbContext.Users.Any())
            {
                CreateUsers(dbContext, roleManager, userManager).GetAwaiter().GetResult();
            }
            //create default Quizzes (if there are none) together with their set of Q&A
            if (!dbContext.Quizzes.Any())
            {
                CreateQuizzes(dbContext);
            }
        }
        #endregion
        #region Seed Methods
        private static async Task CreateUsers(ApplicationDbContext dbContext, 
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            DateTime createdDate = new DateTime(2017, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            string role_Administrator = "Administrator";
            string role_RegisteredUser = "RegisteredUser";

            //Create Roles (if they don't exist yet)
            if(!await roleManager.RoleExistsAsync(role_Administrator)){
                await roleManager.CreateAsync(new IdentityRole(role_Administrator));
            }

            if(!await roleManager.RoleExistsAsync(role_RegisteredUser)){
                await roleManager.CreateAsync(new IdentityRole(role_RegisteredUser));
            }

            //cREATE aDMIN ACCOUNT IF IT DOESN'T EXIST ALREADY
            var user_ADMIN = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "admin@testmaker.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            //Inserting the admin into the Database and assign the "Administrator" and
            //"RegisteredUser" role to him.
            if(await userManager.FindByNameAsync(user_ADMIN.UserName) == null){
                await userManager.CreateAsync(user_ADMIN, "Pass4Admin!!");
                await userManager.AddToRoleAsync(user_ADMIN,role_RegisteredUser);
                await userManager.AddToRoleAsync(user_ADMIN,role_Administrator);

                //Remove Lockout and E-Mail confirmation
                user_ADMIN.EmailConfirmed = true;
                user_ADMIN.LockoutEnabled = false;
            }

#if DEBUG
            //Create some sample registered user accounts (if they don't exist already)
            var user_Ryan = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Ryan",
                Email = "ryan@testmaker.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var user_Solice = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Solice",
                Email = "solice@testmaker.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var user_Vodan = new ApplicationUser
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "Vodan",
                Email = "vodan@testmaker.com",
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            };

            var user = await userManager.FindByNameAsync(user_Ryan.UserName);
            //Insert sample registered users into the database and also assign the "Registered" role to him.
            if(user == null){
                var identityResult = await userManager.CreateAsync(user_Ryan,"Pass4Ryan!");
                if(identityResult.Succeeded){
                    await userManager.AddToRoleAsync(user_Ryan,role_RegisteredUser);

                    //remove lockout and email confirmation
                    user_Ryan.EmailConfirmed = true;
                    user_Ryan.LockoutEnabled = false;
                }
            }

            if(await userManager.FindByNameAsync(user_Solice.UserName) == null){
                await userManager.CreateAsync(user_Solice,"Pass4Solice!");
                await userManager.AddToRoleAsync(user_Solice,role_RegisteredUser);

                //remove lockout and email confirmation
                user_Solice.LockoutEnabled = false;
                user_Solice.EmailConfirmed = true;
            }

            if(await userManager.FindByNameAsync(user_Vodan.UserName) == null){
                await userManager.CreateAsync(user_Vodan,"Pass4Vodan!");
                await userManager.AddToRoleAsync(user_Vodan,role_RegisteredUser);

                //remove lockout and email confirmation
                user_Vodan.EmailConfirmed = true;
                user_Vodan.LockoutEnabled = false;
            }
#endif
            await dbContext.SaveChangesAsync();

        }

        private static void CreateQuizzes(ApplicationDbContext dbContext)
        {
            //local Variables
            DateTime createdDate = new DateTime(2017, 03, 01, 12, 30, 00);
            DateTime lastModifiedDate = DateTime.Now;

            //retrieve the admin user which we'll use as a default author
            var authorId = dbContext.Users
                                    .Where(u => u.UserName == "Admin")
                                    .FirstOrDefault()
                                    .Id;
#if DEBUG
            //creating 47 sample quizzes with auto generated data
            //including questions answers and results
            var num = 47;
            for (int i = 1; i <= num; i++)
            {
                CreateSampleQuiz(dbContext, i, authorId, num - i, 3, 3, 3, createdDate.AddDays(-num));
            }
#endif
            //create 3 more quizzes with better descriptive data
            //(we'll add the questions, answers & results later on)
            EntityEntry<Quiz> e1 = dbContext.Quizzes.Add(new Quiz
            {
                UserId = authorId,
                Title = "Are you more Light or Dark side of the Force?",
                Description = "SW personality test",
                Text = @"Choose wisely you must, young padawan: ",
                ViewCount = 2343,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Quiz> e2 = dbContext.Quizzes.Add(new Quiz
            {
                UserId = authorId,
                Title = "GenX, GenY, or Genz?",
                Description = "Find out what decade most represents you",
                Text = @"Do you feel comfortable in your generation? " +
                        "What year should you have been born in?" +
                        "Here's a bunch of questions that wil help you to find out!",
                ViewCount = 4180,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            EntityEntry<Quiz> e3 = dbContext.Quizzes.Add(new Quiz
            {
                UserId = authorId,
                Title = "Which Shingeki No Kyojin character are you?",
                Description = "Attack On Titan personality test",
                Text = @"Do you relentlessly seek revenge like Erin? " +
                        "Are you willing to put you like on the stake to protect your friends like Mikasa? " +
                        "Would you trust you fighting skills like Levi? " +
                        "or rely on your strategies and tactics like Arwin? " +
                        "Unviel your true self with Attack on titan personality test!",
                ViewCount = 5203,
                CreatedDate = createdDate,
                LastModifiedDate = lastModifiedDate
            });

            //persist the changes to the db 
            dbContext.SaveChanges();
        }
        #endregion
        #region Utility Methods
        /// <summary>
        /// Creates a sample quiz and add it to the database
        /// together with a sample set of questions, answers, and results.
        /// </summary>
        /// <param name="userId"> the author Id</param>
        /// <param name="id">the quiz id</param>
        /// <param name="createdDate">the quiz created date</param> 
        private static void CreateSampleQuiz(
            ApplicationDbContext dbContext,
            int num,
            string authorId,
            int viewcount,
            int numberOfQuestions,
            int numberOfAnswersPerQuestion,
            int numberOfResults,
            DateTime createdDate)
        {
            var quiz = new Quiz
            {
                UserId = authorId,
                Title = string.Format($"Quiz {num} Title"),
                Description = $"This is a sample description for the quiz {num}",
                Text = $"This is a sample quiz creatd by the DBSeeder class for testing purposes. ",
                ViewCount = viewcount,
                CreatedDate = createdDate,
                LastModifiedDate = createdDate
            };
            dbContext.Quizzes.Add(quiz);
            dbContext.SaveChanges();

            for (int i = 0; i < numberOfQuestions; i++)
            {
                var question = new Question
                {
                    QuizId = quiz.Id,
                    Text = @"This is a sample question created by the DbSeeder class for testing purposes. " +
                            "All the child answers are quto generated as well",
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                };
                dbContext.Questions.Add(question);
                dbContext.SaveChanges();

                for (int i2 = 0; i2 < numberOfAnswersPerQuestion; i2++)
                {
                    var e2 = dbContext.Answers.Add(new Answer
                    {
                        QuestionId = question.Id,
                        Text = "This is a sample answer created by the DbSeeder class for testing purposes.",
                        Value = i2,
                        CreatedDate = createdDate,
                        LastModifiedDate = createdDate
                    });
                }
            }

            for (int i = 0; i < numberOfResults; i++)
            {
                dbContext.Results.Add(new Result
                {
                    QuizId = quiz.Id,
                    Text = "This is a sample result created by the DbSeeder class for testing purposes.",
                    MinValue = 0,
                    //max value should be equal to answers number * max answer value
                    MaxValue = numberOfAnswersPerQuestion * 2,
                    CreatedDate = createdDate,
                    LastModifiedDate = createdDate
                });
            }
            dbContext.SaveChanges();
        }
        #endregion
    }
}