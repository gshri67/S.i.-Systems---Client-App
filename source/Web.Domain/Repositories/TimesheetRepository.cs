using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Repositories
{
    public interface ITimesheetRepository
    {
        IEnumerable<Timesheet> GetTimesheetsForDate(DateTime date);
        IEnumerable<TimeEntry> GetTimeEntries(DateTime date);
        IEnumerable<Timesheet> GetActiveTimesheetsForDate(DateTime date);
        IEnumerable<Timesheet> GetTimesheetsForUser(int id);
    }

    public class TimesheetRepository : ITimesheetRepository
    {
        public IEnumerable<Timesheet> GetTimesheetsForDate(DateTime date)
        {
            return UserTimesheets;
        }

        public IEnumerable<TimeEntry> GetTimeEntries(DateTime date)
        {
            return CenovusTimeEntries.Union(NexenTimeEntries);
        }

        public IEnumerable<Timesheet> GetActiveTimesheetsForDate(DateTime date)
        {
            return UserTimesheets;
        }

        public IEnumerable<Timesheet> GetTimesheetsForUser(int id)
        {
            return UserTimesheets;
        }

        //mocked out data for now
        //todo: connect this to the DB using the connection and retrieve the proper data. 
        private static List<Timesheet> UserTimesheets
        {
            get
            {
                return new List<Timesheet>
                {
                    new Timesheet
                    {
                        ClientName = "Cenovus",
                        TimesheetApprover= "bob.smith@email.com",
                        Status = TimesheetStatus.Open,
                        StartDate = new DateTime(2015, 08, 01),
                        EndDate = new DateTime(2015, 08, 31),
                        TimeEntries = CenovusTimeEntries
                    },
                    new Timesheet
                    {
                        ClientName = "Nexen",
                        TimesheetApprover= "sally.abbott@email.com",
                        Status = TimesheetStatus.Open,
                        StartDate = new DateTime(2015, 08, 01),
                        EndDate = new DateTime(2015, 08, 15),
                        TimeEntries = NexenTimeEntries
                    }
                };
            }
        }

        private static List<TimeEntry> NexenTimeEntries
        {
            get
            {
                return new List<TimeEntry>
                {
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 1),
                        Hours = 2,
                        ProjectCode = "PC1111"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 2),
                        Hours = 2,
                        ProjectCode = "PC1111"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 3),
                        Hours = 2,
                        ProjectCode = "PC1111"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 4),
                        Hours = 2,
                        ProjectCode = "PC1111"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 1),
                        Hours = 2,
                        ProjectCode = "PC1112"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 2),
                        Hours = 2,
                        ProjectCode = "PC1112"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 3),
                        Hours = 2,
                        ProjectCode = "PC1112"
                    },
                    new TimeEntry
                    {
                        ClientName = "Nexen",
                        Date = new DateTime(2015, 8, 4),
                        Hours = 2,
                        ProjectCode = "PC1112"
                    }
                };
            }
        }

        private static List<TimeEntry> CenovusTimeEntries
        {
            get
            {
                return new List<TimeEntry>
                {
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 3),
                        Hours = 4,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 4),
                        Hours = 4,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 5),
                        Hours = 4,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 6),
                        Hours = 4,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 7),
                        Hours = 4,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 10),
                        Hours = 8,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 11),
                        Hours = 8,
                        ProjectCode = "PC0001"
                    },
                    new TimeEntry
                    {
                        ClientName = "Cenovus",
                        Date = new DateTime(2015, 8, 12),
                        Hours = 8,
                        ProjectCode = "PC0001"
                    }
                };
            }
        }
    }
}
