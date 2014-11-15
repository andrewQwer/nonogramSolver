using System;
using System.Collections.Generic;

namespace Solver.Infrastructure.DI
{
    public interface IServiceLocator
    {
        IEnumerable<T> GetAll<T>();
        T Get<T>();
        T Get<T>(string name);
        object Get(Type type);
    }
}