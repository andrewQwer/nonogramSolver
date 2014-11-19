using System;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using Solver.Infrastructure.DI;
using Solver.Infrastructure.Models;
using Solver.Infrastructure.Services;

namespace Solver.Tests
{
    [TestFixture]
    public class RowTest
    {
        [Test]
        public void Cant_create_row_without_definition()
        {
            Assert.Throws<ArgumentNullException>(() => new Row(null, 5));
        }

        [Test]
        public void Cant_create_row_with_empty_definition()
        {
            Assert.Throws<ArgumentException>(() => new Row(new RowDefinition(), 5));

        }

        [Test]
        public void Cant_create_row_without_cells()
        {
            var def = new RowDefinition();
            def.AddItem(3);
            Assert.Throws<ArgumentException>(() => new Row(def, 0));
        }

        [Test]
        public void Cant_create_row_with_negative_cells_count()
        {
            var def = new RowDefinition();
            def.AddItem(3);
            Assert.Throws<ArgumentException>(() => new Row(def, -3));
        }

        [Test]
        public void Can_create_row()
        {
            var def = new RowDefinition();
            def.AddItem(3);
            var row = new Row(def, 3);
            Assert.True(row.Cells.TrueForAll(x => x.State == CellState.Undefined));
        }
        
        [Test]
        public void Cant_create_row_if_definition_doesnt_match_row_length()
        {
            var def = new RowDefinition();
            def.AddItem(3);
            Assert.Throws<ArgumentException>(() => new Row(def, 2));
            Assert.DoesNotThrow(() => new Row(def, 3));

            def = new RowDefinition();
            def.AddItem(3);
            def.AddItem(2);
            Assert.Throws<ArgumentException>(() => new Row(def, 5));
            Assert.DoesNotThrow(() => new Row(def, 6));
            
            def = new RowDefinition();
            def.AddItem(new RowDefinitionItem(3, Color.Red));
            def.AddItem(new RowDefinitionItem(2, Color.Yellow));
            Assert.Throws<ArgumentException>(() => new Row(def, 4));
            Assert.DoesNotThrow(() => new Row(def, 5));
        }
    }
}