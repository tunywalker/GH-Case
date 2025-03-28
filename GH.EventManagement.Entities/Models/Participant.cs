using System.ComponentModel.DataAnnotations;

namespace GH.EventManagement.Entities.Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [StringLength(15, ErrorMessage = "Ad en fazla 15 karakter olabilir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        [StringLength(15, ErrorMessage = "Soyad en fazla 15 karakter olabilir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }        

        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
