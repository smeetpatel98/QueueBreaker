﻿using System;
using System.Collections.Generic;

#nullable disable

namespace QueueBreaker_API.Data
{
    public partial class Item
    {
        public Item()
        {
            Carts = new HashSet<Cart>();
            OrderItems = new HashSet<OrderItem>();
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

        public virtual Category Category { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
