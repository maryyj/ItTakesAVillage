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

    [HttpGet("members/{id}")]
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
    [HttpGet("usersgroup/{id}")]
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
    [HttpGet("groupsofuser/{id}")]
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
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Group group, string userId)
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
    public async Task<IActionResult> AddUserAsync([FromBody] string userId, int groupId)
    {
        try
        {
            var result = await _groupService.AddUserAsync(userId, groupId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string userId, int id)
    {
        try
        {
            var deleted = await _groupService.DeleteUserAsync(userId, id);
            return Ok(deleted);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
