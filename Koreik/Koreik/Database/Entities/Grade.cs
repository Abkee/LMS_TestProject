namespace Koreik.Database.Entities
{
    public class Grade : Entiti
    { 
        public int grade { get; set; }
        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        //public Subject? SubjectId { get; set; }
        //public Student? StudentId { get; set; }

    }
}
