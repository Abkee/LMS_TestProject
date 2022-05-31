namespace Koreik.Database.Entities
{
    public class Tutor : Person
    {
        public Tutor ()
        {
            Role = "Tutor";
        }
        public Guid SchoolId { get; set; }
        public School? School { get; set; }
        //public Subject? Subject { get; set; }

        public List<Subject> Subject { get; set; }

    }
}
