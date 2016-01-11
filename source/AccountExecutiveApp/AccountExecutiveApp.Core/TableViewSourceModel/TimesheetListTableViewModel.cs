using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
	public class TimesheetListTableViewModel
	{
		private Dictionary<int, IEnumerable<TimesheetDetails>> _timesheetByYear;
	    public MatchGuideConstants.TimesheetStatus Status;

	    private Dictionary<int, IEnumerable<TimesheetDetails>> TimesheetByYear
		{
			get { return _timesheetByYear ?? new Dictionary<int, IEnumerable<TimesheetDetails>>(); }
			set { _timesheetByYear = value ?? new Dictionary<int, IEnumerable<TimesheetDetails>>(); }
		}

	    public int TimesheetIdBySectionAndRow(int section, int rowNumber)
	    {
            return RowIsInBounds(section, rowNumber)
            ? TimesheetByYear.Values.ElementAt(section).ElementAt(rowNumber).Id
                : 0;
	    }
        public int AgreementIdBySectionAndRow(int section, int rowNumber)
        {
            return RowIsInBounds(section, rowNumber)
            ? TimesheetByYear.Values.ElementAt(section).ElementAt(rowNumber).AgreementId
                : 0;
        }

	    public TimesheetListTableViewModel(IEnumerable<TimesheetDetails> timesheetDetails, MatchGuideConstants.TimesheetStatus status)
		{
            _timesheetByYear = new Dictionary<int, IEnumerable<TimesheetDetails>>();
	        Status = status;
            var yearList = timesheetDetails.GroupBy(d => d.StartDate.Year).Select(grp => grp.First().StartDate.Year);

	        foreach (var year in yearList)
	        {
	            _timesheetByYear.Add(year, timesheetDetails.Where(d=>d.StartDate.Year == year));
	        }
		}

		public bool RowIsInBounds(int section, int rowNumber)
		{
			return section < NumberOfGroups() && rowNumber >= 0 && rowNumber < TimesheetByYear.Values.ElementAt(section).Count();
		}

		public int NumberOfGroups()
		{
			return TimesheetByYear.Keys.Count;
		}

        public string YearBySection(int section)
        {
            return RowIsInBounds(section, 0)
                ? TimesheetByYear.Keys.ElementAt(section).ToString()
                    : string.Empty;
        }

		public string CompanyNameBySectionAndRow(int section, int rowNumber )
		{
			return RowIsInBounds(section, rowNumber)
                ? TimesheetByYear.Values.ElementAt(section).ElementAt(rowNumber).CompanyName
					: string.Empty;
		}

        public string ContractorFullNameBySectionAndRow(int section, int rowNumber)
        {
            return RowIsInBounds(section, rowNumber)
                ? TimesheetByYear.Values.ElementAt(section).ElementAt(rowNumber).ContractorFullName
                    : string.Empty;
        }

        public string FormattedPeriodBySectionAndRow(int section, int rowNumber)
        {
            return RowIsInBounds(section, rowNumber)
                ? string.Format("{0}-{1}", TimesheetByYear.Values.ElementAt(section).ElementAt(rowNumber).StartDate.ToString("MMM d"),
                    TimesheetByYear.Values.ElementAt(section).ElementAt(rowNumber).EndDate.ToString("dd").TrimStart('0')) 
                    : string.Empty;
        }

	    public int NumberOfTimesheetsInSection(int section)
	    {
	        return RowIsInBounds(section, 0)
                ? TimesheetByYear.Values.ElementAt(section).Count()
					: 0;
	    }
	}
}
