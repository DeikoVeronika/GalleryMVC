using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Photo : Entity
{
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

    [Required(ErrorMessage = "Оберіть картинку")]
    public byte[] Image { get; set; } = null!;

    public virtual Author? Author { get; set; }

    public virtual Location? Location { get; set; }
}
