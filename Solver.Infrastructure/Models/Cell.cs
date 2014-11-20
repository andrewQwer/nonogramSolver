using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Solver.Infrastructure.Models
{
    public enum CellState
    {
        Undefined,
        Delimeter,
        Colored
    }

    [DebuggerDisplay("Color = {Color}, State = {State}")]
    public class Cell : IEqualityComparer<Cell>
    {
        public static readonly Color DefaultColor = Color.Black;

        public CellState State { get; set; }

        public Color Color { get; set; }

        public Cell()
        {
            State = CellState.Undefined;
        }

        public Cell(Color color)
            : this()
        {
            Color = color;
        }

        public bool IsDelimeter
        {
            get { return State == CellState.Delimeter; }
        }

        public bool IsSolved
        {
            get { return State == CellState.Colored; }
        }

        public bool IsUndefined
        {
            get { return State == CellState.Undefined; }
        }

        public Cell Clone()
        {
            return new Cell
            {
                Color = this.Color,
                State = this.State
            };
        }

        public void SetDelimeter()
        {
            State = CellState.Delimeter;
            Color = Color.Empty;
        }

        public bool Equals(Cell x, Cell y)
        {
            return x.Color == y.Color && x.State == y.State;
        }

        public int GetHashCode(Cell obj)
        {
            return obj.Color.GetHashCode() ^ obj.State.GetHashCode();
        }

        public void CopyFrom(Cell c)
        {
            this.Color = c.Color;
            this.State = c.State;
        }
    }
}