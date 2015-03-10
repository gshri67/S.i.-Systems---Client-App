using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{
    public class ResetPasswordResult
    {
        public bool IsSuccess { get { return ResponseCode > 0; } }

        public string Description { get; set; }

        public int ResponseCode { get; set; }
    }
}
