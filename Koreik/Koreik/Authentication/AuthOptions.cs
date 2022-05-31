using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Koreik.Authentication
{
    public class AuthOptions
    {
        public const string ISSUER = "https://localhost:7052"; // издатель токена
        public const string AUDIENCE = "https://localhost:7052/"; // потребитель токена
        const string KEY = "H+MbQeThWmZq4t7w";   // ключ для шифрации
        public const int LIFETIME = 200; // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
