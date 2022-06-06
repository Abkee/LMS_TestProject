using Koreik.Database;
using Koreik.Database.Entities;
using Koreik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Koreik.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public StudentController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("api/showsubjects")]
        public async Task<List<SubjectModels>> ShowSubjects()
        {
            var studentname = User.Identity.Name;
            var studentid = await _context.Students
                .Where(x => x.IdentityUser.UserName == studentname)
                .Select(x => x.Id)
                .SingleAsync();

            var subjects = await _context.Students
                .Include(s => s.Subjects)
                .SingleAsync(x => x.Id==studentid);
            
            return subjects.Subjects.Select(s => new SubjectModels
            {
                Id = s.Id,
                Name = s.Name,
            }).ToList();
        }
        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("api/showgrades")]
        public async Task<List<GradeModels>> GetGrade([FromBody]string subject)
        {
            var studentname = User.Identity.Name;
            var studentid = await _context.Students
                .Where(x => x.IdentityUser.UserName == studentname)
                .Select(x => x.Id)
                .SingleAsync();

            var grades = await (from sub in _context.Grades
                                where sub.StudentId == studentid && sub.Subject.Name == subject
                                select new GradeModels
                                {
                                    Id = sub.Id,
                                    grade = sub.grade,
                                    NameSubject = sub.Subject.Name,
                                }).ToListAsync();

            return grades;
        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        [Route("api/profile")]
        public async Task<ProfileModel> ProfileGet()
        {
            var studentname = User.Identity.Name;
            var studentid = await _context.Students
                .Where(x => x.IdentityUser.UserName == studentname)
                .Select(x => x.Id)
                .SingleAsync();

            var profile = await (from s in _context.Students
                                 where s.Id == studentid
                                 select new ProfileModel
                                 {
                                     Name = s.Name,
                                     Surname = s.Surname,                                 
                                 }).FirstOrDefaultAsync();
            return profile;
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [Route("api/update/profile")]
        public async Task<Guid> ProfilePost(ProfileModel model)
        {        
            var studentname = User.Identity.Name;
            var studentid = await _context.Students
                .Where(x => x.IdentityUser.UserName == studentname)
                .Select(x => x.Id)
                .SingleAsync();

            var student = await _context.Students
                .Where(x => x.Id == studentid)
                .SingleAsync();                                          
            student.Name = model.Name;
            student.Surname = model.Surname;
            _context.SaveChanges();
            return student.Id;
        }
    }
}
