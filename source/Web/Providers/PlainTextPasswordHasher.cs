using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace SiSystems.ClientApp.Web.Providers
{
    /// <summary>
    /// TODO: This will need to be replaced with a real hashing algorithm
    /// </summary>
    public class PlainTextPasswordHasher: PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return password;
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return hashedPassword==providedPassword ? PasswordVerificationResult.Success :  PasswordVerificationResult.Failed;
        }
    }
}
