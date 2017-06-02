using System.Linq;
using System.Reflection;
using Xunit;

namespace TinyEventstore.Commanding.Tests
{
    public class HandlerResolverTests
    {
        [Fact]
        public void Should_be_able_to_Resolve_all_types_Derived_from_CommandHandlerBase()
        {
            var resolved = CommandResolver.KnownHandlers(typeof(HandlerResolverTests).GetTypeInfo().Assembly, typeof(CommandBase).GetTypeInfo().Assembly);

            Assert.NotNull(resolved);
            Assert.NotEmpty(resolved);
            Assert.Equal(2, resolved.Count());
            Assert.Contains(typeof(TestCommand1Handler), resolved);
            Assert.Contains(typeof(TestCommand2Handler), resolved);
        }

        public class TestCommand2Handler : CommandHandlerBase<TestCommand2>
        {
            protected override ICommandResult Handle(TestCommand2 command)
            {
                throw new System.NotImplementedException();
            }
        }

        public class TestCommand1Handler : CommandHandlerBase<TestCommand1>
        {
            protected override ICommandResult Handle(TestCommand1 command)
            {
                throw new System.NotImplementedException();
            }
        }

        public class TestCommand2 : CommandBase { }

        public class TestCommand1 : CommandBase { }
    }
}
