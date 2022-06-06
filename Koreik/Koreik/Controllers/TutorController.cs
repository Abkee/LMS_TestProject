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
        [Authorize(Roles = "Tutor")]
        [HttpGet]
        [Route("api/showstudents")]
        public async Task<List<StudentModels>> ShowStudents([FromBody]Guid subjectId)
        {
            
            var tutorname = User.Identity.Name;
            var tutorid = await _context.Tutors
                .Where(x => x.IdentityUser.UserName == tutorname)
                .Select(x=>x.Id)
                .SingleAsync();
            var students = await _context.Subjects
                .Include(x => x.Students)
                .Where(s => s.TutorId==tutorid && s.Id == subjectId)
                .SingleAsync();

            return students.Students.Select(x => new StudentModels
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname
            }).ToList();

        }
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
        [HttpGet]
        [Route("api/profile")]
        public async Task<ProfileModel> ProfileGet()
        {
            var tutorname = User.Identity.Name;
            var tutorid = await _context.Tutors
                .Where(x => x.IdentityUser.UserName == tutorname)
                .Select(x => x.Id)
                .SingleAsync();

            var profile = await (from s in _context.Tutors
                                 where s.Id == tutorid
                                 select new ProfileModel
                                 {
                                     Name = s.Name,
                                     Surname = s.Surname,
                                 }).FirstOrDefaultAsync();
            return profile;
        }

        [Authorize(Roles = "Tutor")]
        [HttpPost]
        [Route("api/update/profile")]
        public async Task<Guid> ProfilePost(ProfileModel model)
        {
            var tutorname = User.Identity.Name;
            var tutorid = await _context.Tutors
                .Where(x => x.IdentityUser.UserName == tutorname)
                .Select(x => x.Id)
                .SingleAsync();

            var tutor = await _context.Tutors
                .Where(x => x.Id == tutorid)
                .SingleAsync();
            tutor.Name = model.Name;
            tutor.Surname = model.Surname;
            _context.SaveChanges();
            return tutor.Id;
        }
    }
}
