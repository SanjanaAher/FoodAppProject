using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApp.Web.Models
{
    public class MyIdentityRole
        :IdentityRole<Guid>

    {
        [Display(Name="Description")]
        [StringLength(100, ErrorMessage="{0} cannot have more than {1} characters.")]
        public string Description { get; set; }
    }
}
