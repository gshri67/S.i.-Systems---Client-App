using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantApp.Core
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public ValidationResult() { }

        public ValidationResult(bool isValid)
        {
            this.IsValid = isValid;
        }

        public ValidationResult(bool isValid, string message)
            : this(isValid)
        {
            this.Message = message;
        }
    }
}
