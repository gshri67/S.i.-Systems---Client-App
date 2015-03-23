using SiSystems.ClientApp.SharedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface IMatchGuideApi
    {
        Task<ValidationResult> Login(string username, string password);

        Task Logout();

        Task<Consultant> GetConsultant(int id);

        Task<IEnumerable<ConsultantGroup>> GetAlumniConsultantGroups(string query);

        Task<IEnumerable<ConsultantGroup>> GetActiveConsultantGroups(string query);

        Task Submit(ContractProposal proposal);

        Task SendMessage(ConsultantMessage message);

        Task<Eula> GetMostRecentEula();

        Task<ClientAccountDetails> GetClientDetails();

        Task<ResetPasswordResult> ResetPassword(string emailAddress);
    }
}
