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
            var cells = row.Cells;
            if (row.Blocks.Count() == 1 && row.Blocks.First().Length == row.Cells.Count)
            {
                cells.ForEach(x=>x.Color = row.Blocks.First().Color);
            }
        }
    }
}