using System;
using System.Collections.Generic;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.DTOs
{
    public class OrderDTO
    {
        public OrderDTO()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int CanteenId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Review { get; set; }
        public virtual Canteen Canteen { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderCreateDTO
    {
        public int UserId { get; set; }
        public int CanteenId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Review { get; set; }
    }

    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CanteenId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string Review { get; set; }
    }
}
