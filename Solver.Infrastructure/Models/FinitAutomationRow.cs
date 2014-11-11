using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver.Infrastructure.Models
{
    /// <summary>
    /// Finit Automation row. Contains relations info between edges.
    /// </summary>
    public class FinitAutomationRow
    {
        private readonly Dictionary<FinitAutomationEdge, Dictionary<FinitAutomationEdge, Func<Cell, bool>>> edges;

        public int LastEdgeNumber { get; private set; }

        public FinitAutomationRow()
        {
            edges = new Dictionary<FinitAutomationEdge, Dictionary<FinitAutomationEdge, Func<Cell, bool>>>();
            LastEdgeNumber = 0;
        }

        /// <summary>
        /// Inidcates whether edges exist
        /// </summary>
        public bool IsEmpty
        {
            get { return LastEdgeNumber == 0; }
        }

        public IEnumerable<FinitAutomationEdge> Edges
        {
            get { return edges.Select(x => x.Key); }
        }

        public FinitAutomationEdge FirstEdge
        {
            get
            {
                return edges.Any()
                    ? edges.First().Key
                    : null;
            }
        }
        public FinitAutomationEdge LastEdge
        {
            get
            {
                return edges.Any()
                    ? edges.Last().Key
                    : null;
            }
        }

        public IEnumerable<FinitAutomationEdge> EdgesRelations(FinitAutomationEdge edge)
        {
            return edges.ContainsKey(edge)
                ? edges[edge].Select(x => x.Key)
                : Enumerable.Empty<FinitAutomationEdge>();
        }

        public IEnumerable<Func<Cell, bool>> ConditionsRelations(FinitAutomationEdge edge)
        {
            return edges.ContainsKey(edge)
                ? edges[edge].Select(x => x.Value)
                : Enumerable.Empty<Func<Cell, bool>>();
        }

        /// <summary>
        /// Creates relations between two edges
        /// </summary>
        /// <param name="from">Source edge</param>
        /// <param name="to">Destination edge</param>
        /// <param name="func">Condition to move to the destination edge</param>
        /// <returns></returns>
        public FinitAutomationEdge AddPath(FinitAutomationEdge from, FinitAutomationEdge to, Func<Cell, bool> func)
        {
            if (edges.ContainsKey(from))
            {
                edges[from].Add(to, func);
                return to;
            }
            return null;
        }

        /// <summary>
        /// Creates relation loop for the edge
        /// </summary>
        /// <param name="loop">Source edge</param>
        /// <param name="func">Condition to move to the same edge</param>
        /// <returns></returns>
        public FinitAutomationEdge AddPath(FinitAutomationEdge loop, Func<Cell, bool> func)
        {
            if (edges.ContainsKey(loop))
            {
                edges[loop].Add(loop, func);
                return loop;
            }
            return null;
        }

        /// <summary>
        /// Creates edge and assigns serial number to it
        /// </summary>
        /// <returns></returns>
        public FinitAutomationEdge CreateEdge()
        {
            var edge = new FinitAutomationEdge(++LastEdgeNumber);
            edges.Add(edge, new Dictionary<FinitAutomationEdge, Func<Cell, bool>>());
            return edge;
        }
    }
}