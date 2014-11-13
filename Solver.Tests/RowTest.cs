using System;
using NUnit.Framework;
using Solver.Infrastructure.Models;
using Solver.Infrastructure.Services;

namespace Solver.Tests
{
    [TestFixture]
    public class RowTest
    {
        private IRowSolver solver;
        public RowTest()
        {
            solver = new RowSolver();
        }

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
            Assert.DoesNotThrow(() => new Row(def, 3));
        }

    }
}