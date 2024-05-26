namespace ItTakesAVillage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IItTakesAVillageUserService itTakesAVillageUserService) : ControllerBase
    {
        private readonly IItTakesAVillageUserService _itTakesAVillageUserService = itTakesAVillageUserService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var user = await _itTakesAVillageUserService.GetAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
