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

            do
            {
                SolveRows(nonogram.HorizontalRows, nonogram, res);
                SolveRows(nonogram.VerticalRows, nonogram, res);
            } while (!nonogram.IsSolved && res.IterationsCount < 100);

            return res;
        }

        private void SolveRows(List<Row> rows, Nonogram nonogram, SolveStatistics res)
        {
            var watch = new Stopwatch();
//            Parallel.ForEach(rows, row =>
//            {
//                Debug.Write("Solving row:   Blocks: " + row.Blocks.Count() + ". Solved for: ");
//                watch.Restart();
//                rowSolver.SolveRow(row);
//                watch.Stop();
//                Debug.Write(watch.Elapsed);
//                Debug.WriteLine("");
//            });
            for (int index = 0; index < rows.Count; index++)
            {
                var row = rows[index];
                Debug.Write("Solving row: " + (index + 1)+". Blocks: " + row.Blocks.Count()+". Solved for: ");
                watch.Restart();
                rowSolver.SolveRow(row);
                watch.Stop();
                Debug.Write(watch.Elapsed);
                Debug.WriteLine("");
            }
            Debug.WriteLine(nonogram.ToString());
            res.IterationsCount++;
            if (rows.TrueForAll(x => x.State == RowState.Solved))
            {
                nonogram.SetState(NonogramState.Solved);
            }
        }
    }
}