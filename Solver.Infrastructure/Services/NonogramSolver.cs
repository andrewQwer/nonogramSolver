using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Solver.Infrastructure.Models;

namespace Solver.Infrastructure.Services
{
    public interface INonogramSolver
    {
        SolveStatistics Solve(Nonogram nonogram);
    }

    public class NonogramSolver : INonogramSolver
    {
        private IRowSolver rowSolver;

        public NonogramSolver(IRowSolver rowSolver)
        {
            this.rowSolver = rowSolver;
        }

        public SolveStatistics Solve(Nonogram nonogram)
        {
            if (nonogram == null)
                throw new ArgumentNullException("nonogram");

            if (nonogram.HorizontalRows.Count == 0 || nonogram.VerticalRows.Count == 0)
                throw new ArgumentException("Nonogram is empty", "nonogram");

            var res = new SolveStatistics();
            while (!nonogram.IsSolved && res.IterationsCount < 100)
            {
                Parallel.ForEach(nonogram.HorizontalRows, rowSolver.SolveRow);
                Debug.WriteLine(nonogram.ToString());
                res.IterationsCount++;

                Parallel.ForEach(nonogram.VerticalRows, rowSolver.SolveRow);
                Debug.WriteLine(nonogram.ToString());
                res.IterationsCount++;
                if (nonogram.HorizontalRows.TrueForAll(x => x.State == RowState.Solved))
                {
                    nonogram.SetState(NonogramState.Solved);
                }
            }
            return res;
        }
    }
}