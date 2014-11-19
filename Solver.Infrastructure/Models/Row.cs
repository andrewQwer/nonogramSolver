using System;
using System.Collections.Generic;
using System.Linq;

namespace Solver.Infrastructure.Models
{
    public enum RowState
    {
        Undefined,
        Solved
    }

    public class Row
    {
        public RowDefinition Definition { get; private set; }
        public List<Cell> Cells { get; private set; }
        public RowState State { get; set; }

        public Row(RowDefinition definition, int cellsCount)
        {
            ValidateData(definition, cellsCount);
            Cells = new List<Cell>(Enumerable.Repeat(new Cell(), cellsCount));
            Definition = definition;
            State = RowState.Undefined;
        }

        private static void ValidateData(RowDefinition definition, int cellsCount)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");
            if (definition.IsEmpty)
                throw new ArgumentException("definition is empty", "definition");
            if (cellsCount <= 0)
                throw new ArgumentException("Incorrect cells count");
            RowDefinitionItem previous = null;
            var minimumCellsCount = definition.Blocks.Aggregate(0, (sum, x) =>
            {
                sum += x.Length;
                if (previous!=null && x.Color == previous.Color)
                    sum += 1;
                previous = x;
                return sum;
            });
            if (minimumCellsCount > cellsCount)
            {
                throw new ArgumentException("Row length is smaller than minimal required", "cellsCount");
            }
        }

        /// <summary>
        /// Shortcut for <see cref="Row.Definition.Blocks"/>
        /// </summary>
        public IEnumerable<RowDefinitionItem> Blocks
        {
            get { return Definition.Blocks; }
        }
    }
}