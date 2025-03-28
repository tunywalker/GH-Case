using GH.EventManagement.Entities.Models;

namespace GH.EventManagement.Business.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task<Event> GetByIdWithParticipantsAsync(int id); 
        Task AddAsync(Event entity);
        Task UpdateAsync(Event entity);
        Task DeleteAsync(int id);
    }
}
