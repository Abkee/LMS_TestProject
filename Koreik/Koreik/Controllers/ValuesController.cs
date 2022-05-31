using Koreik.Database;
using Koreik.Database.Entities;
using Koreik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Koreik.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //private readonly UserManager<TutorModels> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationContext _context;

        public ValuesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<SchoolModels>> Get()
        {
            var schools = await _context.Schools.Select(c => 
            new SchoolModels
            {
                Id = c.Id,
                Name = c.Name,
            }).ToListAsync();

            return schools;
        }

        [HttpPost]
        [Route("api/create/school")]
        public async Task<Guid> CreateSchool(SchoolModels schoolModel)
        {
            var school = new School();
            school.Name = schoolModel.Name;
            await _context.Schools.AddAsync(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }

        [HttpPost]
        [Route("api/create/klass")]
        public async Task<Guid> CreateKlass(KlassModels klassModel)
        {
            var klass = new Klass();
            klass.Name = klassModel.Name;
            klass.SchoolId = klassModel.SchoolId;
            await _context.Klasses.AddAsync(klass);
            await _context.SaveChangesAsync();
            return klass.Id;
        }
        
        
        [HttpGet]
        [Route("api/subjects")]
        public async Task<IEnumerable<TutorModels>> GetSubjects()
        {
            var tutors = await _context.Tutors.Select(c =>
            new TutorModels
            {
                Id=c.Id,
                Name =c.Name,
                Surname = c.Surname,
                //School = c.School,

            }).ToListAsync();

            return tutors;
        }
        //[Authorize(Roles = "tutor")]
        /*
        //[HttpPost]
        //[Route("api/register/tutor")]
       
        public async Task<IActionResult> Register([FromBody] TutorModels model)
        {
            var userExists = await userManager.FindByNameAsync(model.Name);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        */
    }
}
