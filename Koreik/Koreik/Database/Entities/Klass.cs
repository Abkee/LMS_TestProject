namespace Koreik.Database.Entities
{
    public class Klass: Entiti
    {
        public string Name { get; set; }
        public Guid SchoolId { get; set; }
        public School School { get; set; }
        public List<Student> Students { get; set; } = new();

    }
}
