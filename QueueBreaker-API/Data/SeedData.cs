using Microsoft.AspNetCore.Identity;
using QueueBreaker_API.Helpers;
using QueueBreaker_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueBreaker_API.Data
{
    public static class SeedData
    {
        public async static Task Seed()
        {
            await SeedRoles();
            await SeedUsers();
        }
        private async static Task SeedUsers()
        {
            RolesRepository _roleRepository = new RolesRepository(new QUEUEBREAKERContext());
            UsersRepository _userRepository = new UsersRepository(new QUEUEBREAKERContext());

            if (await _userRepository.FindByName("superadmin@qb.com") == null)
            {
                var role = await _roleRepository.FindByName(RoleName.SuperAdministrator);
                if (role != null)
                {
                    var user = new User
                    {
                        //FirstName = "Smeet",
                        //LastName = "Patel",
                        Email = "superadmin@qb.com",
                        Password = AESAlgorithm.Encrypt("qb@12345"),
                        RoleId = role.RoleId
                    };
                    await _userRepository.Create(user);
                }
            }
        }

        private async static Task SeedRoles()
        {
            RolesRepository _roleRepository = new RolesRepository(new QUEUEBREAKERContext());

            if (await _roleRepository.FindByName("SuperAdmin") == null)
            {
                var role = new Role
                {
                    Name = "SuperAdmin"
                };
                await _roleRepository.Create(role);
            }
            if (await _roleRepository.FindByName("Admin") == null)
            {
                var role = new Role
                {
                    Name = "Admin"
                };
                await _roleRepository.Create(role);
            }

            if (await _roleRepository.FindByName("Customer") == null)
            {
                var role = new Role
                {
                    Name = "Customer"
                };
                await _roleRepository.Create(role);
            }
        }
    }
}
