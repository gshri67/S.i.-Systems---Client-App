using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        public ConsultantMessage Message { get; set; }

        public Task<bool> SendMessage()
        {
            //TODO
            return Task.FromResult(false);
        }
    }
}
