using System.Diagnostics;
using System.Drawing;

namespace Solver.Infrastructure.Models
{
    /// <summary>
    /// Represents a single block of the nonogram row
    /// </summary>
    public class RowDefinitionItem
    {
        public int Length { get; set; }
        public Color Color { get; set; }

        /// <summary>
        /// Initializes item with specific length and Black color
        /// </summary>
        /// <param name="length"></param>
        public RowDefinitionItem(int length)
        {
            Length = length;
            Color = Cell.DefaultColor;
        }
    }
}