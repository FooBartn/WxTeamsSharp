using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WxTeamsSharp.Models.Messages;
using WxTeamsSharp.Models.Webhooks;
using WxTeamsWebhookReceiver.Services;

namespace WxTeamsWebhookReceiver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly TeamsService _teamsService;

        public MessagesController(TeamsService teamsService)
        {
            _teamsService = teamsService;
        }

        // POST api/messages
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] WebhookData<Message> value)
        {
            // Teams seems to just be hitting this over and over.. really confused.
            await _teamsService.HandleCreatedMessage(value);
            return new JsonResult(value);
        }
    }
}
