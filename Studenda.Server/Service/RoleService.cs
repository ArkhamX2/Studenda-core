﻿
﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Studenda.Core.Server.Security.Service
{
    public class RoleService(RoleManager<IdentityRole> roleManager)
    {
        private readonly RoleManager<IdentityRole> roleManager = roleManager;

        public async Task<bool> Post(string roleName)
        {
            bool roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var role = new IdentityRole { Name= roleName };
                await roleManager.CreateAsync(role);
                return true;
            }
            return false;
        }
        public async Task<List<IdentityRole>> GetRolesList()
        {
            return await roleManager.Roles.ToListAsync();
        }
        public async Task<bool> DeleteRole(string rolename)
        {
            var Role = await roleManager.FindByNameAsync(rolename);
            if (Role != null)
            {
                await roleManager.DeleteAsync(Role);
                return true;
            }
            return false;

        }
        public async Task<IdentityRole> EditRole(string rolename)
        {

            var Role = await roleManager.FindByNameAsync(rolename);
            if (Role != null)
            {
                Role.Name=rolename;
                var result = await roleManager.UpdateAsync(Role);
                if (result.Succeeded)
                {
                    return Role;
                }
                else
                {
                    throw new Exception("Problem with role edit");
                }
            }
            else
            {
                throw new ArgumentException($"Role with name {rolename} cannot be found");
            }
        }
    }
}