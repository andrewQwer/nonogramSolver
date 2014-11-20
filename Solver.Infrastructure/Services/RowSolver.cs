using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Solver.Infrastructure.Models;

namespace Solver.Infrastructure.Services
{
    public interface IRowSolver
    {
        void SolveRow(Row row);
    }

    public class RowSolver : IRowSolver
    {
        private IFinitAutomationRowBuilder faRowBuilder;

        public RowSolver(IFinitAutomationRowBuilder faRowBuilder)
        {
            this.faRowBuilder = faRowBuilder;
        }

        public void SolveRow(Row row)
        {
            if (row == null)
                throw new ArgumentNullException("row");
            if (row.State == RowState.Solved)
                return;
            var cells = row.Cells;
            //solve row if definition fills full row with 1 item
            if (row.Blocks.Count() == 1 && row.Blocks.First().Length == row.Cells.Count)
            {
                cells.ForEach(x => x.Color = row.Blocks.First().Color);
            }
            var faRow = faRowBuilder.BuildFARow(row.Definition);
            var rowColors = row.Blocks.Select(x => x.Color);
            var possibleCellsByColors = rowColors.Select(x => new Cell(x));
            var delimeterCell = new Cell
            {
                State = CellState.Delimeter
            };
            var possibleCells = new List<Cell>(possibleCellsByColors);
            possibleCells.Add(delimeterCell);
            var map = ImmutableDirectedGraph<EdgeCellData, Cell>.Empty;
            var startingData = new EdgeCellData(0, 1);
            var lastFilledEdges = ImmutableList<EdgeCellData>.Empty.Add(startingData);
            lastFilledEdges.Clear();
            for (int cIdx = 0; cIdx < row.Cells.Count; cIdx++)
            {
                var cell = row.Cells[cIdx];
                var cellToCheck = cell.IsUndefined
                    ? possibleCells
                    : new List<Cell>
                    {
                        cell
                    };
                var newEdges = new List<EdgeCellData>();
                foreach (var lastFilledEdge in lastFilledEdges)
                {
                    var edge = faRow.GetEdgeByNumber(lastFilledEdge.EdgeNumber);
                    var edgeData = faRow.EdgeData(edge);
                    var edgesToMove = edgeData.Where(x => cellToCheck.Any(pc => x.Value(pc)));

                    foreach (var etm in edgesToMove)
                    {
                        var endEdge = new EdgeCellData(cIdx + 1, etm.Key.Number);
                        newEdges.Add(endEdge);
                        map = map.AddEdge(lastFilledEdge, endEdge, cellToCheck.First(c => etm.Value(c)));
                    }
                }
                lastFilledEdges = ImmutableList<EdgeCellData>.Empty.AddRange(newEdges);
            }
            var pathes = new List<ImmutableStack<KeyValuePair<Cell, EdgeCellData>>>();
            foreach (var path in map.AllEdgeTraversals(startingData, x => map.Edges(x)))
            {
                var lastNode = path.Last().Value;
                if (lastNode.Equals(new EdgeCellData(row.Cells.Count, faRow.LastEdgeNumber)))
                {
                    pathes.Add(path);
                }
            }
            var groupedCellss =
                (from stack in pathes
                 from i in stack
                 group i by i.Value.CellNumber into iGrouped
                 select iGrouped)
                 .ToDictionary(key => key.Key, key => new HashSet<Cell>(key.Select(x => x.Key)));
            var solvedCells = groupedCellss.Where(x => x.Value.Count == 1);
            foreach (var solvedCell in solvedCells)
            {
                var cellTemplate = solvedCell.Value.First();
                row.Cells[solvedCell.Key - 1].Color = cellTemplate.Color;
                row.Cells[solvedCell.Key - 1].State = cellTemplate.IsDelimeter
                ? CellState.Delimeter
                : CellState.Colored;
            }
            //check whether all cells have been solved
            if (row.Cells.TrueForAll(x => x.IsSolved))
            {
                row.State = RowState.Solved;
            }
        }
    }
}