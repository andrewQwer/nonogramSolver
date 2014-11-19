using System;
using System.Linq;
using Solver.Infrastructure.Models;

namespace Solver.Infrastructure.Services
{
    public interface IRowSolver
    {
        void SolveRow(Row row);
    }

    public class RowSolver : IRowSolver
    {
        private IFinitAutomationRowBuilder faRowBuilder;

        public RowSolver(IFinitAutomationRowBuilder faRowBuilder)
        {
            this.faRowBuilder = faRowBuilder;
        }

        public void SolveRow(Row row)
        {
            if (row == null)
                throw new ArgumentNullException("row");
            if (row.State == RowState.Solved)
                return;
            var cells = row.Cells;
            //solve row if definition fills full row with 1 item
            if (row.Blocks.Count() == 1 && row.Blocks.First().Length == row.Cells.Count)
            {
                cells.ForEach(x => x.Color = row.Blocks.First().Color);
            }
            var faRow = faRowBuilder.BuildFARow(row.Definition);
            //check whether all cells have been solved
            if (row.Cells.TrueForAll(x => x.IsSolved))
            {
                row.State = RowState.Solved;
            }
        }
    }
}