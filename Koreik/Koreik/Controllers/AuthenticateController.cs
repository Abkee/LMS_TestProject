using Koreik.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Koreik.Authentication;
using Koreik.Database;
using Newtonsoft.Json;
using Koreik.Models;
using Koreik.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Koreik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationContext _applicationDbContext;
        public AuthenticateController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationContext applicationDbContext, SignInManager<IdentityUser> signInManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _applicationDbContext = applicationDbContext;
            _signInManager = signInManager;
        }
        [Authorize]
        [HttpGet]
        [Route("GetTutors")]
        public async Task<IEnumerable<Tutor>> GetTest()
        {
            var username = User.Identity.Name;
            return await _applicationDbContext.Tutors.ToListAsync();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")] 
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = _userManager.GetRolesAsync(user)
                }) ;
            }
            var result =
                await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            return Unauthorized();
        }
        
        [HttpPost]
        [Route("register/tutor")]
        public async Task<IActionResult> RegisterTutor([FromBody] TutorRegisterModel model)
        {
            await _roleManager.CreateAsync(new IdentityRole("Tutor"));
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            //var newUser = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Tutor");

            var tutor = new Tutor();
            tutor.Name = "Gulzhan";
            tutor.Surname = "Rahimova";
            tutor.SchoolId = model.SchoolId;
            tutor.IdentityUser = user;
            await _applicationDbContext.Tutors.AddAsync(tutor);
            await _applicationDbContext.SaveChangesAsync();
            
            
            //if (!newUser.Succeeded)
            //    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        
        [HttpPost]
        [Route("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterModel model)
        {
            await _roleManager.CreateAsync(new IdentityRole("Student"));
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Student");

            var student = new Student();
            student.Name = "Aibek";
            student.Surname = "Esenpulov";
            student.KlassId = model.KlassId;
            student.IdentityUser = user;
            await _applicationDbContext.Students.AddAsync(student);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        
        }
        
    }
}
