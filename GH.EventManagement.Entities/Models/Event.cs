using System.ComponentModel.DataAnnotations;

namespace GH.EventManagement.Entities.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Etkinlik başlığı gereklidir.")]
        [StringLength(70, ErrorMessage = "Başlık en fazla 70 karakter olabilir.")]
        public string Title { get; set; }        
       
        [Required(ErrorMessage = "Etkinlik Başlangıç tarihi gereklidir.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Etkinlik Bitiş tarihi gereklidir.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Yer alanı gereklidir.")]
        [StringLength(100, ErrorMessage = "Yer en fazla 100 karakter olabilir.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Açıklama alanı gereklidir.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Etkinlik Görseli alanı gereklidir.")]
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Etkinlik Tipi alanı gereklidir.")]
        public string? Type { get; set; }     
        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
