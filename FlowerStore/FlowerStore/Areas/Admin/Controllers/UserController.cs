using FlowerStore.Areas.Admin.ViewModel;
using FlowerStore.DAL;
using FlowerStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowerStore.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(AppDbContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _dbContext.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.Name,
                    Email = user.Email,
                    UserName = user.UserName,
                    Role = (await _userManager.GetRolesAsync(user))[0]
                });

            }

            return View(userList);
        }

        public async Task<IActionResult> AddRole(string id)
        {
          
            var user = await _dbContext.Users.FindAsync(id);
            var roles = await _dbContext.Roles.ToListAsync();
            AddRoleViewModel model = new AddRoleViewModel
            {
                user = user,
                Roles=roles,
            };
            if (user==null)
            {
                return NotFound();
            }
            return View(model);
        }
        [ActionName("AddRole")]
        [HttpPost]

        public async Task<IActionResult> AddRolePost()
        {
            var id = Request.Form["userId"];
            var formRole = Request.Form["Role"].ToString();
            var role =  _dbContext.Roles.Where(r=> r.Name==formRole ).FirstOrDefault();
            var user = await _dbContext.Users.FindAsync(id);
            var oldRole = _dbContext.UserRoles.Where(r => r.UserId == user.Id).FirstOrDefault();
            var oldRoleName = await _dbContext.Roles.FindAsync(oldRole.RoleId);
            
            if (user==null)
            {
                return NotFound();
            }

           await _userManager.RemoveFromRoleAsync(user,oldRoleName.Name);
           await _userManager.AddToRoleAsync(user, role.Name);
            
            
            return RedirectToAction("Index");
        }
    }
}
