using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solver.Infrastructure.Models
{
    /// <summary>
    /// Finit Automation edge
    /// </summary>
    [DebuggerDisplay("[Number = {Number}]")]
    public class  FinitAutomationEdge
    {
        public int Number { get; set; }

        public override string ToString()
        {
            return Number.ToString();
        }

        public FinitAutomationEdge(int number)
        {
            Number = number;
        }
    }
}