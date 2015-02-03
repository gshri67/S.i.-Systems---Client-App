using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services.Interfaces;
using SiSystems.ClientApp.SharedModels;

namespace ClientApp.Services
{
    public class EulaService : IEulaService
    {
        public Task<Eula> GetMostRecentEula()
        {
            //TODO connect to webservice and make call
            return Task.FromResult(new Eula {PublishedDate = DateTime.Now, Text = "This is a test Eula", Version = 1});
        }
    }
}
