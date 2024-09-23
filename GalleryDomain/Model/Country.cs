using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GalleryDomain.Model;

public partial class Country : Entity
{
    [Required(ErrorMessage = "Введіть назву країни")]
    [Display(Name = "Країна")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
