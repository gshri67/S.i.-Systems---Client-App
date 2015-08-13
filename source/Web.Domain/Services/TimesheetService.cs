﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SiSystems.Web.Domain.Context;
using SiSystems.ConsultantApp.Web.Domain.Repositories;
using SiSystems.SharedModels;

namespace SiSystems.ConsultantApp.Web.Domain.Services
{
    /// <summary>
    /// You might be wondering why there are two layers here.
    /// 
    /// Service intended to perform any logic, transformations, etc..
    /// so that this can be tested. Repo/data access can be mocked out.
    /// </summary>
    public class TimesheetService
    {
        private readonly ITimesheetRepository _timeSheetRepository;
        private readonly ISessionContext _sessionContext;

        public TimesheetService(ITimesheetRepository timesheetRepository, ISessionContext sessionContext)
        {
            _timeSheetRepository = timesheetRepository;
            _sessionContext = sessionContext;
        }

        public IEnumerable<Timesheet> GetOpenTimesheets(DateTime date)
        {
            return _timeSheetRepository.GetTimesheetsForDate(date);
        }

        public IEnumerable<Timesheet> GetActiveTimesheets(DateTime date)
        {
            return _timeSheetRepository.GetActiveTimesheetsForDate(date);
        }

        public IEnumerable<TimeEntry> GetTimeEntries(DateTime date)
        {
            return _timeSheetRepository.GetTimeEntries(date);
        }
    }
}