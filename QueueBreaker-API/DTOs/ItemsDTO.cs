using System;
using System.Collections.Generic;

namespace QueueBreaker_API.DTOs
{
    public class ItemsDTO
    {
        public ItemsDTO()
        {
            OrderItems = new HashSet<OrderItemsDTO>();
        }

        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string ImageFile { get; set; }
        public string Comment { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual CategoryDTO Category { get; set; }
        public virtual ICollection<OrderItemsDTO> OrderItems { get; set; }
    }

    public class ItemsCreateDTO
    {
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string Comment { get; set; }
        public string ImageFile { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }

    public class ItemsUpdateDTO
    { 
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string ImageFile { get; set; }
        public string Comment { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
