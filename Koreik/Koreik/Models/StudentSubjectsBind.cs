namespace Koreik.Models
{
    public class StudentSubjectsBind
    {
        public Guid StudentId { get; set; }
        public List<Guid> SubjectIds { get; set; }

    }
}
