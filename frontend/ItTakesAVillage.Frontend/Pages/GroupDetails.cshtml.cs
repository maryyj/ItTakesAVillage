namespace ItTakesAVillage.Frontend.Pages
{
    public class GroupDetailsModel(
        UserManager<ItTakesAVillageUser> userManager,
        IHttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly IHttpService _httpService = httpService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        public Group? CurrentGroup { get; set; }
        [BindProperty]
        public UserGroup NewUserGroup { get; set; } = new();
        [BindProperty]
        public DinnerInvitation EditDinnerInvitation { get; set; } = new();
        [BindProperty]
        public PlayDate EditPlayDate { get; set; } = new();
        public List<UserGroup>? UsersInGroup { get; set; } = [];
        public List<BaseEvent> EventsOfGroup { get; set; } = [];
        public async Task<IActionResult> OnGet(int groupId)
        {
            if (groupId == 0)
                return RedirectToPage("/Group");

            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
            {
                CurrentGroup = await _httpService.HttpGetRequest<Group>($"Group/{groupId}");
                UsersInGroup = await _httpService.HttpGetRequest<List<UserGroup>>($"Group/UsersGroup/{groupId}");

                var dinnerinvitationOfGroup = await _httpService.HttpGetRequest<List<DinnerInvitation>>($"DinnerInvitation/AllForUserGroup/{groupId}");
                var playdatesOfGroup = await _httpService.HttpGetRequest<List<PlayDate>>($"PlayDate/AllForUserGroup/{groupId}");

                if (dinnerinvitationOfGroup != null && playdatesOfGroup != null)
                    SortAndAddLists(dinnerinvitationOfGroup, playdatesOfGroup);

                await SetCreator(EventsOfGroup);
                var allUsers = _userManager.Users.Where(x => x.Id != CurrentUser.Id).ToList();
                ViewData["UserId"] = new SelectList(allUsers, "Id", "Email");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromGroup(string userId, int groupId)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
            await _httpService.HttpDeleteRequest<Group>($"Group/{userId}/{groupId}");
            return RedirectToPage("/Group");
        }
        public async Task<IActionResult> OnPostAddUserToGroup(int groupId)
        {
            if (ModelState.IsValid)
            {
                await _httpService.HttpPostRequest($"Group/{groupId}",NewUserGroup.UserId);
            }
            return RedirectToPage("/GroupDetails", new { groupId });
        }
        public async Task<IActionResult> OnPostEditDinnerInvitation()
        {
            if (ModelState.IsValid)
            {
                EditDinnerInvitation.Creator = await _userManager.GetUserAsync(User);
                await _httpService.HttpPutRequest("DinnerInvitation" , EditDinnerInvitation);
            }
            return RedirectToPage("GroupDetails", new { EditDinnerInvitation.GroupId });
        }
        public async Task<IActionResult> OnPostEditPlayDate()
        {
            if (ModelState.IsValid)
            {
                EditPlayDate.Creator = await _userManager.GetUserAsync(User);
                await _httpService.HttpPutRequest("PlayDate" , EditPlayDate);
            }
            return RedirectToPage("GroupDetails", new { EditPlayDate.GroupId });
        }
        public async Task<IActionResult> OnPostDeletePlayDate(int eventId, int groupId)
        {
            if (ModelState.IsValid)
            {
                await _httpService.HttpDeleteRequest<PlayDate>($"PlayDate/{eventId}");
            }
            return RedirectToPage("/GroupDetails", new { groupId });
        }
        public async Task<IActionResult> OnPostDeleteDinnerInvitation(int eventId, int groupId)
        {
            if (ModelState.IsValid)
            {
                await _httpService.HttpDeleteRequest<DinnerInvitation>($"DinnerInvitation/{eventId}");
            }
            return RedirectToPage("/GroupDetails", new { groupId });
        }
        private async Task SetCreator(List<BaseEvent> eventsOfGroup)
        {
            foreach (var baseEvent in eventsOfGroup)
            {
                baseEvent.Creator = await _userManager.FindByIdAsync(baseEvent.CreatorId);
            }
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
