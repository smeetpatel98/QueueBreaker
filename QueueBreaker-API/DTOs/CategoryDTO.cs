using System;
using System.Collections.Generic;

namespace QueueBreaker_API.DTOs
{
    public class CategoryDTO
    {
        //public CategoryDTO()
        //{
        //    Items = new HashSet<ItemsDTO>();
        //}

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

        public virtual CanteenDTO Canteen { get; set; }
        public virtual ICollection<ItemsDTO> Items { get; set; }
    }
    public class CategoryCreateDTO
    {
        public int CanteenId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ImageFile { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
    public class CategoryUpdateDTO
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
    }
}
