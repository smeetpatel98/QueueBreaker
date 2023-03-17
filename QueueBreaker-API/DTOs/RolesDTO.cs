using System;
using System.Collections.Generic;

namespace QueueBreaker_API.DTOs
{
    public class RolesDTO
    {
        public RolesDTO()
        {
            Users = new HashSet<UsersDTO>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UsersDTO> Users { get; set; }
    }
    public class RolesCreateDTO
    {
        public string Name { get; set; }

    }
    public class RolesUpdateDTO
    {
       
        public int RoleId { get; set; }
        public string Name { get; set; }

    }
}
