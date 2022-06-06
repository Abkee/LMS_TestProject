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
    public class AdminController : ControllerBase
    {
        //private readonly UserManager<TutorModels> userManager;
        //private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationContext _context;

        public AdminController(ApplicationContext context)
        {
            _context = context;
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
        [AllowAnonymous]
        [Route("api/bind-sub-to-stu")]
        public async Task BindSubjectsToStudent([FromBody] StudentSubjectsBind model)
        {
            var student = await _context.Students
                .Include(s => s.Subjects)
                .SingleAsync(x => x.Id == model.StudentId);

            var subjects = await _context.Subjects
                .Where(x => model.SubjectIds.Contains(x.Id)).ToListAsync();

            student.Subjects.AddRange(subjects);
            await _context.SaveChangesAsync();

        }
        [HttpPost]
        [Route("api/create/subject")]
        public async Task<Guid> CreateSubject(SubjectModels subjectModel)
        {
            var subject = new Subject();
            subject.Id = subjectModel.Id;
            subject.Name = subjectModel.Name;
            subject.TutorId = subjectModel.TutorId;
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
            return subject.Id;
        }

    }
}
