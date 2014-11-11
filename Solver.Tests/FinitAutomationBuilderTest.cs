using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Solver.Infrastructure.Models;
using Solver.Infrastructure.Services;

namespace Solver.Tests
{
    [TestFixture]
    public class FinitAutomationBuilderTest
    {
        private IFinitAutomationRowBuilder rowBuilder;

        public FinitAutomationBuilderTest()
        {
            this.rowBuilder = new FinitAutomationRowBuilder();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FAB_generates_exception_for_NULL_row_definition()
        {
            rowBuilder.BuildFARow(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FAB_generates_exception_for_empty_row_definition()
        {
            rowBuilder.BuildFARow(new RowDefinition());
        }

        [Test]
        public void FAB_generates_empty_FA_row_for_zero_definition_item()
        {
            var def = new RowDefinition();
            def.AddItem(new RowDefinitionItem(0));
            var row = rowBuilder.BuildFARow(def);
            Assert.IsEmpty(row.Edges);
            Assert.AreEqual(0, row.LastEdgeNumber);
        }

        [Test]
        public void FAB_generates_valid_row_for_1_item()
        {
            var itemLength = 5;
            var def = new RowDefinition();
            def.AddItem(itemLength);
            var row = rowBuilder.BuildFARow(def);
            Assert.AreEqual(itemLength + 1, row.LastEdgeNumber);
            Assert.AreEqual(row.EdgesRelations(row.FirstEdge).First(), row.FirstEdge);
            Assert.AreEqual(row.EdgesRelations(row.LastEdge).First(), row.LastEdge);
            var cell = new Cell
            {
                IsDelimeter = true
            };
            Assert.True(row.ConditionsRelations(row.FirstEdge).Any(x => x(cell)));
            Assert.True(row.ConditionsRelations(row.LastEdge).Any(x => x(cell)));
            cell = new Cell
            {
                Color = Color.Black
            };
            Assert.True(row.ConditionsRelations(row.FirstEdge).Any(x => x(cell)));
            foreach (var edge in row.Edges.Except(new []{row.LastEdge, row.FirstEdge}))
            {
                Assert.True(row.ConditionsRelations(edge).All(x => x(cell)));
            }
        }

        [Test]
        public void FAB_generates_valid_row_for_2_items()
        {
            var rnd = new Random();
            var blocksToGenerate = rnd.Next(2, 10);
            var blockLengths = new List<int>();
            var def = new RowDefinition();
            for (var i = 0; i < blocksToGenerate; i++)
            {
                var blockLength = rnd.Next(1, 30);
                blockLengths.Add(blockLength);
                def.AddItem(blockLength);
            }
            Console.WriteLine("Generated blocks: " + string.Join(":", blockLengths.Select(x=>x.ToString())));
            var row = rowBuilder.BuildFARow(def);
            Console.WriteLine("Edges count: " + row.Edges.Count());

            Assert.AreEqual(blockLengths.Sum(x => x) + blockLengths.Count, row.LastEdgeNumber);
            Assert.AreEqual(row.EdgesRelations(row.FirstEdge).First(), row.FirstEdge);
            Assert.AreEqual(row.EdgesRelations(row.LastEdge).First(), row.LastEdge);

            var cellDelimeter = new Cell
            {
                IsDelimeter = true
            };
            var coloredCell = new Cell
            {
                Color = Color.Black
            };
            Assert.True(row.ConditionsRelations(row.FirstEdge).Any(x => x(cellDelimeter)));
            Assert.True(row.ConditionsRelations(row.LastEdge).Any(x => x(cellDelimeter)));
            Assert.True(row.ConditionsRelations(row.FirstEdge).Any(x => x(coloredCell)));
            for (int idx = 0; idx < blockLengths.Count-1; idx++)
            {
                var block = blockLengths[idx];
                var previousBlocksTotallength = blockLengths.GetRange(0, idx).Sum(x => x);
                var edgeIdx = block + previousBlocksTotallength + idx;
                Assert.True(row.ConditionsRelations(row.Edges.ElementAt(edgeIdx)).All(x => x(cellDelimeter)));
                Assert.True(row.ConditionsRelations(row.Edges.ElementAt(edgeIdx + 1)).Any(x => x(cellDelimeter)));
                Assert.True(row.ConditionsRelations(row.Edges.ElementAt(edgeIdx + 1)).Any(x => x(coloredCell)));
            }
        }
    }
}
