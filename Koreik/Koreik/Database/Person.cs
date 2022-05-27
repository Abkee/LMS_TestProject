
using Microsoft.AspNetCore.Identity;

namespace Koreik.Database
{
    public abstract class Person : Entiti
    {
        public IdentityUser IdentityUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; protected set; }
    }
}
