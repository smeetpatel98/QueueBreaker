using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QueueBreaker_UI.WASM.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int CanteenId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ImageFile { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual Canteen Canteen { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
