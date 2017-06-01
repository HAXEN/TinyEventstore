using Microsoft.AspNetCore.Mvc;
using TinyEventstore.Samples.Api.Controllers;

namespace TinyEventstore.Samples.Api
{
    public class CreateTodoCommandHandler : CommandHandlerBase<CreateTodoCommand>
    {
        protected override IActionResult Handle(CreateTodoCommand command)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler
        where TCommand : CommandBase
    {
        protected abstract IActionResult Handle(TCommand command);

        public IActionResult Handle(CommandBase command)
        {
            return Handle((TCommand) command);
        }
    }

    public interface ICommandHandler
    {
        IActionResult Handle(CommandBase command);
    }

    public class CreateTodoCommand : CommandBase
    {
    }
}
