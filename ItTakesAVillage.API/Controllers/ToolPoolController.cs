namespace ItTakesAVillage.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToolPoolController(IEventService<ToolPool> toolPoolService) : ControllerBase
{
    private readonly IEventService<ToolPool> _toolPoolService = toolPoolService;

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var tools = await _toolPoolService.GetAllAsync();
            return Ok(tools);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllForUserGroupsAsync(string id)
    {
        try
        {
            var tools = await _toolPoolService.GetAllForUserGroupsAsync(id);
            return Ok(tools);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    public async Task <IActionResult> CreateAsync([FromBody]ToolPool toolPool)
    {
        try
        {
            var result = await _toolPoolService.CreateAsync(toolPool);
            return Ok();
        }
        catch(Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var deleted = await _toolPoolService.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}