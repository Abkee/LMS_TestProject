namespace Koreik.Models
{
    public class GradeModels
    {
        public Guid Id { get; set; }
        public int grade { get; set; }
        public Guid SubjectId { get; set; }
        public SubjectModels Subject { get; set; }
        public Guid StudentId { get; set; }
        public StudentModels Student { get; set; }
    }
}
