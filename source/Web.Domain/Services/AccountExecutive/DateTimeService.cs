using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public interface IDateTimeService
    {
        bool DateIsWithinNextThirtyDays(DateTime date);
    }

    public class DateTimeService : IDateTimeService
    {
        public bool DateIsWithinNextThirtyDays(DateTime date)
        {
            return DateTime.Compare(date, DateTime.UtcNow) > 0 
                && DateTime.Compare(date, DateTime.UtcNow.AddDays(30)) < 0;
        }
    }
}
