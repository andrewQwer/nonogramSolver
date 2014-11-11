using System;
using System.Collections.Generic;

namespace Solver.Infrastructure.Models
{
    public class  FinitAutomationEdge
    {
        public Dictionary<FinitAutomationEdge, Func<Cell, bool>> EdgesToMove { get; set; }
        public int Number { get; set; }

        public void AddPath(FinitAutomationEdge edge, Func<Cell, bool> func)
        {
            EdgesToMove.Add(edge, func);
        }

        public override string ToString()
        {
            return Number.ToString();
        }

        public FinitAutomationEdge(int number)
        {
            EdgesToMove = new Dictionary<FinitAutomationEdge, Func<Cell, bool>>();
            Number = number;
        }
    }
}