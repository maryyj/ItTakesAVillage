namespace ItTakesAVillage.API.Controllers;

//[Route("api/[controller]")]
[Route("/")]
[ApiController]
public class ToolPoolController(IEventService<ToolPool> toolPoolService) : ControllerBase
{
    private readonly IEventService<ToolPool> _toolPoolService = toolPoolService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var tool = await _toolPoolService.GetAsync(id);
            return Ok(tool);
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
            var tools = await _toolPoolService.GetAllAsync();
            return Ok(tools);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("AllForUserGroup/{id}")]
    public async Task<IActionResult> GetAllForUserGroupsAsync(string id)
    {
        try
        {
            var tools = await _toolPoolService.GetAllOfGroupAsync(id);
            return Ok(tools);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ToolPool toolPool)
    {
        try
        {
            var result = await _toolPoolService.CreateAsync(toolPool);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] ToolPool toolPool)
    {
        try
        {
            var result = await _toolPoolService.UpdateAsync(toolPool);
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
            var result = await _toolPoolService.DeleteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}