using System;
using System.Collections.Generic;

namespace Solver.Infrastructure.Models
{
    public struct EdgeCellData :IEqualityComparer<EdgeCellData>
    {
        public int CellId { get; set; }
        public int EdgeNumber { get; set; }

        public EdgeCellData(int cellId, int edgeNumber) : this()
        {
            CellId = cellId;
            EdgeNumber = edgeNumber;
        }

        public override int GetHashCode()
        {
            int hash = CellId;
            hash = hash * 31 + EdgeNumber;
            return hash.GetHashCode();
        }

        public bool Equals(EdgeCellData x, EdgeCellData y)
        {
            return x.CellId == y.CellId && x.EdgeNumber == y.EdgeNumber;
        }

        public int GetHashCode(EdgeCellData obj)
        {
            return obj.GetHashCode();
        }
    }
}