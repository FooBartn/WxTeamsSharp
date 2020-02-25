using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
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
        public async Task<JsonResult> Post([FromBody] object value)
        {
            var text = System.Text.Json.JsonSerializer.Serialize(value);
            var data = JsonConvert.DeserializeObject<WebhookData<Message>>(text);
            await _teamsService.HandleCreatedMessage(data);
            return new JsonResult(value);
        }
    }
}
