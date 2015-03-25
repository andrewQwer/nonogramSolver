using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
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
            var possibleCells = new List<Cell>(possibleCellsByColors.Count() + 1);
            possibleCells.AddRange(possibleCellsByColors);
            possibleCells.Add(Cell.Delimeter);
            var route = new Route();
            BuildRoute(row.Cells, faRow, possibleCells, 0, 1, route);
            ClearDeadRoutes(route, faRow, row.Cells.Count);
            if (!route.ChildRoutes.Any())
            {
                row.State = RowState.NoSolution;
                return;
            }
            var routes = GetAllRoutes(route);
            var groupedRoutes = from r in routes
                                group r by r.CellIdx into rGrouped
                                select rGrouped;

            foreach (var r in groupedRoutes)
            {
                var cells = r.Select(x => x.Cell).ToList();
                var first = cells[0];
                if (cells.Any(x => x != first))
                    continue;
                if (first.IsDelimeter)
                    row.Cells[r.Key].SetDelimeter();
                else
                    row.Cells[r.Key].SetColor(first.Color);

            }
            //check whether all cells have been solved
            if (row.Cells.Any(x => x.HasColor || x.IsDelimeter))
            {
                row.State = RowState.Solved;
            }
        }

        private List<Route> GetAllRoutes(Route route)
        {
            var list = new List<Route>(route.ChildRoutes);
            foreach (var r in route.ChildRoutes)
            {
                list.AddRange(GetAllRoutes(r));
            }
            return list;
        }

        private void ClearDeadRoutes(Route route, FinitAutomationRow faRow, int count)
        {
            //if route's children are not empty we should invoke this function for every route's children
            if (route.ChildRoutes.Any())
            {
                var routesToProcess = new List<Route>(route.ChildRoutes);
                foreach (var nextRoute in routesToProcess)
                {
                    ClearDeadRoutes(nextRoute, faRow, count);
                }
            }
            else
            {
                //for the last child we check edge number. 
                //If number is not equal to the DA's row  last edge number - remove this route children
                if (route.EdgeIdx != faRow.LastEdgeNumber || route.CellIdx != count-1)
                {
                    RemoveRouteInParent(route);
                }
            }
        }

        private void RemoveRouteInParent(Route route)
        {
            var parent = route.ParentRoute;
            if (parent != null)
            {
                parent.ChildRoutes.Remove(route);
                if (!parent.ChildRoutes.Any())
                {
                    RemoveRouteInParent(parent);
                }
            }
        }

        private void BuildRoute(List<Cell> cells, FinitAutomationRow faRow, List<Cell> possibleCells, int cIdx, int edgeNum, Route route)
        {
            if (cIdx >= cells.Count)
                return;
            var cell = cells[cIdx];
            var curPossibleCells = cell.IsUndefined
                ? possibleCells
                : new List<Cell> { cell };
            var edge = faRow.GetEdgeByNumber(edgeNum);
            var edgeData = faRow.EdgeData(edge);
            //possible moves from the edge accroding to possible current cell's states
            var edgesToMove = edgeData.Where(x => curPossibleCells.Any(pc => x.Value(pc)));
            var nextIdx = cIdx + 1;
            foreach (var kvp in edgesToMove)
            {
                var cellTemplate = curPossibleCells.First(c => kvp.Value(c));
                var child = new Route(cIdx, kvp.Key.Number);
                child.Cell = cellTemplate;
                child.ParentRoute = route;
                route.ChildRoutes.Add(child);
                BuildRoute(cells, faRow, possibleCells, nextIdx, kvp.Key.Number, child);
            }
        }
    }

    public class Route
    {
        public int CellIdx { get;set; }
        public int EdgeIdx { get; set; }

        public Route()
        {
            ChildRoutes = new List<Route>();
        }

        public Route(int cellIdx, int edgeIdx)
            : this()
        {
            CellIdx = cellIdx;
            EdgeIdx = edgeIdx;
        }

        public List<Route> ChildRoutes { get; set; }
        public Route ParentRoute { get; set; }
        public Cell Cell { get; set; }
    }
}