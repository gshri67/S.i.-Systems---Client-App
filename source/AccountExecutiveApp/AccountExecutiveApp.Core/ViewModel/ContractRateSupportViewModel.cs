using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core;
using SiSystems.SharedModels;
using SiSystems.SharedModels.Contract_Creation;

namespace AccountExecutiveApp.Core.ViewModel
{
    public class ContractRateSupportViewModel
    {
        private readonly IMatchGuideApi _api;

        private RateOptions _rateOptions;
        private RateOptions Options
        {
            get { return _rateOptions ?? new RateOptions(); }
            set { _rateOptions = value ?? new RateOptions(); }
        }

        public ContractRateSupportViewModel(IMatchGuideApi api)
        {
            _api = api;
        }

        public List<string> RateTypeOptions { get { return new List<string>(new string[] { "Per hour", "Per day" }); } }
        public List<string> RateDescriptionOptions { get { return new List<string>(new string[] { "Regular", "Hourly", "Daily" }); } }

        //public List<string> RateTypes { get { return Options.RateTypes.ToList(); } }
        //public List<string> RateDescriptionOptions { get { return Options.RateDescriptionOptions.ToList(); } }


        public Task GetRateOptions()
        {
            var task = GetOptions();
            //todo: task.continueWith
            return task;
        }

        private async Task GetOptions()
        {
            Options = await _api.GetDropDownValuesForContractCreationRatesForm();
        }
    }
}