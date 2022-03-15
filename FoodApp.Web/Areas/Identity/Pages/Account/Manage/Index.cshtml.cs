using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

using FoodApp.Web.Models;
using FoodApp.Web.Models.Enums;
using FoodApp.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodApp.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(
            UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public string PhoneNumber { get; private set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Display Name")]
            [Required(ErrorMessage = "{0} cannot be empty.")]
            [MinLength(2, ErrorMessage = "{0} should have at least {1} characters.")]
            [StringLength(60, ErrorMessage = "{0} cannot have more than {1} characters.")]
            public string DisplayName { get; set; }

            [Display(Name = "Date of Birth")]
            [Required]
            [PersonalData]
            [Column(TypeName = "smalldatetime")]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Gender")]
            [Required(ErrorMessage = "Please indicate which of these best describes your gender.")]
            public MyIdentityGenders Gender { get; set; }


            [Display(Name = "Is Admin User?")]
            [Required]
            public bool IsAdminUser { get; set; }
        }

        private void LoadUserData(MyIdentityUser user)
        {
           
            Username = user.UserName;

            Input = new InputModel
            {
                PhoneNumber = user.PhoneNumber,
                DisplayName=user.DisplayName,
                DateOfBirth=user.DateOfBirth,
                IsAdminUser=user.IsAdminUser,
                Gender=user.Gender
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LoadUserData(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
               LoadUserData(user);
                return Page();
            }

            bool hasChangedPhoneNumber = false;
            
 
            if (Input.PhoneNumber != PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    hasChangedPhoneNumber = true;
                }
                else
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            bool hasOtherChanges = false;

            if(Input.DisplayName != user.DisplayName)
            {
                user.DisplayName = Input.DisplayName;
                hasOtherChanges = true;
            }
            if (Input.DateOfBirth != user.DateOfBirth)
            {
                user.DateOfBirth = Input.DateOfBirth;
                hasOtherChanges = true;
            }
            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
                hasOtherChanges = true;
            }
            if (hasChangedPhoneNumber || hasOtherChanges)
            {
                _dbContext.SaveChanges();
                this.StatusMessage = "Your profile has been updated successfully";
                await _signInManager.RefreshSignInAsync(user);

            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
