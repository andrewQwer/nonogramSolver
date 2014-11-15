using System;
using System.Collections.Generic;
using StructureMap;

namespace Solver.Infrastructure.DI
{
    public class StructuremapServiceLocator : IServiceLocator
    {
        readonly IContainer container;

        public StructuremapServiceLocator(IContainer container)
        {
            this.container = container;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return container.GetAllInstances<T>();
        }

        public T Get<T>()
        {
            return container.GetInstance<T>();
        }

        public T Get<T>(string name)
        {
            return container.GetInstance<T>(name);
        }

        public object Get(Type type)
        {
            return container.GetInstance(type);
        }
    }
}