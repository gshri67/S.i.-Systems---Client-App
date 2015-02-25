using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Services
{
    public class SpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;

        public SpecializationService(ISpecializationRepository specializationRepository)
        {
            this._specializationRepository = specializationRepository;
        }

        public async Task<IEnumerable<Specialization>> GetAllAsync()
        {
            return await _specializationRepository.GetAllAsync();
        }
    }
}
