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

        public CellState State { get; private set; }

        public Color Color { get; private set; }

        public Cell()
        {
            State = CellState.Undefined;
        }

        public Cell(Color color)
            : this()
        {
            SetColor(color);
        }

        public bool IsDelimeter
        {
            get { return State == CellState.Delimeter; }
        }

        public bool HasColor
        {
            get { return Color != Color.Empty; }
        }

        public bool IsUndefined
        {
            get { return State == CellState.Undefined; }
        }

        public static Cell Delimeter
        {
            get
            {
                var c = new Cell();
                c.SetDelimeter();
                return c;
            }
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

        public void SetColor(Color color)
        {
            Color = color;
            if (Color != Color.Empty)
                State = CellState.Colored;
        }
    }
}