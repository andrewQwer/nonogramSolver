using System;
using System.Collections.Generic;

namespace Solver.Infrastructure.Models
{
    public class Row
    {
        public RowDefinition Definition { get; private set; }
        public List<Cell> Cells { get; set; }

        public Row(RowDefinition definition, int cellsCount)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");
            if (definition.IsEmpty)
                throw new ArgumentException("definition is empty", "definition");
            if (cellsCount <= 0)
                throw new ArgumentException("Incorrect cells count");
            Cells = new List<Cell>(cellsCount);
            Definition = definition;
        }
    }
}