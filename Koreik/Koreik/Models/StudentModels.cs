namespace Koreik.Models
{
    public class StudentModels
    {
        public Guid Id { get; set; }
        public List<GradeModels> Grades { get; set; } = new();
        public List<SubjectModels> Subjects { get; set; } = new();
        public Guid KlassId { get; set; }
        public KlassModels Klass { get; set; }
    }
}
