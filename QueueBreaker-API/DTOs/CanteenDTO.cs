using System;
using System.Collections.Generic;

namespace QueueBreaker_API.DTOs
{
    public class CanteenDTO
    {
        public CanteenDTO()
        {
            Category = new HashSet<CategoryDTO>();
            Order = new HashSet<OrderDTO>();
            Users = new HashSet<UsersDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }

        public virtual ICollection<CategoryDTO> Category { get; set; }
        public virtual ICollection<OrderDTO> Order { get; set; }
        public virtual ICollection<UsersDTO> Users { get; set; }
    }

    public class CanteenCreateDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }

    public class CanteenUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Status { get; set; }
        public int? CreatedId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
