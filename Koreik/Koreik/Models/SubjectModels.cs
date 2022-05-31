namespace Koreik.Models
{
    public class SubjectModels
    {
        public Guid Id { get; set; }
        public Guid TutorId { get; set; }
        public TutorModels Tutor { get; set; }
        //public Tutor? TutorId { get; set; }
        public List<StudentModels> Students { get; set; } = new();
    }
}
