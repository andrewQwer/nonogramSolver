using System;
using NUnit.Framework;
using Solver.Infrastructure.DI;
using Solver.Infrastructure.Models;
using Solver.Infrastructure.Services;

namespace Solver.Tests
{
    [TestFixture]
    public class NonogramSolverTest
    {
        private INonogramSolver solver;
        private IServiceLocator locator;

        public NonogramSolverTest()
        {
            locator = StructureMapTest.Configure();
            solver = new NonogramSolver(locator.Get<IRowSolver>());
        }

        [Test]
        public void Cant_solve_null_nonogram()
        {
            Assert.Throws<ArgumentNullException>(() => solver.Solve(null));
        }

        [Test]
        public void Cant_solve_empty_nonogram()
        {
            Assert.Throws<ArgumentException>(() => solver.Solve(new Nonogram()));
        }
    }
}