using Microsoft.AspNetCore.Mvc;

namespace TinyEventstore.Commanding
{
    public interface ICommandResolver
    {
        IActionResult Execute(string commandName, CommandBase command);
    }
}