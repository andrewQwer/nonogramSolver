using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Solver.Infrastructure.Models
{
    /// <summary>
    /// Represents definition for nonogram row
    /// </summary>
    public class RowDefinition
    {
        List<RowDefinitionItem> _blocks;

        public RowDefinition()
        {
            _blocks = new List<RowDefinitionItem>();
        }

        public IEnumerable<RowDefinitionItem> Blocks
        {
            get { return _blocks; }
        }

        public void AddItem(RowDefinitionItem item)
        {
            if (item.Length == 0 && _blocks.Any())
            {
                throw new ArgumentException("Row already contains Blocks. Zero length item is not allowed.", "item");
            }
            if (_blocks.Any(x => x.Length == 0))
            {
                throw new ArgumentException("Zero length item  has already been added. Can't add new item.", "item");
            }
            if (item.Length < 0)
            {
                throw new ArgumentException("Length can't be less than 0. Can't add new item.", "item");
            }
            _blocks.Add(item);
        }

        public void AddItem(int length)
        {
            _blocks.Add(new RowDefinitionItem(length));
        }
    }
}