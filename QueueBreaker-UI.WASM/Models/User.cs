using System;
using System.Collections.Generic;

#nullable disable

namespace QueueBreaker_UI.WASM.Models
{
    public partial class User
    {
        //public User()
        //{
        //    Orders = new HashSet<Order>();
        //}

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EnrollmentNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public int? CanteenId { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public int RoleId { get; set; }
        public string Token { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        // public virtual Canteen Canteen { get; set; }
        //   public virtual Role Role { get; set; }
        // public virtual ICollection<Order> Orders { get; set; }
    }
}
