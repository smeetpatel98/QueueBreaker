using System;
using System.Collections.Generic;

#nullable disable

namespace QueueBreaker_API.Data
{
    public partial class Cart
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

        public virtual Canteen Canteen { get; set; }
        public virtual Item Item { get; set; }
        public virtual User User { get; set; }
    }
}
