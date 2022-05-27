namespace Koreik.Database.Entities
{
    public class Student: Person
    {
        public Student()
        {
            Role = "Student";
        }
        public List<Grade> Grades { get; set; } = new();
        public List<Subject> Subjects { get; set; } = new();
        // public Klass? Klass { get; set; }
        public Guid KlassId { get; set; }
        public Klass Klass { get; set; }

    }
}
