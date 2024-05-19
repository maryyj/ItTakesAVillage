using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ItTakesAVillage.Pages
{
    public class GroupDetailsModel(
        IEventService<DinnerInvitation> _dinnerInvitationService,
        IEventService<PlayDate> _playdateService,
        IGroupService groupService,
        UserManager<ItTakesAVillageUser> userManager) : PageModel
    {
        private readonly IGroupService _groupService = groupService;
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        public Group? CurrentGroup { get; set; }
        [BindProperty]
        public UserGroup NewUserGroup { get; set; } = new();
        public List<UserGroup?> UsersInGroup { get; set; } = [];
        public List<BaseEvent> EventsOfGroup { get; set; } = [];
        public async Task<IActionResult> OnGet(int groupId)
        {
            if (groupId == 0)
                return RedirectToPage("/Group");

            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
            {
                CurrentGroup = await _groupService.Get(groupId);
                UsersInGroup = await _groupService.GetUsersAndGroups(groupId);
                var dinnerinvitationOfGroup = await _dinnerInvitationService.GetAllOfGroup(groupId);
                var playdatesOfGroup = await _playdateService.GetAllOfGroup(groupId);
                SortAndAddLists(dinnerinvitationOfGroup, playdatesOfGroup);

                var allUsers = _userManager.Users.Where(x => x.Id != CurrentUser.Id).ToList();
                ViewData["UserId"] = new SelectList(allUsers, "Id", "Email");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromGroup(string userId, int groupId)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
                await _groupService.RemoveUser(userId, groupId);
            return RedirectToPage("/Group");
        }
        public async Task<IActionResult> OnPostAddUserToGroupAsync(int groupId)
        {
            if (ModelState.IsValid)
            {
                await _groupService.AddUser(NewUserGroup.UserId, groupId);
            }
            return RedirectToPage("/GroupDetails", new {groupId});
        }
        public async Task<IActionResult> OnPostDeletePlayDate(int eventId, int groupId)
        {
            if(ModelState.IsValid)
            {                
                await _playdateService.Delete(eventId);
            }
            return RedirectToPage("/GroupDetails", new { groupId });
        }
        public async Task<IActionResult> OnPostDeleteDinnerInvitation(int eventId, int groupId)
        {
            if(ModelState.IsValid)
            {
                await _dinnerInvitationService.Delete(eventId);
            }
            return RedirectToPage("/GroupDetails", new { groupId });
        }
        private List<BaseEvent> SortAndAddLists(List<DinnerInvitation> invitation, List<PlayDate> playdate)
        {

            EventsOfGroup.AddRange(invitation.Where(x => x.DateTime.Date >= DateTime.Now.Date));
            EventsOfGroup.AddRange(playdate.Where(x => x.DateTime.Date >= DateTime.Now.Date));
            EventsOfGroup = EventsOfGroup.OrderBy(e => e.DateTime.Date).ToList();
            return EventsOfGroup;
        }
    }
}
