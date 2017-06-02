namespace TinyEventstore.Commanding
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler
        where TCommand : CommandBase
    {
        protected abstract ICommandResult Handle(TCommand command);

        public ICommandResult Handle(CommandBase command)
        {
            return Handle((TCommand) command);
        }
    }
}