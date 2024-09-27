using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Photo : Entity
{
    [Display(Name = "Автор")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Придумайте назву для фото")]
    [Display(Name = "Назва фото")]
    public string Title { get; set; } = null!;
   
    [Display(Name = "Дата фото")]
    public DateOnly? Date { get; set; }

    [Display(Name = "Опис фото")]
    public string? Description { get; set; }

    [Display(Name = "Назва локації")]
    public int? LocationId { get; set; }

    //[Required(ErrorMessage = "Оберіть картинку")]
    [Display(Name = "Фото")]
    public byte[]? Image { get; set; } = null!;

    [Display(Name = "Автор")]
    public virtual Author? Author { get; set; }

    [Display(Name = "Локація")]
    public virtual Location? Location { get; set; }
}
