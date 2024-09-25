using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Author : Entity
{
    [Required(ErrorMessage = "Введіть ім'я фотографа")]
    [Display(Name = "Ім'я фотографа")]
    public string Name { get; set; } = null!;

    [Display(Name = "Країна фотографа")]
    public int? CountryId { get; set; }

    [Display(Name = "Біографія фотографа")]
    public string? Biography { get; set; }

    [Display(Name = "Країна")]
    public virtual Country? Country { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
