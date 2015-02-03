using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class EulaRepository
    {
        private const string OldEulaText = "Old EULA";

        private const string CurrentEulaText = @"Current EULA
-point one
-point two

Some stuff..
Some other stuff.
";

        private static readonly List<Eula>  Eulas = new List<Eula>
        {
            new Eula
            {
                Version = 1,
                Text = OldEulaText, PublishedDate = DateTime.Now.AddDays(-3)
            },
            new Eula
            {
                Version = 2,
                Text = CurrentEulaText, PublishedDate = DateTime.Now.AddDays(-1)
            }
        };

        public Eula GetMostRecentEula()
        {
            return Eulas.OrderByDescending(e => e.Version).FirstOrDefault();
        }
    }
}
