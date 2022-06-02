using Koreik.Database;
using Koreik.Database.Entities;
using Koreik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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


            var subjects = await (from sub in _context.Subjects
                           where sub.Id == studentid
                           select new SubjectModels
                           {
                               Id = sub.Id,
                               Name = sub.Name
                               
                           }).ToListAsync();

            return subjects;
        }
        [Authorize(Roles ="Student")]
        [HttpGet]
        [Route("api/showgrades")]
        public async Task<List<GradeModels>> GetGrade()
        {
            var studentname = User.Identity.Name;
            var studentid = await _context.Students
                .Where(x => x.IdentityUser.UserName == studentname)
                .Select(x => x.Id)
                .SingleAsync();

            var grades = await (from sub in _context.Grades
                            where sub.StudentId == studentid
                            select new GradeModels
                            {
                                Id = sub.Id,
                                grade = sub.grade,
                                NameSubject = sub.Subject.Name,
                            }).ToListAsync();
            
            return grades;
        }       
    }
}
