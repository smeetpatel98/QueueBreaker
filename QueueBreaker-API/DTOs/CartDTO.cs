using QueueBreaker_API.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QueueBreaker_API.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int CanteenId { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual CanteenDTO Canteen { get; set; }
        public virtual ItemsDTO Item { get; set; }
        public virtual UsersDTO User { get; set; }
    }

    public class CartCreateDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int CanteenId { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int Count { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }

    }

    public class CartUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int CanteenId { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int Count { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
