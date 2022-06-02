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
    }
}
