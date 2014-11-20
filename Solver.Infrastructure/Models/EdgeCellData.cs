using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solver.Infrastructure.Models
{
    [DebuggerDisplay("Cell = {CellNumber}, Edge = {EdgeNumber}")]
    public struct EdgeCellData :IEqualityComparer<EdgeCellData>
    {
        public int CellNumber { get; set; }
        public int EdgeNumber { get; set; }

        public EdgeCellData(int cellNum, int edgeNum) : this()
        {
            CellNumber = cellNum;
            EdgeNumber = edgeNum;
        }

        public override int GetHashCode()
        {
            int hash = CellNumber;
            hash = hash * 31 + EdgeNumber;
            return hash.GetHashCode();
        }

        public bool Equals(EdgeCellData x, EdgeCellData y)
        {
            return x.CellNumber == y.CellNumber && x.EdgeNumber == y.EdgeNumber;
        }

        public int GetHashCode(EdgeCellData obj)
        {
            return obj.GetHashCode();
        }
    }
}