using System.Text;

namespace OneCoreTest.Common.Security
{
    public static class PasswordEncryptor
    {
        public static string EncryptPassword(string password)
        {
            // Get the bytes from password
            byte[] bytes = Encoding.ASCII.GetBytes(password);

            // Compute based on SHA256
            bytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(bytes);

            // Returns the hash
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
