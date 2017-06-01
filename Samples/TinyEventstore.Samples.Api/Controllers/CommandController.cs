using Microsoft.AspNetCore.Mvc;

namespace TinyEventstore.Samples.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Command/{commandName}")]
    public class CommandController : Controller
    {
        private readonly ICommandResolver _commandResolver;

        public CommandController(ICommandResolver commandResolver)
        {
            _commandResolver = commandResolver;
        }

        [HttpPost]
        public IActionResult Execute(string commandName, [FromBody] CommandBase command)
        {
            return _commandResolver.Execute(commandName, command);
        }
    }

    public abstract class CommandBase { }
}