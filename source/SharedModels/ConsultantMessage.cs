using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{
    public class ConsultantMessage
    {
        public int ToEmailAddress { get; set; }
        public int FromEmailAddress { get; set; }
        public string Text { get; set; }
    }
}
