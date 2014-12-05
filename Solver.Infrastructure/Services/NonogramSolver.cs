using System;
using Solver.Infrastructure.Models;

namespace Solver.Infrastructure.Services
{
    public interface INonogramSolver
    {
        void Solve(Nonogram nonogram);
    }

    public class NonogramSolver : INonogramSolver
    {
        private IRowSolver rowSolver;

        public NonogramSolver(IRowSolver rowSolver)
        {
            this.rowSolver = rowSolver;
        }

        public void Solve(Nonogram nonogram)
        {
            if (nonogram == null)
                throw new ArgumentNullException("nonogram");

            if (nonogram.HorizontalRows.Count == 0 || nonogram.VerticalRows.Count == 0)
                throw new ArgumentException("Nonogram is empty", "nonogram");
        }
    }
}