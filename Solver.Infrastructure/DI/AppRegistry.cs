using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Solver.Infrastructure.DI
{
    public class AppRegistry: Registry
    {
        public AppRegistry()
        {
            Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.WithDefaultConventions();
                });
        }
    }
}