using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{
    public class ValidationResult
    {
        public ValidationResult(bool isValid)
        {
            IsValid = isValid;
        }

        public ValidationResult(bool isValid, string message)
        {
            IsValid = isValid;
            Message = message;
        }

        public ValidationResult()
        {
        }

        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
