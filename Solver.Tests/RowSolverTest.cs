using System;
using System.Drawing;
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
            Assert.True(row.Cells.TrueForAll(c => c.State == CellState.Colored));
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
            var assumedSolvedCells = row.Cells.GetRange(freeSpace, freeSpace/2);

            Assert.True(assumedSolvedCells.TrueForAll(x => x.State == CellState.Colored));
            Assert.True(assumedSolvedCells.TrueForAll(x => x.Color == Cell.DefaultColor));

            var assumedUndefinedCells = row.Cells.Except(assumedSolvedCells);
            Assert.True(assumedUndefinedCells.All(x => x.State == CellState.Undefined));
        }

        [Test]
        public void Solve_simple_row_with_2_items()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(3);
            rowDef.AddItem(2);
            var row = new Row(rowDef, 9);
            row.Cells[1].SetDelimeter();
            row.Cells[4].SetColor(Color.Black);
            rowSolver.SolveRow(row);
            Assert.True(row.Cells[3].Color == Color.Black);
            Assert.True(row.Cells[4].Color == Color.Black);
            Assert.True(row.Cells[7].Color == Color.Black);
        }

        [Test]
        public void Solve_simple_row_with_2_items_2()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(3);
            rowDef.AddItem(2);
            var row = new Row(rowDef, 6);
            rowSolver.SolveRow(row);
            Assert.True(row.Cells[0].Color == Color.Black);
            Assert.True(row.Cells[1].Color == Color.Black);
            Assert.True(row.Cells[2].Color == Color.Black);
            Assert.True(row.Cells[3].Color == Color.Empty);
            Assert.True(row.Cells[3].State == CellState.Delimeter);
            Assert.True(row.Cells[4].Color == Color.Black);
            Assert.True(row.Cells[5].Color == Color.Black);
            Assert.AreEqual(RowState.Solved, row.State);
        }

        [Test]
        public void Solve_simple_row_with_all_undefined_cell()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(1);
            var row = new Row(rowDef, 10);
            rowSolver.SolveRow(row);
            Assert.True(row.Cells.All(c => c.IsUndefined));
        }

        [Test]
        public void Solve_simple_row_without_solution()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(5);
            var row = new Row(rowDef, 5);
            row.Cells[1].SetDelimeter();
            rowSolver.SolveRow(row);
            Assert.AreEqual(RowState.NoSolution, row.State);
        }
    }
}