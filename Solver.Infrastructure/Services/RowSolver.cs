using Solver.Infrastructure.Models;

namespace Solver.Infrastructure.Services
{
    public interface IRowSolver
    {
        void SolveRow(Row row);
    }

    public class RowSolver : IRowSolver
    {

        public void SolveRow(Row row)
        {
            throw new System.NotImplementedException();
        }
    }
}