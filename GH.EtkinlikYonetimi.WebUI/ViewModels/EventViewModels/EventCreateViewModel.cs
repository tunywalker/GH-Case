using GH.EventManagement.Entities.Models;
using System;
using System.ComponentModel.DataAnnotations;
using WebUI.Attributes;

public class EventCreateViewModel
{
    public EventCreateViewModel()
    {
        var now = DateTime.Now;
        StartDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
        EndDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
    }

    [Display(Name = "Etkinlik Başlığı")]
    [Required(ErrorMessage = "Etkinlik başlığı gereklidir.")]
    [StringLength(70, ErrorMessage = "Başlık en fazla 70 karakter olabilir.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Etkinlik başlangıç tarihi gereklidir.")]
    [Display(Name = "Başlangıç Tarihi")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Etkinlik bitiş tarihi gereklidir.")]
    [DateRange("StartDate")]
    [Display(Name = "Bitiş Tarihi")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Yer gereklidir.")]
    [StringLength(100, ErrorMessage = "Yer en fazla 100 karakter olabilir.")]
    [Display(Name = "Yer")]
    public string Location { get; set; }

    [Required(ErrorMessage = "Açıklama gereklidir.")]
    [Display(Name = "Açıklama")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Görsel URL gereklidir.")]
    [Display(Name = "Görsel URL")]
    public string ImageUrl { get; set; }

    [Required(ErrorMessage = "Etkinlik tipi gereklidir.")]
    [Display(Name = "Etkinlik Tipi")]
    public string Type { get; set; }
    public List<Participant> AllParticipants { get; set; } = new List<Participant>();
    public List<int> SelectedParticipantIds { get; set; } = new List<int>();
}
