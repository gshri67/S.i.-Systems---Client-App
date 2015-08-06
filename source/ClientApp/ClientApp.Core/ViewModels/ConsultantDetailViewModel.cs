using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Core.ViewModels
{
    public class ConsultantDetailViewModel : ViewModelBase
    {
        private readonly IMatchGuideApi _api;

        private Consultant _consultant;

        public string Title { get { return _consultant.FullName; } }

        public string TitleLabel 
        {
            get
            {
                var latestContract = _consultant.Contracts
                .Where(c => !string.IsNullOrEmpty(c.Title))
                .OrderByDescending(c => c.EndDate).FirstOrDefault();

                return latestContract != null ? latestContract.Title : string.Empty;
            }
        }

        public string ActionText
        {
            get { return IsActive ? "Renew" : "Onboard"; }
        }

        public string ContractsLabel 
        {
            get
            {
                return string.Format("{0} ({1})", _consultant.Contracts.Count(),
                    _consultant.Contracts.OrderByDescending(c => c.EndDate)
                    .Select(c => c.RateWitheld ? "Rate Withheld" : string.Format("{0:c}/hr", c.Rate)).FirstOrDefault()).TrimEnd();
            }
        }

        public string ResumeLabel
        {
            get { return this.HasResume ? "Resume" : "No Resume"; }
        }

        public bool HasResume
        { 
            get { return _consultant != null && !string.IsNullOrEmpty(_consultant.ResumeText); }
        }

        public string RatingAsString
        {
            get { return _consultant.Rating.ToString(); }
        }

        public int RatingAsInt
        {
            get
            {
                switch ((int)_consultant.Rating)
                {
                    case MatchGuideConstants.ResumeRating.BelowStandard:
                        return 1;
                    case MatchGuideConstants.ResumeRating.Standard:
                        return 2;
                    case MatchGuideConstants.ResumeRating.AboveStandard:
                        return 3;
                    default:
                        return 0;
                }
            }
        }

        public bool IsActive
        {
            get
            {
                return _consultant.Contracts.Any(c => c.StatusType == MatchGuideConstants.ContractStatusTypes.Active
                || c.StatusType == MatchGuideConstants.ContractStatusTypes.Pending);
            }
        }

        public IEnumerable<Specialization> Specializations
        {
            get { return _consultant.Specializations; }
        }

        public ConsultantDetailViewModel(IMatchGuideApi api)
        {
            this._api = api;
            this._consultant = new Consultant();
        }

        public async Task Initialize(ConsultantSummary summary)
        {
            this._consultant = new Consultant
            {
                Id = summary.Id,
                FirstName = summary.FirstName,
                LastName = summary.LastName,
                Rating = summary.Rating
            };

            await Load();
        }

        public async Task Load()
        {
            this._consultant = await _api.GetConsultant(this._consultant.Id);
        }

        /// <summary>
        /// Legacy
        /// </summary>
        public Consultant Consultant 
        {
            get { return _consultant; }
        }
    }
}
