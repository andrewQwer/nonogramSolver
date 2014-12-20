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
            var faRow = faRowBuilder.BuildFARow(row.Definition);
            var possibleCellsByColors = row.Blocks.Select(x => new Cell(x.Color)).Distinct(Cell.EqualityComparer);
            //list of possible cells combinations for the current row
            var possibleCells = ImmutableList<Cell>.Empty
                .AddRange(possibleCellsByColors)
                .Add(Cell.Delimeter);
            //declare the main graph for row solving
            var map = ImmutableDirectedGraph<EdgeCellData, Cell>.Empty;
            //the first point to draw graph (0 - cell idx, 1 - edge number)
            var startingData = new EdgeCellData(0, 1);
            //this variable will contain the last created edges on each iteration over the row
            var lastFilledEdges = ImmutableList<EdgeCellData>.Empty.Add(startingData);
            //edges created on each iteration
            var newEdges = new List<EdgeCellData>();
            for (int cIdx = 0; cIdx < row.Cells.Count; cIdx++)
            {
                var cell = row.Cells[cIdx];
                var curPossibleCells = cell.IsUndefined
                    ? possibleCells
                    : ImmutableList<Cell>.Empty.Add(cell);
                newEdges.Clear();
                //iterate over the last filled edges to determine the possible paths from these edges
                foreach (var lastFilledEdge in lastFilledEdges)
                {
                    var edge = faRow.GetEdgeByNumber(lastFilledEdge.EdgeNumber);
                    var edgeData = faRow.EdgeData(edge);
                    //possible moves from the edge accroding to possible current cell's states
                    var edgesToMove = edgeData.Where(x => curPossibleCells.Any(pc => x.Value(pc)));
                    //generate new edges for the graph
                    foreach (var etm in edgesToMove)
                    {
                        var endEdge = new EdgeCellData(cIdx + 1, etm.Key.Number);
                        newEdges.Add(endEdge);
                        map = map.AddEdge(lastFilledEdge, endEdge, curPossibleCells.First(c => etm.Value(c)));
                    }
                }
                lastFilledEdges = ImmutableList<EdgeCellData>.Empty.AddRange(newEdges);
            }
            //get all paths that ends with the last edge-cell combination
            var paths =
                (from path in map.AllEdgeTraversals(startingData, x => map.Edges(x))
                 let lastNode = path.Last().Value
                 where lastNode.Equals(new EdgeCellData(row.Cells.Count, faRow.LastEdgeNumber))
                 select path)
                 .ToList();
            //if there are no paths that lead to the last edge-cell combination then row can't be solved with the 
            //current definition
            if (!paths.Any())
            {
                row.State = RowState.NoSolution;
                return;
            }
            //group paths by cell index to determine possible cell states
            var groupedCells =
                (
                 from stack in paths
                 from cellEdgePair in stack
                 group cellEdgePair by cellEdgePair.Value.CellNumber into cellEdgePairGrouped
                 select cellEdgePairGrouped
                 ).ToDictionary(key => key.Key, key => new HashSet<Cell>(key.Select(x => x.Key), Cell.EqualityComparer));
            //cell is solved if only 1 possible state exists for this cell
            var solvedCells = groupedCells.Where(x => x.Value.Count == 1);
            foreach (var solvedCell in solvedCells)
            {
                var cellTemplate = solvedCell.Value.First();
                var cellId = solvedCell.Key - 1;
                if (cellTemplate.IsDelimeter)
                {
                    row.Cells[cellId].SetDelimeter();
                }
                else
                {
                    row.Cells[cellId].SetColor(cellTemplate.Color);
                }
            }
            //check whether all cells have been solved
            if (row.Cells.TrueForAll(x => x.HasColor || x.IsDelimeter))
            {
                row.State = RowState.Solved;
            }
        }
    }
}