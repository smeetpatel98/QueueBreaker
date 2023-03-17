using System;
namespace QueueBreaker_API.DTOs
{
    public class OrderItemsDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual ItemsDTO Item { get; set; }
        public virtual OrderDTO Order { get; set; }
    }

    public class OrderItemsCreateDTO
    {
        public int? OrderId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }

    public class OrderItemsUpdateDTO
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public decimal? ItemPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
