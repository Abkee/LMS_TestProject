using Koreik.Database;
using Koreik.Database.Entities;
using Koreik.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Koreik.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TutorController(ApplicationContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Tutor")]
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

        [Authorize(Roles = "Tutor")]
        [HttpPost]
        [Route("api/putgrade")]
        public async Task<Guid> PutGrade(GradeModels gradeModel)
        {
            var grade = new Grade();
            grade.Id = gradeModel.Id;
            grade.grade = gradeModel.grade;
            grade.SubjectId = gradeModel.SubjectId;
            grade.StudentId = gradeModel.StudentId;
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return grade.Id;
        }
        /*
        [Authorize(Roles = "Tutor")]
        [HttpGet]
        [Route("api/showstudents")]
        public async Task<List<StudentModels>> ShowStudents()
        {
            
            var tutorname = User.Identity.Name;
            var tutorid = await _context.Tutors
                .Where(x => x.IdentityUser.UserName == tutorname)
                .Select(x=>x.Id)
                .SingleAsync();
            
            var students = from g in _context.Grades
                           where g.Tutor
                           
            return students;

        }
        */
        [Authorize(Roles = "Tutor")]
        [HttpGet]
        [Route("api/showsubjects")]
        public async Task<List<SubjectModels>> ShowSubjects()
        {
            var tutorname = User.Identity.Name;
            var tutorid = await _context.Tutors
                .Where(x => x.IdentityUser.UserName == tutorname)
                .Select(x => x.Id)
                .SingleAsync();

            var subjects = await (from s in _context.Subjects
                            where s.TutorId == tutorid
                            select new SubjectModels
                            {
                                Id = s.Id,
                                Name = s.Name,
                                TutorId = tutorid
                            }).ToListAsync();
                               

            return subjects;
        }

        [Authorize(Roles = "Tutor")]
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
