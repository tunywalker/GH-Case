using GH.EventManagement.Business.Interfaces;
using GH.EventManagement.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GH.EventManagement.WebUI.Controllers
{
    /// <summary>
    /// Katılımcıların listelenmesi ve oluşturulması işlemlerini yönetir.
    /// </summary>
    public class ParticipantController : Controller
    {
        private readonly IParticipantService _participantService;

        /// <summary>
        /// ParticipantController sınıfının yeni bir örneğini başlatır.
        /// </summary>
        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// GET: /Participant - Tüm katılımcıları listeler.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var participants = await _participantService.GetAllAsync();
            return View(participants);
        }

        /// <summary>
        /// GET: /Participant/Create - Yeni katılımcı oluşturma formunu gösterir.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: /Participant/Create - Katılımcı oluşturma formunun gönderimini işler.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParticipantCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var participant = new Participant
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                   
                };

                await _participantService.AddAsync(participant);

                TempData["SuccessMessage"] = "Katılımcı başarıyla eklendi.";

                ModelState.Clear();
                return View(new ParticipantCreateViewModel());
            }

            return View(model);
        }
    }
}
