using GH.EventManagement.Entities.Models;
using GH.EventManagement.DataAccess.Contexts;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GH.EventManagement.DataAccess.Seed
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Events.Any() || context.Participants.Any())
                return; // Zaten veri varsa tekrar ekleme

            // Katılımcılar (20 adet)
            var participantFaker = new Faker<Participant>("tr")
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Email, f => f.Internet.Email())
              ;

            var participants = participantFaker.Generate(20);
            context.Participants.AddRange(participants);
            context.SaveChanges();

            var now = DateTime.Now;

            // Tek kelimelik etkinlik türleri
            var eventTypes = new List<string>
            {
                "Konferans", "Seminer", "Sunum", "Eğitim", "Çalıştay", "Panel", "Forum", "Zirve", "Sempozyum", "Kongre"
            };

            // Özel başlık listesi
            var eventTitles = new List<string>
            {
                "Siber Güvenlik Sigortası Trendleri",
                "Sağlık Sigortalarında Dijital Dönüşüm",
                "Reasürans Piyasasına Bakış",
                "Hasar Yönetimi ve Teknoloji",
                "Araç Sigortalarında Yeni Yaklaşımlar",
                "Yangın Sigortalarında Risk Analizi",
                "Hayat Sigortalarında Gelecek",
                "Emeklilik Sistemleri ve Reformlar",
                "Sigorta Acentelerinin Rolü",
                "Aktüeryal Değerlendirme ve Modeller"
            };

            // Etkinlikler (10 adet, özel başlıklar, tür tek kelime)
            var eventFaker = new Faker<Event>("tr")
                .RuleFor(e => e.Title, f => eventTitles[f.IndexFaker]) // Özel başlık listesinden al
                .RuleFor(e => e.StartDate, f => now.AddDays(f.Random.Number(5, 60)).AddHours(f.Random.Number(8, 10)))
                .RuleFor(e => e.EndDate, (f, e) => e.StartDate.AddHours(f.Random.Number(6, 9)))
                .RuleFor(e => e.Location, f => f.Address.City())
                .RuleFor(e => e.Description, f => string.Join(" ", f.Lorem.Sentences(2)))
                .RuleFor(e => e.ImageUrl, f => $"/images/events/{f.IndexFaker + 1}.jpg") // Sıralı resim URL'si
                .RuleFor(e => e.Type, f => f.PickRandom(eventTypes))
                .RuleFor(e => e.EventParticipants, f =>
                {
                    var eventParticipants = new List<EventParticipant>();
                    var numberOfParticipants = f.Random.Number(1, participants.Count / 2);
                    var shuffledParticipants = participants.OrderBy(x => f.Random.Guid()).ToList();

                    for (int i = 0; i < numberOfParticipants; i++)
                    {
                        eventParticipants.Add(new EventParticipant { Participant = shuffledParticipants[i] });
                    }
                    return eventParticipants;
                });

            var events = eventFaker.Generate(10);
            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}