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
            Assert.Throws<ArgumentException>(() => solver.Solve(new Nonogram(5,5)));
        }

        [Test]
        public void Can_solve_simple_nanogram_heart()
        {
            var nanogram = new Nonogram(9, 9);
            nanogram.AddHorizontalRow(new RowDefinition(2, 2));
            nanogram.AddHorizontalRow(new RowDefinition(4, 4));
            nanogram.AddHorizontalRow(new RowDefinition(9));
            nanogram.AddHorizontalRow(new RowDefinition(9));
            nanogram.AddHorizontalRow(new RowDefinition(9));
            nanogram.AddHorizontalRow(new RowDefinition(7));
            nanogram.AddHorizontalRow(new RowDefinition(5));
            nanogram.AddHorizontalRow(new RowDefinition(3));
            nanogram.AddHorizontalRow(new RowDefinition(1));

            nanogram.AddVerticalRow(new RowDefinition(4));
            nanogram.AddVerticalRow(new RowDefinition(6));
            nanogram.AddVerticalRow(new RowDefinition(7));
            nanogram.AddVerticalRow(new RowDefinition(7));
            nanogram.AddVerticalRow(new RowDefinition(7));
            nanogram.AddVerticalRow(new RowDefinition(7));
            nanogram.AddVerticalRow(new RowDefinition(7));
            nanogram.AddVerticalRow(new RowDefinition(6));
            nanogram.AddVerticalRow(new RowDefinition(4));

            var stat = solver.Solve(nanogram);
            Assert.AreEqual(NonogramState.Solved, nanogram.State);
        }
    }
}