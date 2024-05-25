namespace ItTakesAVillage.API.Controllers
{
    [Route("/")]
    [ApiController]
    public class ToolLoanController(IEventService<ToolLoan> toolLoanService) : ControllerBase
    {
        private readonly IEventService<ToolLoan> _toolLoanService = toolLoanService;
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var tool = await _toolLoanService.GetAsync(id);
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
                var tools = await _toolLoanService.GetAllAsync();
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
                var tools = await _toolLoanService.GetAllOfGroupAsync(id);
                return Ok(tools);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ToolLoan toolLoan)
        {
            try
            {
                var result = await _toolLoanService.CreateAsync(toolLoan);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutProduct([FromBody] ToolLoan toolLoan)
        {
            try
            {
                var result = await _toolLoanService.UpdateAsync(toolLoan);
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
                var deleted = await _toolLoanService.DeleteAsync(id);
                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
