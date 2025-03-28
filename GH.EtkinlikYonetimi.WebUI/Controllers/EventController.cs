using GH.EventManagement.Business.Interfaces;
using GH.EventManagement.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace GH.EventManagement.WebUI.Controllers
{
    /// <summary>
    /// Etkinliklerin listelenmesi, detaylarının görüntülenmesi, oluşturulması ve katılımcı ekleme/silme işlemlerini yönetir.
    /// </summary>
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IParticipantService _participantService;

        /// <summary>
        /// EventController sınıfının yeni bir örneğini başlatır.
        /// </summary>
        public EventController(IEventService eventService, IParticipantService participantService)
        {
            _eventService = eventService;
            _participantService = participantService;
        }

        /// <summary>
        /// GET: /Event - Etkinlikleri listeler, isteğe bağlı filtreleme ve sayfalama uygular.
        /// </summary>
        public async Task<IActionResult> Index(string search, DateTime? startDate, string location, string eventType, int page = 1)
        {
            var pageSize = 8;
            var events = await _eventService.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                events = events.Where(e => e.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                           e.Location.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(eventType))
            {
                events = events.Where(e => e.Type.Contains(eventType, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (startDate.HasValue)
            {
                events = events.Where(e => e.StartDate.Date == startDate.Value.Date).ToList();
            }

            if (!string.IsNullOrEmpty(location))
            {
                events = events.Where(e => e.Location.Contains(location, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var totalEvents = events.Count();
            var paginatedEvents = events.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.Search = search;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.Location = location;
            ViewBag.TotalPages = (int)Math.Ceiling(totalEvents / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TotalResults = totalEvents;

            return View(paginatedEvents);
        }

        /// <summary>
        /// GET: /Event/Details/{id} - Belirtilen etkinliğin detaylarını gösterir.
        /// </summary>
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _eventService.GetByIdWithParticipantsAsync(id);
            if (eventItem == null)
                return NotFound();

            var allParticipants = await _participantService.GetAllAsync();
            ViewBag.AllParticipants = allParticipants;

            return View(eventItem);
        }

        /// <summary>
        /// GET: /Event/LoadParticipants/{eventId} - Belirtilen etkinliğe ait katılımcıları getirir (Partial View).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LoadParticipants(int eventId)
        {
            var eventItem = await _eventService.GetByIdWithParticipantsAsync(eventId);
            var eventParticipants = eventItem?.EventParticipants?.ToList() ?? new List<EventParticipant>();

            return PartialView("_ParticipantsPartial", eventParticipants);
        }

        /// <summary>
        /// POST: /Event/RemoveParticipant - Etkinlikten bir katılımcıyı kaldırır.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RemoveParticipant([FromBody] RemoveParticipantFromEventViewModel model)
        {
            var evt = await _eventService.GetByIdWithParticipantsAsync(model.EventId);
            if (evt == null)
                return NotFound();

            var existing = evt.EventParticipants.FirstOrDefault(p => p.ParticipantId == model.ParticipantId);
            if (existing != null)
            {
                evt.EventParticipants.Remove(existing);
                await _eventService.UpdateAsync(evt);
                return Ok();
            }

            return BadRequest("Katılımcı bulunamadı.");
        }

        /// <summary>
        /// POST: /Event/UpdateSingleField - Etkinliğe ait tek bir alanı günceller (satır içi düzenleme).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateSingleField([FromBody] InlineEditModel model)
        {
            var evt = await _eventService.GetByIdAsync(model.EventId);
            if (evt == null) return NotFound();

            switch (model.Field)
            {
                case "Title":
                    evt.Title = model.Value;
                    break;
                case "Description":
                    evt.Description = model.Value;
                    break;
                case "Location":
                    evt.Location = model.Value;
                    break;
                case "Type":
                    evt.Type = model.Value;
                    break;
                case "ImageUrl":
                    evt.ImageUrl = model.Value;
                    break;
                case "StartDate":
                    if (DateTime.TryParse(model.Value, out var newStart))
                    {
                        if (evt.EndDate != null && newStart > evt.EndDate)
                            return BadRequest("Başlangıç tarihi, bitiş tarihinden sonra olamaz.");
                        evt.StartDate = newStart;
                    }
                    break;
                case "EndDate":
                    if (DateTime.TryParse(model.Value, out var newEnd))
                    {
                        if (evt.StartDate != null && newEnd < evt.StartDate)
                            return BadRequest("Bitiş tarihi, başlangıç tarihinden önce olamaz.");
                        evt.EndDate = newEnd;
                    }
                    break;
                default:
                    return BadRequest();
            }

            await _eventService.UpdateAsync(evt);
            return Ok();
        }

        /// <summary>
        /// POST: /Event/AddParticipants - Etkinliğe bir veya birden fazla katılımcı ekler.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddParticipants([FromBody] AddParticipantsToEventViewModel model)
        {
            var evt = await _eventService.GetByIdWithParticipantsAsync(model.EventId);
            if (evt == null) return NotFound();

            foreach (var pid in model.ParticipantIds)
            {
                if (!evt.EventParticipants.Any(ep => ep.ParticipantId == pid))
                {
                    evt.EventParticipants.Add(new EventParticipant { ParticipantId = pid });
                }
            }

            await _eventService.UpdateAsync(evt);
            return Ok();
        }

        /// <summary>
        /// GET: /Event/LoadAddParticipantForm/{eventId} - Katılımcı ekleme formunu (Partial View) yükler.
        /// </summary>
        public IActionResult LoadAddParticipantForm(int eventId)
        {
            var eventItem = _eventService.GetByIdWithParticipantsAsync(eventId).Result;
            ViewBag.AllParticipants = _participantService.GetAllAsync().Result;
            return PartialView("~/Views/Event/Partials/_AddParticipantPartial.cshtml", eventItem);
        }

        /// <summary>
        /// GET: /Event/Create - Yeni etkinlik oluşturma formunu görüntüler.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var participants = await _participantService.GetAllAsync();
            var model = new EventCreateViewModel
            {
                AllParticipants = participants.ToList()
            };

            return View(model);
        }

        /// <summary>
        /// POST: /Event/Create - Yeni etkinlik oluşturma formunun gönderimini işler.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    Title = model.Title,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Location = model.Location,
                    Description = model.Description,
                    ImageUrl = string.IsNullOrEmpty(model.ImageUrl) ? "/images/events/10.jpg" : model.ImageUrl,
                    Type = model.Type,
                    EventParticipants = model.SelectedParticipantIds?.Select(id => new EventParticipant
                    {
                        ParticipantId = id
                    }).ToList() ?? new List<EventParticipant>()
                };

                await _eventService.AddAsync(newEvent);

                TempData["SuccessMessage"] = "Etkinlik başarıyla eklendi!";
                return RedirectToAction("Index");
            }

            model.AllParticipants = (await _participantService.GetAllAsync()).ToList();
            return View(model);
        }
    }
}
