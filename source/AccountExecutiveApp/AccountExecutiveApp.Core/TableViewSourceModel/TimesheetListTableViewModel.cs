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
		private Dictionary<int, IEnumerable<TimesheetDetails>> TimesheetByYear
		{
			get { return _timesheetByYear ?? new Dictionary<int, IEnumerable<TimesheetDetails>>(); }
			set { _timesheetByYear = value ?? new Dictionary<int, IEnumerable<TimesheetDetails>>(); }
		}

		public TimesheetListTableViewModel(IEnumerable<TimesheetDetails> timesheetDetails)
		{
			_timesheetByYear = timesheetDetails.ToDictionary(x => x.StartDate.Year, x => timesheetDetails.Where(d => d.StartDate.Year == x.StartDate.Year ));
		}

		public bool RowIsInBounds(int section, int rowNumber)
		{
			return section < NumberOfGroups() && rowNumber >= 0 && rowNumber < TimesheetByYear[section].Count();
		}

		public int NumberOfGroups()
		{
			return TimesheetByYear.Keys.Count;
		}

		public string CompanyNameBySectionAndRow(int section, int rowNumber )
		{
			return RowIsInBounds(section, rowNumber) 
				? TimesheetByYear[section].ElementAt(rowNumber).CompanyName
					: string.Empty;
		}

        public string ContractorFullNameBySectionAndRow(int section, int rowNumber)
        {
            return RowIsInBounds(section, rowNumber)
                ? TimesheetByYear[section].ElementAt(rowNumber).ContractorFullName
                    : string.Empty;
        }

        public string FormattedPeriodBySectionAndRow(int section, int rowNumber)
        {
            return RowIsInBounds(section, rowNumber)
                ? string.Format("{0}-{1}", TimesheetByYear[section].ElementAt(rowNumber).StartDate.ToString("MMM d"),
                    TimesheetByYear[section].ElementAt(rowNumber).EndDate.ToString("d")) 
                    : string.Empty;
        }

	    public int NumberOfTimesheetsInSection(int section)
	    {
	        return RowIsInBounds(section, 0)
                ? TimesheetByYear[section].Count()
					: 0;
	    }
	}
}
