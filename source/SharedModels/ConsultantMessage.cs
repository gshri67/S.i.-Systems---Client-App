﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.SharedModels
{
    public class ConsultantMessage
    {
        public int SenderUserId { get; set; }
        public int RecipientConsultantId { get; set; }
        public string Text { get; set; }
    }
}
