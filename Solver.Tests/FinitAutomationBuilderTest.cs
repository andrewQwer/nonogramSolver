using System;
using NUnit.Framework;
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
            Assert.AreEqual(itemLength + 2, row.LastEdgeNumber);
        }
    }
}
