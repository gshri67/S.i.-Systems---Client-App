using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Context;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class ConsultantMessageService
    {
        private readonly IConsultantRepository _consultantRepository;
        private readonly ICompanyRepository _companyRepository;

        private readonly ISessionContext _sessionContext;

        public ConsultantMessageService(IConsultantRepository consultantRepository, 
            ICompanyRepository companyRepository, ISessionContext context)
        {
            _consultantRepository = consultantRepository;
            _companyRepository = companyRepository;
            _sessionContext = context;
        }

        public void SendMessage(ConsultantMessage message)
        {
            //Find consultant to fetch their email address.
            var consultant = _consultantRepository.Find(message.ConsultantId);

            AssertCurrentUserCanAccessConsultantRecord(consultant);
            
            var mailService = new SendGridMailService();
            mailService.SendTemplatedEmail(Settings.ContactAlumniTemplateId,
                consultant.EmailAddress, _sessionContext.CurrentUser.Login,
                message.Text,
                new Dictionary<string, string>
                {
                    { "-clientContactFullName-", _sessionContext.CurrentUser.FullName },
                    { "-clientCompanyName-", _sessionContext.CurrentUser.CompanyName }
                },
                new []{"Contact Alumni"});
        }

        /// <summary>
        /// Validate that current user works for or is associated with a company
        /// that the consultant has worked for
        /// </summary>
        private void AssertCurrentUserCanAccessConsultantRecord(Consultant consultant)
        {
            var associatedCompanyIds =
                _companyRepository.GetAllAssociatedCompanyIds(_sessionContext.CurrentUser.ClientId);

            if (consultant != null)
            {
                consultant.Contracts =
                    consultant.Contracts.Where(c => associatedCompanyIds.Contains(c.ClientId)).ToList();
            }

            if (consultant != null && !consultant.Contracts.Any())
                throw new UnauthorizedAccessException();
        }
    }
}
