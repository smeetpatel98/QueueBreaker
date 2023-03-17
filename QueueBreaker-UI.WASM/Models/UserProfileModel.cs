using System.ComponentModel.DataAnnotations;

namespace QueueBreaker_UI.WASM.Models
{
    public class UserProfileModel
    {
        public int ProfileId { get; set; }
        public string UserName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        [Phone]
        [StringLength(11, ErrorMessage = "Please Enter Valid Phone Number")]
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }

    public class UpdatePasswordModel
    {
        public int ProfileId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [Compare("Password", ErrorMessage = "The password and New password do not match.")]
        public string NewPassword { get; set; }

    }
}
