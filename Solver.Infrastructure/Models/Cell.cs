using System.Drawing;

namespace Solver.Infrastructure.Models
{
    public enum CellState
    {
        Undefined,
        Delimeter,
        Solved
    }

    public class Cell
    {
        public static readonly Color DefaultColor = Color.Black;
        private Color _color;
        public CellState State { get; set; }

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                State = CellState.Solved;
            }
        }

        public Cell()
        {
            State = CellState.Undefined;
        }

        public bool IsDelimeter
        {
            get { return State == CellState.Delimeter; }
        }

        public bool IsSolved
        {
            get { return State == CellState.Solved; }
        }

        public bool IsUndefined
        {
            get { return State == CellState.Undefined; }
        }
    }
}