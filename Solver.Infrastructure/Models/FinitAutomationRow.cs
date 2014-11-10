using System.Collections.Generic;

namespace Solver.Infrastructure.Models
{
    public class FinitAutomationRow
    {
        public List<FinitAutomationEdge> Edges { get; set; }
        public int LastEdgeNumber { get; private set; }

        public FinitAutomationRow()
        {
            Edges = new List<FinitAutomationEdge>();
            LastEdgeNumber = 0;
        }

        public FinitAutomationEdge CreateEdge()
        {
            var edge = new FinitAutomationEdge(++LastEdgeNumber);
            Edges.Add(edge);
            return edge;
        }
    }
}