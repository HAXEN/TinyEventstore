using Microsoft.AspNetCore.Mvc;
using TinyEventstore.Commanding;

namespace TinyEventstore.Samples.Api
{
    public class CreateTodoCommandHandler : CommandHandlerBase<CreateTodoCommand>
    {
        protected override IActionResult Handle(CreateTodoCommand command)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CreateTodoCommand : CommandBase
    {
    }
}
