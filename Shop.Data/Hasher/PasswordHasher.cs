using System.Security.Cryptography; 

namespace Shop.Data.Helpers
{
    public static class PasswordHasher
    {
        private const int Iterations = 10000;
        private const int KeySize = 32; 

        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        public static (string passwordHash, string passwordSalt) HashPassword(string password)
        {
           
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password, 
                 salt, 
                 Iterations, 
                 HashAlgorithm, 
                 KeySize);

                return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }
        

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
         
                byte[] salt = Convert.FromBase64String(storedSalt);
                byte[] expectedHash = Convert.FromBase64String(storedHash);

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password, 
                    salt, 
                    Iterations, 
                    HashAlgorithm, 
                    KeySize);
                    return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}