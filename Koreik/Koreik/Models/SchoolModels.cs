namespace Koreik.Models
{
    public class SchoolModels
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TutorModels> Tutors { get; set; } = new();
        public List<KlassModels> Klases { get; set; } = new();

    }
}
