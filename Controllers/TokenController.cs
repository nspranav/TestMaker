using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TestMakerFree.Data;
using System.Reflection.Metadata;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TestMakerFree.ViewModels;

namespace TestMakerFree.Controllers
{
    public class TokenController : BaseApiController
    {
        #region Private Members
        #endregion Private Members

        #region Constructor
            public TokenController(ApplicationDbContext dbContext,
                RoleManager<IdentityRole> roleManager,
                UserManager<ApplicationUser> userManager,
                IConfiguration configuration) : base(dbContext, roleManager, userManager, configuration){}
        #endregion

        [HttpPost("Auth")]
        public async Task<IActionResult> Jwt([FromBody] TokenRequestViewModel model){
            //return a generic HTTP status 500 (server error)
            //if the client payload is invalid.
            if(model == null){
                return new StatusCodeResult(500);
            }

            switch (model.grant_type)
            {
                case "password": 
                    return await GetToken(model);
                default:
                    //not supported - return a HTTp 401 (Unauthorized)
                    return  new UnauthorizedResult();
            }
        }

        private async Task<IActionResult> GetToken(TokenRequestViewModel model){
            try{
                //check if there's an user with the given username
                var user = await UserManager.FindByNameAsync(model.username);
                //fallback to support email address instead of username
                if(user == null && model.username.Contains("@"))
                    user  = await UserManager.FindByEmailAsync(model.username);

                if(user == null || !await UserManager.CheckPasswordAsync(user,model.password)){
                    //user does not exists or password mismatch
                    return new UnauthorizedResult();
                }

                //username & password matches: create and return the JWT token.

                DateTime now = DateTime.UtcNow;

                //add the registered claims for JWT (RFC7519).
                //For more info, see https://tools.ietf.org/html/rfc7519#section-4.1 
                var claims = new[]{
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
                    //TODO: add additional claims here
                };

                var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: Configuration["Auth:Jwt:Issuer"],
                    audience: Configuration["Auth:Jwt:Audience"],
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
                    signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

                //build & return the response
                var response = new TokenResponseViewModel{
                    token = encodedToken,
                    expiration = tokenExpirationMins
                };
                return Json(response);
            }
            catch (Exception)
            {
                return new UnauthorizedResult();
            }
        }
    }
}