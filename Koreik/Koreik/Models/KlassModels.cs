namespace Koreik.Models
{
    public class KlassModels
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public Guid SchoolId { get; set; }
        public SchoolModels? School { get; set; }
        public List<StudentModels> Students { get; set; } = new();
    }
}
