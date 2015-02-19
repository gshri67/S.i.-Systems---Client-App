using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.ViewModels
{
    public class ConsultantDetailViewModel : ViewModelBase
    {
        private readonly IAlumniService _alumniService;

        private Consultant _consultant;

        public bool IsLoading
        {
            get { return _consultant == null; }
        }

        public ConsultantDetailViewModel(IAlumniService alumniService)
        {
            _alumniService = alumniService;
        }

        public async Task<Consultant> GetConsultant(int id)
        {
            if (_consultant != null && _consultant.Id == id)
            {
                return _consultant;
            }

            _consultant = await _alumniService.GetConsultant(id);
            return _consultant;
        }

        public Consultant GetConsultant()
        {
            return _consultant ?? new Consultant();
        }

        public static string GetRatingString(int? rating)
        {
            switch (rating)
            {
                case MatchGuideConstants.ResumeRating.Standard:
                    return "Standard";
                case MatchGuideConstants.ResumeRating.AboveStandard:
                    return "Above Standard";
                case MatchGuideConstants.ResumeRating.BelowStandard:
                    return "Below Standard";
                default:
                    return "Not Checked";
            }
        }

        public static string GetYearsExperienceString(int expCode)
        {
            switch (expCode)
            {
                case MatchGuideConstants.YearsOfExperience.LessThanTwo:
                    return " (< 2 years)";
                case MatchGuideConstants.YearsOfExperience.TwoToFour:
                    return " (2 - 4 years)";
                case MatchGuideConstants.YearsOfExperience.FiveToSeven:
                    return " (5 - 7 years)";
                case MatchGuideConstants.YearsOfExperience.EightToTen:
                    return " (8 - 10 years)";
                case MatchGuideConstants.YearsOfExperience.MoreThanTen:
                    return " (> 10 years)";
                default:
                    return "";
            }
        }
    }
}
