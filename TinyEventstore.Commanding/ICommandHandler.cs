using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace TinyEventstore.Commanding
{
    public interface ICommandHandler
    {
        ICommandResult Handle(CommandBase command);
    }

    public enum CommandExecutionStatus
    {
        Unhandled,
        Successful,
        InputValidationErrors,
        DomainValidationErrors,
        ConcurrencyError
    }

    public interface ICommandResult
    {
        bool WasSuccessfull();
        CommandBase Command { get; }
        CommandExecutionStatus Status { get; }
        IEnumerable<IValidationResult> ValidationErrors();
    }

    public interface IValidatable
    {
        bool IsValid();
        IEnumerable<IValidationResult> GetValidationResults();
    }

    public interface IValidationResult
    {
        string Property { get; }
        string Message { get; }
    }
}