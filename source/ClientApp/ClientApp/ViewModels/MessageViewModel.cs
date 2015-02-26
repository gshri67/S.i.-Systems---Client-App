using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private readonly IEmailService _emailService;
        public ConsultantMessage Message { get; set; }

        public MessageViewModel(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task<bool> SendMessage()
        {
            return _emailService.SendMessage(Message);
        }
    }
}
