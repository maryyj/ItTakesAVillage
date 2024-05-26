namespace ItTakesAVillage.API.Controllers;

[Route("api/[controller]")]

[ApiController]
public class DinnerInvitationController(IEventService<DinnerInvitation> dinnerInvitationService) : ControllerBase
{
    private readonly IEventService<DinnerInvitation> _dinnerInvitationService = dinnerInvitationService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var invitation = await _dinnerInvitationService.GetAsync(id);
            return Ok(invitation);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var invitations = await _dinnerInvitationService.GetAllAsync();
            return Ok(invitations);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("AllForUserGroup/{id}")]
    public async Task<IActionResult> GetAllForUserGroupsAsync(int id)
    {
        try
        {
            var invitations = await _dinnerInvitationService.GetAllOfGroupAsync(id);
            return Ok(invitations);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] DinnerInvitation invitation)
    {
        try
        {
            var result = await _dinnerInvitationService.CreateAsync(invitation);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] DinnerInvitation invitation)
    {
        try
        {
            var result = await _dinnerInvitationService.UpdateAsync(invitation);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var result = await _dinnerInvitationService.DeleteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
