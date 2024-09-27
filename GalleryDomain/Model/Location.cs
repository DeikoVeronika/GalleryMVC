using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Location : Entity
{
    [Required(ErrorMessage = "Введіть назву локації")]
    [Display(Name = "Локація")]
    public string Name { get; set; } = null!;

    [Display(Name = "Посилання на локацію")]
    public string? Link { get; set; }
    public double Latitude { get; set; } // Широта
    public double Longitude { get; set; } // Довгота

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
