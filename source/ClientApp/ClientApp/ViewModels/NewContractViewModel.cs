using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class NewContractViewModel : ViewModelBase
    {
        private Consultant _consultant;
        public Consultant Consultant
        {
            get { return _consultant;}
            set
            {
                _consultant = value;
                var lastContract = _consultant.Contracts.OrderByDescending(c => c.EndDate).First();
                if (lastContract != null)
                {
                    ContractorRate = lastContract.Rate;
                }
            }
        }

        public const decimal ServiceRate = 3;
        public decimal ContractorRate { get; set; }
        public decimal TotalRate { get { return ContractorRate + ServiceRate; } }
    }
}
