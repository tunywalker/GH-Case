using GH.EventManagement.Business.Interfaces;
using GH.EventManagement.Core.Interfaces;
using GH.EventManagement.DataAccess.Contexts;
using GH.EventManagement.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace GH.EventManagement.Business.Services
{
    public class EventManager : IEventService
    {

        private readonly IGenericRepository<Event> _eventRepository;
        private readonly AppDbContext _context;

        public EventManager(IGenericRepository<Event> eventRepository, AppDbContext context)
        {
            _eventRepository = eventRepository;
            _context= context;
        }
        public async Task<IEnumerable<Event>> GetAllAsync() => await _eventRepository.GetAllAsync();
        public async Task<Event> GetByIdAsync(int id) => await _eventRepository.GetByIdAsync(id);

        public async Task<Event> GetByIdWithParticipantsAsync(int id)
        {
            
            var eventWithParticipants = await _context.Events
                .Include(e => e.EventParticipants)  
                .ThenInclude(ep => ep.Participant) 
                .FirstOrDefaultAsync(e => e.Id == id);  

            return eventWithParticipants;
        }
        public async Task AddAsync(Event entity)
        {
            await _eventRepository.AddAsync(entity);
            await _eventRepository.SaveAsync();
        }

        public async Task UpdateAsync(Event entity)
        {
            _eventRepository.Update(entity);
            await _eventRepository.SaveAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _eventRepository.GetByIdAsync(id);
            if (entity != null)
            {
                _eventRepository.Remove(entity);
                await _eventRepository.SaveAsync();
            }
        }
       



    }
}
