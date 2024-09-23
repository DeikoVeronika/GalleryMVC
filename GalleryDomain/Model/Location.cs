﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Location : Entity
{
    [Required(ErrorMessage = "Введіть локацію")]
    [Display(Name = "Локація")]
    public string Name { get; set; } = null!;

    [Display(Name = "Посилання на локацію")]
    public string? Link { get; set; }

    public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
}
