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
            Assert.Throws<ArgumentException>(() => solver.Solve(new Nonogram(5, 5)));
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

        [Test]
        public void Can_solve_complex_nanogram_crown()
        {
            var nanogram = new Nonogram(40, 50);
            nanogram.AddHorizontalRow(new RowDefinition(5));
            nanogram.AddHorizontalRow(new RowDefinition(7));
            nanogram.AddHorizontalRow(new RowDefinition(3, 2, 3));
            nanogram.AddHorizontalRow(new RowDefinition(7, 3, 4));
            nanogram.AddHorizontalRow(new RowDefinition(2, 3, 6, 4));
            nanogram.AddHorizontalRow(new RowDefinition(3, 3, 2, 3));
            nanogram.AddHorizontalRow(new RowDefinition(3, 2, 7, 3));
            nanogram.AddHorizontalRow(new RowDefinition(4, 2, 3, 1, 2));
            nanogram.AddHorizontalRow(new RowDefinition(5, 2, 4, 2));
            nanogram.AddHorizontalRow(new RowDefinition(6, 5, 3));
            nanogram.AddHorizontalRow(new RowDefinition(7, 1, 2, 2, 4));
            nanogram.AddHorizontalRow(new RowDefinition(6, 5, 1, 3, 6));
            nanogram.AddHorizontalRow(new RowDefinition(5, 6, 1, 3, 8));
            nanogram.AddHorizontalRow(new RowDefinition(4, 6, 4, 1, 6));
            nanogram.AddHorizontalRow(new RowDefinition(4, 5, 1, 3, 1, 2, 2, 2));
            nanogram.AddHorizontalRow(new RowDefinition(5, 4, 1, 2, 2, 1, 3, 2, 3));
            nanogram.AddHorizontalRow(new RowDefinition(5, 3, 2, 2, 1, 1, 2, 1, 1, 4));
            nanogram.AddHorizontalRow(new RowDefinition(5, 3, 2, 2, 1, 1, 1, 2, 3, 3));
            nanogram.AddHorizontalRow(new RowDefinition(4, 2, 2, 2, 2, 1, 8, 3));
            nanogram.AddHorizontalRow(new RowDefinition(3, 2, 3, 1, 4, 1, 4, 2, 3));
            nanogram.AddHorizontalRow(new RowDefinition(2, 1, 3, 1, 1, 1, 2, 3, 2));
            nanogram.AddHorizontalRow(new RowDefinition(6, 2, 4, 10, 1, 2));
            nanogram.AddHorizontalRow(new RowDefinition(4, 4, 2, 6, 8, 2, 3));
            nanogram.AddHorizontalRow(new RowDefinition(1, 2, 2, 3, 7, 5, 3, 3));
            nanogram.AddHorizontalRow(new RowDefinition(2, 2, 2, 3, 7, 1, 6, 1, 2));
            nanogram.AddHorizontalRow(new RowDefinition(2, 2, 2, 4, 7, 2, 4, 1, 2));
            nanogram.AddHorizontalRow(new RowDefinition(3, 2, 2, 5, 6, 4, 2, 2));
            nanogram.AddHorizontalRow(new RowDefinition(1, 2, 2, 3, 4, 1, 3, 8, 3));
            nanogram.AddHorizontalRow(new RowDefinition(2, 2, 2, 2, 4, 4, 2, 5, 4));
            nanogram.AddHorizontalRow(new RowDefinition(3, 5, 2, 3, 7, 2, 2, 5));
            nanogram.AddHorizontalRow(new RowDefinition(4, 4, 3, 2, 9, 7));
            nanogram.AddHorizontalRow(new RowDefinition(1, 2, 4, 2, 1, 15, 6));
            nanogram.AddHorizontalRow(new RowDefinition(2, 2, 4, 2, 17, 3));
            nanogram.AddHorizontalRow(new RowDefinition(2, 2, 2, 3, 2, 16, 1));
            nanogram.AddHorizontalRow(new RowDefinition(2, 1, 1, 2, 3, 1, 1, 15));
            nanogram.AddHorizontalRow(new RowDefinition(2, 1, 1, 2, 4, 2, 14));
            nanogram.AddHorizontalRow(new RowDefinition(3, 3, 5, 2, 10));
            nanogram.AddHorizontalRow(new RowDefinition(2, 1, 6, 2, 5));
            nanogram.AddHorizontalRow(new RowDefinition(2, 1, 4, 2, 3));
            nanogram.AddHorizontalRow(new RowDefinition(2, 3, 3, 1));
            nanogram.AddHorizontalRow(new RowDefinition(1, 3, 7, 1));
            nanogram.AddHorizontalRow(new RowDefinition(1, 3, 7, 1));
            nanogram.AddHorizontalRow(new RowDefinition(1, 3, 5));
            nanogram.AddHorizontalRow(new RowDefinition(1, 8, 4));
            nanogram.AddHorizontalRow(new RowDefinition(1, 3, 1, 3));
            nanogram.AddHorizontalRow(new RowDefinition(2, 3, 2, 2));
            nanogram.AddHorizontalRow(new RowDefinition(2, 3, 7));
            nanogram.AddHorizontalRow(new RowDefinition(2, 3, 4, 1));
            nanogram.AddHorizontalRow(new RowDefinition(6));
            nanogram.AddHorizontalRow(new RowDefinition(5));

            nanogram.AddVerticalRow(new RowDefinition(8));
            nanogram.AddVerticalRow(new RowDefinition(3,5));
            nanogram.AddVerticalRow(new RowDefinition(6,3,4));
            nanogram.AddVerticalRow(new RowDefinition(8,1,4,3));
            nanogram.AddVerticalRow(new RowDefinition(2,7,5,8));
            nanogram.AddVerticalRow(new RowDefinition(3,6,1,6,7));
            nanogram.AddVerticalRow(new RowDefinition(5,10,2,7,6,4));
            nanogram.AddVerticalRow(new RowDefinition(5,11,3,6,1,3));
            nanogram.AddVerticalRow(new RowDefinition(3,1,1,12,10,3,2));
            nanogram.AddVerticalRow(new RowDefinition(3,1,1,1,13,7,2,2,3));
            nanogram.AddVerticalRow(new RowDefinition(2,2,1,2,4,4,2,3,5));
            nanogram.AddVerticalRow(new RowDefinition(2,2,2,2,3,9,2,4,7));
            nanogram.AddVerticalRow(new RowDefinition(2,2,2,1,9,2,6,2,4,5));
            nanogram.AddVerticalRow(new RowDefinition(3,1,2,1,8,2,5,6,4));
            nanogram.AddVerticalRow(new RowDefinition(2,1,1,2,7,3,4,3,4,4));
            nanogram.AddVerticalRow(new RowDefinition(2,2,1,6,3,8,2,1,3,1));
            nanogram.AddVerticalRow(new RowDefinition(3,1,4,4,11,2,5,2));
            nanogram.AddVerticalRow(new RowDefinition(2,2,4,2,6,1,5,2));
            nanogram.AddVerticalRow(new RowDefinition(4,4,2,3,3,3,2));
            nanogram.AddVerticalRow(new RowDefinition(7,6,6,6,2));
            nanogram.AddVerticalRow(new RowDefinition(5,6,1,15,4));
            nanogram.AddVerticalRow(new RowDefinition(4,2,6,7,2,1,2));
            nanogram.AddVerticalRow(new RowDefinition(3,5,6,8,2,1,2));
            nanogram.AddVerticalRow(new RowDefinition(1,2,2,5,9,3,2,2));
            nanogram.AddVerticalRow(new RowDefinition(2,3,1,5,8,1,1,1,2));
            nanogram.AddVerticalRow(new RowDefinition(2,2,4,3,9,1,1,4));
            nanogram.AddVerticalRow(new RowDefinition(5,3,5,2,8,1,3));
            nanogram.AddVerticalRow(new RowDefinition(4,2,2,3,2,1,8,1,1));
            nanogram.AddVerticalRow(new RowDefinition(4,1,3,3,3,1,8,1));
            nanogram.AddVerticalRow(new RowDefinition(4,1,3,4,2,1,8));
            nanogram.AddVerticalRow(new RowDefinition(4,1,2,4,3,8));
            nanogram.AddVerticalRow(new RowDefinition(4,2,3,2,3,8));
            nanogram.AddVerticalRow(new RowDefinition(8,2,3,8));
            nanogram.AddVerticalRow(new RowDefinition(1,5,3,2,1,6));
            nanogram.AddVerticalRow(new RowDefinition(3,4,2,2,4));
            nanogram.AddVerticalRow(new RowDefinition(9,2,8));
            nanogram.AddVerticalRow(new RowDefinition(6,3,5,3));
            nanogram.AddVerticalRow(new RowDefinition(4,3,6));
            nanogram.AddVerticalRow(new RowDefinition(11));
            nanogram.AddVerticalRow(new RowDefinition(10));

            var stat = solver.Solve(nanogram);
            Assert.AreEqual(NonogramState.Solved, nanogram.State);
        }
    }
}