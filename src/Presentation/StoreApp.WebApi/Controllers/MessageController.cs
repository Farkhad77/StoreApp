using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Abstracts.Rabbit;

namespace StoreApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IRabbitMqProducer _producer;

        public MessageController(IRabbitMqProducer producer)
        {
            _producer = producer;
        }

        [HttpPost]
        public IActionResult Send([FromBody] string text)
        {
            _producer.SendMessageAsync(new { Text = text, Date = DateTime.UtcNow });
            return Ok("Mesaj göndərildi!");
        }
    }

}
