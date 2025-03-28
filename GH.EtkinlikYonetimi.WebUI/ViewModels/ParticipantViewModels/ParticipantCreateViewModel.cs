using System.ComponentModel.DataAnnotations;

public class ParticipantCreateViewModel
{
    [Required(ErrorMessage = "Ad boş bırakılamaz.")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Soyad boş bırakılamaz.")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
    public string Email { get; set; }

    [Display(Name = "Katılım Durumu")]
    public bool IsAttending { get; set; } = true;
}

