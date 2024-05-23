namespace ItTakesAVillage.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroupService groupService) : ControllerBase
{
    private readonly IGroupService _groupService = groupService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var group = await _groupService.GetAsync(id);
            return Ok(group);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Members/{id}")]
    public async Task<IActionResult> GetMembersAsync(int id)
    {
        try
        {
            var members = await _groupService.GetMembersAsync(id);
            return Ok(members);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("Usersgroup/{id}")]
    public async Task<IActionResult> GetUsersAndGroupsAsync(int id)
    {
        try
        {
            var userGroups = await _groupService.GetUsersAndGroupsAsync(id);
            return Ok(userGroups);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("GroupsOfUser/{id}")]
    public async Task<IActionResult> GetGroupsByUserIdAsync(string id)
    {
        try
        {
            var groups = await _groupService.GetGroupsByUserIdAsync(id);
            return Ok(groups);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("CreateGroup/{userId}")]
    public async Task<IActionResult> CreateAsync(string userId, [FromBody] Group group)
    {
        try
        {
            var result = await _groupService.CreateAsync(group, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("{id}")]
    public async Task<IActionResult> AddUserAsync(int id, [FromBody] string userId)
    {
        try
        {
            var result = await _groupService.AddUserAsync(userId, id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{userId}/{groupId}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string userId, int groupId)
    {
        try
        {
            var deleted = await _groupService.DeleteUserAsync(userId, groupId);
            return Ok(deleted);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
