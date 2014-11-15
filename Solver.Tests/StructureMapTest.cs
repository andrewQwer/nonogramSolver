using Solver.Infrastructure.DI;
using StructureMap;

namespace Solver.Tests
{
    public static class StructureMapTest
    {
        public static IServiceLocator Configure()
        {
            var container = new Container(x => x.AddRegistry<AppRegistry>());
            return new StructuremapServiceLocator(container);
        }
    }
}