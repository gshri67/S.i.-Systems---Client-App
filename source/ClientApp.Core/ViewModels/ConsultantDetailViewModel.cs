using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Core.ViewModels
{
    public class ConsultantDetailViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;

        private Consultant _consultant;

        public bool IsLoading
        {
            get { return _consultant == null; }
        }

        public ConsultantDetailViewModel(IMatchGuideApi api)
        {
            this._api = api;
        }

        public async Task<Consultant> GetConsultant(int id)
        {
            if (_consultant != null && _consultant.Id == id)
            {
                return _consultant;
            }

            _consultant = await _api.GetConsultant(id);
            return _consultant;
        }

        public Consultant GetConsultant()
        {
            return _consultant ?? new Consultant();
        }

        public bool IsActiveConsultant { get; set; }
    }
}
