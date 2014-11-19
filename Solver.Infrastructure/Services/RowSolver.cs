using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
            var map = ImmutableDirectedGraph<EdgeCellData, Func<Cell, bool>>.Empty;
            var currentEdge = 1;
            var startingData = new EdgeCellData(-1, currentEdge);
            var lastFilledEdges = ImmutableList<EdgeCellData>.Empty.Add(startingData);
            for (int cIdx = 0; cIdx < row.Cells.Count; cIdx++)
            {
                var cell = row.Cells[cIdx];
                var newEdges = new List<EdgeCellData>();
                foreach (var lastFilledEdge in lastFilledEdges)
                {
                    var edge = faRow.Edges.ElementAt(lastFilledEdge.EdgeNumber - 1);
                    var edgeData = faRow.EdgeData(edge);
                    var edgesToMove = edgeData.Where(x => cell.IsUndefined || x.Value(cell)).ToList();

                    edgesToMove.ForEach(x =>
                    {
                        var endEdge = new EdgeCellData(cIdx, x.Key.Number);
                        newEdges.Add(endEdge);
                        map = map.AddEdge(lastFilledEdge, endEdge, x.Value);
                    });
                }
                lastFilledEdges = ImmutableList<EdgeCellData>.Empty.AddRange(newEdges);
            }
            foreach (var path in map.AllEdgeTraversals(new EdgeCellData(row.Cells.Count-1, faRow.LastEdgeNumber),x=> map.Edges(x)))
            {
                Console.WriteLine(string.Join(" ", from pair in path select pair.Key));
            }
            //check whether all cells have been solved
            if (row.Cells.TrueForAll(x => x.IsSolved))
            {
                row.State = RowState.Solved;
            }
        }
    }
}