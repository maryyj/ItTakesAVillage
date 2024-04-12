using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Helper;
using ItTakesAVillage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.RegularExpressions;

namespace ItTakesAVillage.Pages
{
    public class GroupModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<ItTakesAVillageUser> _userManager;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public Models.Group NewGroup { get; set; } = new Models.Group();
        [BindProperty]
        public UserGroup NewUserGroup { get; set; } = new UserGroup();
        public List<Models.Group?> GroupsOfCurrentUser { get; set; } = new();
        public GroupModel(IGroupService groupService, UserManager<ItTakesAVillageUser> userManager)
        {
            _groupService = groupService;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
            {
                var allUsers = _userManager.Users.Where(x => x.Id != CurrentUser.Id).ToList();
                ViewData["UserId"] = new SelectList(allUsers, "Id", "Email");
                GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
                ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostNewGroupAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid && CurrentUser != null)
            {
                NewGroup.CreatorId = CurrentUser.Id;
                var newGroupId = await _groupService.Save(NewGroup, CurrentUser.Id);

                if (newGroupId != 0)
                {
                    await _groupService.AddUser(CurrentUser.Id, newGroupId);
                }
            }
            return RedirectToPage("/Group");
        }
        public async Task<IActionResult> OnPostAddUserToGroupAsync()
        {
            if (ModelState.IsValid)
            {
                await _groupService.AddUser(NewUserGroup.UserId, NewUserGroup.GroupId);
            }
            return RedirectToPage("/Group");
        }
    }
}