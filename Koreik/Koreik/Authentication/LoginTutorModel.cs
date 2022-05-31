using System.ComponentModel.DataAnnotations;

namespace Koreik.Authentication
{
    public class LoginTutorModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
