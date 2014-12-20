using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Solver.Infrastructure.Models
{
    public enum NonogramState
    {
        Undefined,
        NoSolution,
        Solved,
    }

    public class Nonogram
    {
        public List<Row> VerticalRows { get; private set; }
        public List<Row> HorizontalRows { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public NonogramState State { get; private set; }

        private bool VerticalRowsFilled
        {
            get { return VerticalRows.Count == Width; }
        }

        private bool HorizontalRowsFilled
        {
            get { return HorizontalRows.Count == Height; }
        }

        public bool IsSolved
        {
            get { return State == NonogramState.Solved; }
        }

        public Nonogram(int width, int height)
        {
            VerticalRows = new List<Row>(width);
            HorizontalRows = new List<Row>(height);
            Width = width;
            Height = height;
            State = NonogramState.Undefined;
        }

        public void AddVerticalRow(RowDefinition rowDef)
        {
            if (VerticalRowsFilled)
                throw new Exception("Vertical rows have already been filled");

            VerticalRows.Add(new Row(rowDef, Height));
            NormalizeRows();
        }

        public void AddHorizontalRow(RowDefinition rowDef)
        {
            if (HorizontalRowsFilled)
                throw new Exception("Horizontal rows have already been filled");

            HorizontalRows.Add(new Row(rowDef, Width));
            NormalizeRows();
        }

        private void NormalizeRows()
        {
            if (VerticalRowsFilled && HorizontalRowsFilled)
            {
                for (int hIdx = 0; hIdx < HorizontalRows.Count; hIdx++)
                {
                    var hr = HorizontalRows[hIdx];
                    for (int vIdx = 0; vIdx < VerticalRows.Count; vIdx++)
                    {
                        var vr = VerticalRows[vIdx];
                        vr.Cells[hIdx] = hr.Cells[vIdx];
                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var horizontalRow in HorizontalRows)
            {
                foreach (var cell in horizontalRow.Cells)
                {
                    if (cell.HasColor)
                    {
                        sb.Append('*');
                    }
                    else if (cell.IsDelimeter)
                    {
                        sb.Append('·');
                    }
                    else
                    {
                        sb.Append('_');
                    }
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public void SetState(NonogramState state)
        {
            State = state;
        }
    }
}