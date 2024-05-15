namespace ItTakesAVillage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupChatController(IGroupChatService groupChatService) : ControllerBase
    {
        private readonly IGroupChatService _groupChatService = groupChatService;
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var messages = await _groupChatService.GetAsync(id);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]GroupChat message)
        {
            try
            {
                var result = await _groupChatService.CreateAsync(message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


//Task<List<GroupChat>> GetAsync(int groupId);
//Task<bool> AddAsync(GroupChat message);