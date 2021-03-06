using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApp.Web.Models
{
    public class MyIdentityUser
        : IdentityUser<Guid>
    {
        [Display(Name="Display Name")]
        [Required(ErrorMessage = "{0} cannot be empty.")]
        [MinLength(2,ErrorMessage="{0} should have at least {1} characters.")]
        [StringLength(60, ErrorMessage="{0} cannot have more than {1} characters.")]
        public string DisplayName { get; set; }

        [Display(Name="Date of Birth")]
        [Required]
        [PersonalData]
        [Column(TypeName = "smalldatetime")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name="Is Admin USer?")]
        [Required]
        public bool IsAdminUser { get; set; }
    }
}
