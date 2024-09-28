using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Location : Entity
{
    [Required(ErrorMessage = "Введіть назву локації")]
    [Display(Name = "Локація")]
    public string Name { get; set; } = null!;

    [Display(Name = "Посилання на інформацію (опціонально)")]
    [Url(ErrorMessage = "Будь ласка, введіть дійсний URL.")]

    public string? Link { get; set; }
    [Required(ErrorMessage = "Введіть широту")]

    [Display(Name = "Широта")]
    public double Latitude { get; set; } // Широта
    [Required(ErrorMessage = "Введіть довготу")]
    [Display(Name = "Довгота")]
    public double Longitude { get; set; } // Довгота

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
