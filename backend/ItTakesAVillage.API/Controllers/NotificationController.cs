namespace ItTakesAVillage.API.Controllers;

[Route("api/[controller]")]

[ApiController]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var notification = await _notificationService.GetAsync(id);
            return Ok(notification);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("Count/{id}")]
    public async Task<IActionResult> GetCountAsync(string id)
    {
        try
        {
            var amount = await _notificationService.GetCountAsync(id);
            return Ok(amount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("All/{id}")]
    public async Task<IActionResult> GetAllAsync(string id)
    {
        try
        {
            var notifications = await _notificationService.GetAllAsync(id);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] Notification notification)
    {
        try
        {
            await _notificationService.UpdateIsReadAsync(notification);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}