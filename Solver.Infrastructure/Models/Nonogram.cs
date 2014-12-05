using System.Collections.Generic;

namespace Solver.Infrastructure.Models
{
    public class Nonogram
    {
        public List<Row> VerticalRows { get; set; }
        public List<Row> HorizontalRows { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Nonogram(int width, int height)
        {
            VerticalRows = new List<Row>(width);
            HorizontalRows = new List<Row>(height);
            Width = width;
            Height = height;
        }

        public void AddVerticalRow(RowDefinition rowDef)
        {
            VerticalRows.Add(new Row(rowDef, Height));
        }

        public void AddHorizontalRow(RowDefinition rowDef)
        {
            HorizontalRows.Add(new Row(rowDef, Width));
        }

        private void NormalizeRows()
        {
            for (int hIdx = 0; hIdx < HorizontalRows.Count; hIdx++)
            {
                var horizontalRow = HorizontalRows[hIdx];
                for (int vIdx = 0; vIdx < VerticalRows.Count; vIdx++)
                {
                    var verticalRow = VerticalRows[vIdx];
                    verticalRow.Cells[vIdx] = horizontalRow.Cells[hIdx];
                }
            }
        }
    }
}