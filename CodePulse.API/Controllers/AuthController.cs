using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        // POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]  LoginRequestDto request)
        {
            // Check Email
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                // Check Password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    // Create a Token and Response
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");


            return ValidationProblem(ModelState);
        }


        // POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            //Check if User Name or MailId already Exists
            var existingMail= await userManager.FindByEmailAsync(request.Email);
            var existingUserName = await userManager.FindByNameAsync(request.UserName);

            if (existingMail is not null)
            {
                ModelState.AddModelError("","Email Id already exists");
                return ValidationProblem(ModelState);
            }

            if (existingUserName is not null)
            {
                ModelState.AddModelError("", "UserName already exists");
                return ValidationProblem(ModelState);

            }

            // Create IdentityUser object
            var user = new IdentityUser
            {
                UserName = request.UserName?.Trim(),
                Email = request.Email?.Trim()
            };

            // Create User
            var identityResult = await userManager.CreateAsync(user, request.Password);  //UserManager class is from Identity which is used to create User

            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader"); ////UserManager class is from Identity which is used to assign role to user

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }

    }
}
