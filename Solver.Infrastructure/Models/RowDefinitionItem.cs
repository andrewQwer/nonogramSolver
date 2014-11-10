using System.Drawing;

namespace Solver.Infrastructure.Models
{
    public struct RowDefinitionItem
    {
        public int Length { get; set; }
        public Color Color { get; set; }

        /// <summary>
        /// Initializes item with specific length and Black color
        /// </summary>
        /// <param name="length"></param>
        public RowDefinitionItem(int length) : this()
        {
            Length = length;
            Color = Color.Black;
        }
    }
}