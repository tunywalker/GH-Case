using GH.EventManagement.Business.Interfaces;
using GH.EventManagement.Core.Interfaces;
using GH.EventManagement.Entities.Models;

namespace GH.EventManagement.Business.Services
{
    public class ParticipantManager : IParticipantService
    {
        private readonly IGenericRepository<Participant> _participantRepository;

        public ParticipantManager(IGenericRepository<Participant> participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<IEnumerable<Participant>> GetAllAsync()
        {
            return await _participantRepository.GetAllAsync();
        }

        public async Task<Participant> GetByIdAsync(int id)
        {
            return await _participantRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Participant entity)
        {
            await _participantRepository.AddAsync(entity);
            await _participantRepository.SaveAsync();
        }

        public async Task UpdateAsync(Participant entity)
        {
            _participantRepository.Update(entity);
            await _participantRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _participantRepository.GetByIdAsync(id);
            if (entity != null)
            {
                _participantRepository.Remove(entity);
                await _participantRepository.SaveAsync();
            }
        }
    }
}
