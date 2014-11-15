using System;
using System.Linq;
using NUnit.Framework;
using Solver.Infrastructure.DI;
using Solver.Infrastructure.Models;
using Solver.Infrastructure.Services;

namespace Solver.Tests
{
    [TestFixture]
    public class RowSolverTest
    {
        private IRowSolver rowSolver;
        private IServiceLocator locator;
        public RowSolverTest()
        {
            locator = StructureMapTest.Configure();
            rowSolver = locator.Get<IRowSolver>();
        }

        [Test]
        public void Null_rows_doesnt_allowed()
        {
            Assert.Throws<ArgumentNullException>(() => rowSolver.SolveRow(null));
        }

        [Test]
        public void Solve_completely_filled_row()
        {
            var rowLength = 5;
            var rowDef = new RowDefinition();
            rowDef.AddItem(rowLength);
            var row = new Row(rowDef, rowLength);
            rowSolver.SolveRow(row);
            Assert.True(row.Cells.TrueForAll(c => c.State == CellState.Solved));
            Assert.True(row.Cells.TrueForAll(c => c.Color == Cell.DefaultColor));
        }

        [Test]
        public void Solve_simple_row_with_1_item()
        {
            var rowLength = 10;
            var freeSpace = 4;
            var rowDef = new RowDefinition();
            rowDef.AddItem(rowLength - freeSpace);
            var row = new Row(rowDef, rowLength);
            rowSolver.SolveRow(row);
            var assumedSolvedCells = row.Cells.GetRange(freeSpace, (rowLength - freeSpace)/2);

            Assert.True(assumedSolvedCells.TrueForAll(x => x.State == CellState.Solved));
            Assert.True(assumedSolvedCells.TrueForAll(x => x.Color == Cell.DefaultColor));

            var assumedUndefinedCells = row.Cells.Except(assumedSolvedCells);
            Assert.True(assumedUndefinedCells.All(x => x.State == CellState.Undefined));
        }
    }
}