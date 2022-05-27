namespace Koreik.Database.Entities
{
    public class Subject: Entiti
    {
        public string Name { get; set; }

        public Guid TutorId { get; set; }
        public Tutor Tutor { get; set; }
        //public Tutor? TutorId { get; set; }
        public List<Student> Students { get; set; } = new();
    }
}
