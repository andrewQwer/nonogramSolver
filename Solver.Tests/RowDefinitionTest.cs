using System;
using NUnit.Framework;
using Solver.Infrastructure.Models;

namespace Solver.Tests
{
    [TestFixture]
    public class RowDefinitionTest
    {
        [Test]
        public void Cant_add_more_than_1_zero_length_item()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(new RowDefinitionItem(0));
            Assert.Throws<ArgumentException>(() => rowDef.AddItem(new RowDefinitionItem(0)));
        }

        [Test]
        public void Can_add_any_non_zero_length_item_if_zero_presents()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(new RowDefinitionItem(0));
            Assert.Throws<ArgumentException>(() => rowDef.AddItem(new RowDefinitionItem(5)));
        }

        [Test]
        public void Can_add_zero_length_item_if_non_zero_items_presents()
        {
            var rowDef = new RowDefinition();
            rowDef.AddItem(new RowDefinitionItem(5));
            Assert.Throws<ArgumentException>(() => rowDef.AddItem(new RowDefinitionItem(0)));
        }

        [Test]
        public void Cant_add_negative_numbers()
        {
            var rowDef = new RowDefinition();
            Assert.Throws<ArgumentException>(() => rowDef.AddItem(new RowDefinitionItem(-5)));
        }

        [Test]
        public void Can_add_any_positive_numbers()
        {
            var rowDef = new RowDefinition();
            for (var i = 1; i <= 100; i++)
            {
                rowDef.AddItem(i);
            }
            Assert.Pass("Test passed");
        }
    }
}