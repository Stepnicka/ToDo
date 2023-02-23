using Microsoft.Extensions.DependencyInjection;

namespace ToDo.Test
{
    public class Fixture
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public Fixture()
        {
            var serviceCollection = new ServiceCollection();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
