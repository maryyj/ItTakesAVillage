namespace ItTakesAVillage.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PlayDateController(
    IEventService<PlayDate> playDateService,
    INotificationService notificationService) : ControllerBase
{
    private readonly IEventService<PlayDate> _playDateService = playDateService;
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var playDate = await _playDateService.GetAsync(id);
            return Ok(playDate);
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
            var playDates = await _playDateService.GetAllAsync();
            return Ok(playDates);
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
            var playDates = await _playDateService.GetAllOfGroupAsync(id);
            return Ok(playDates);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] PlayDate playDate)
    {
        try
        {
            var result = await _playDateService.CreateAsync(playDate);
            if (result)
                await _notificationService.CreateNotificationAsync(playDate);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] PlayDate playDate)
    {
        try
        {
            var result = await _playDateService.UpdateAsync(playDate);
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
            var result = await _playDateService.DeleteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}