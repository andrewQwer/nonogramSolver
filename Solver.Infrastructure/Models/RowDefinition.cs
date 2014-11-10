using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Solver.Infrastructure.Models
{
    public class RowDefinition
    {
        List<RowDefinitionItem> items;

        public RowDefinition()
        {
            items = new List<RowDefinitionItem>();
        }

        public IEnumerable<RowDefinitionItem> Items
        {
            get { return items; }
        }

        public void AddItem(RowDefinitionItem item)
        {
            if (item.Length == 0 && items.Any())
            {
                throw new ArgumentException("Row already contains items. Zero length item is not allowed.", "item");
            }
            if (items.Any(x => x.Length == 0))
            {
                throw new ArgumentException("Zero length item  has already been added. Can't add new item.", "item");
            }
            if (item.Length < 0)
            {
                throw new ArgumentException("Length can't be less than 0. Can't add new item.", "item");
            }
            items.Add(item);
        }

        public void AddItem(int length)
        {
            items.Add(new RowDefinitionItem(length));
        }
    }
}