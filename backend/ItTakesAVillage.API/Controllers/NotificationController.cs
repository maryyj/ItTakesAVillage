namespace ItTakesAVillage.API.Controllers;

[Route("/")]
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
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] JsonElement baseEventJson)
    {
        try
        {
            BaseEvent baseEvent = DeserializeBaseEvent(baseEventJson);

            if (baseEvent != null)
            {
                await _notificationService.CreateNotificationAsync(baseEvent);
                return Ok();
            }
            return BadRequest("Invalid event data.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    private static BaseEvent DeserializeBaseEvent(JsonElement baseEventJson)
    {
        if (baseEventJson.TryGetProperty("Course", out var courseProp) && courseProp.ValueKind != JsonValueKind.Null)
        {
            return JsonSerializer.Deserialize<DinnerInvitation>(baseEventJson.GetRawText());
        }
        if (baseEventJson.TryGetProperty("ChildName", out var childNameProp) && childNameProp.ValueKind != JsonValueKind.Null)
        {
            return JsonSerializer.Deserialize<PlayDate>(baseEventJson.GetRawText());
        }
        if (baseEventJson.TryGetProperty("Name", out var toolNameProp) && toolNameProp.ValueKind != JsonValueKind.Null)
        {
            return JsonSerializer.Deserialize<ToolPool>(baseEventJson.GetRawText());
        }
        throw new ArgumentException("Unknown event type");
    }
}