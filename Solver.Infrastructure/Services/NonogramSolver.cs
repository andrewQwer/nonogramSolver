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
            var iterationsCount = 0;
            while (!nonogram.IsSolved && iterationsCount < 100)
            {
                Parallel.ForEach(nonogram.HorizontalRows, rowSolver.SolveRow);
                Debug.WriteLine(nonogram.ToString());
                iterationsCount++;

                Parallel.ForEach(nonogram.VerticalRows, rowSolver.SolveRow);
                Debug.WriteLine(nonogram.ToString());
                iterationsCount++;
                if (nonogram.HorizontalRows.TrueForAll(x => x.State == RowState.Solved))
                {
                    nonogram.SetState(NonogramState.Solved);
                }
            }
            res.IterationsCount = iterationsCount;
            return res;
        }
    }
}