using System.Security.Cryptography;
using System.Text;

namespace TestBackend.Utilidades
{
    public class PasswordHash
    {
        public static string HashSha256(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();

        }
    }
}
