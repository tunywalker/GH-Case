using GH.EventManagement.Entities.Models;

namespace GH.EventManagement.Business.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<Participant>> GetAllAsync();
        Task<Participant> GetByIdAsync(int id);
        Task AddAsync(Participant entity);
        Task UpdateAsync(Participant entity);
        Task DeleteAsync(int id);
    }
}
