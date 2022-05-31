namespace Koreik.Models
{
    public class TutorModels
    {
        public string Name { get; set; } 
        public string Surname { get; set; }
        public Guid Id { get; set; }
        public Guid SchoolId { get; set; }
        public SchoolModels School { get; set; }
        //public Subject? Subject { get; set; }

        public List<SubjectModels> Subjects { get; set; }
    }
}
