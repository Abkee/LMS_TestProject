namespace Koreik.Database.Entities
{
    public class School: Entiti
    {
        public string Name { get; set; }
        public List<Tutor> Tutors { get; set; } = new();
        public List<Klass> Klases { get; set; } = new();
    }
}
