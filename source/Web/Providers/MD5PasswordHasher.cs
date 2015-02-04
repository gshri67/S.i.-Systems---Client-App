using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity;

namespace SiSystems.ClientApp.Web.Providers
{
    public class Md5PasswordHasher: PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return CalculateMd5Hash(password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var match = string.Compare(HashPassword(providedPassword), hashedPassword,
                StringComparison.CurrentCultureIgnoreCase)==0;
            return match ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }

        public string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
