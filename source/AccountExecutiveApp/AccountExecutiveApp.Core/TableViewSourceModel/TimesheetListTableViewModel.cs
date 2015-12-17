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
		public Dictionary<int, IEnumerable<TimesheetDetails>> TimesheetByYear
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
	}
}
