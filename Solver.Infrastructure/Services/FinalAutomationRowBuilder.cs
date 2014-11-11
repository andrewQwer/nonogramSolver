using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solver.Infrastructure.Models;

namespace Solver.Infrastructure.Services
{
    public interface IFinitAutomationRowBuilder
    {
        FinitAutomationRow BuildFARow(RowDefinition def);
    }

    public class FinitAutomationRowBuilder : IFinitAutomationRowBuilder
    {
        public FinitAutomationRow BuildFARow(RowDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition", "Value can't be null");
            }
            if (!definition.Blocks.Any())
            {
                throw new ArgumentException( "Value can't be empty", "definition");
            }

            var row = new FinitAutomationRow();
            //zero row definition should return empty row
            if (definition.Blocks.All(x => x.Length == 0))
            {
                return row;
            }
            var edge = row.CreateEdge();
            RowDefinitionItem previousDef = null;
            foreach (var currentDef in definition.Blocks)
            {
                var color = currentDef.Color;
                //there should be a delimeter between blocks of the same color
                if (previousDef != null && previousDef.Color == color)
                {
                    edge = row.AddPath(edge, row.CreateEdge(), x => x.IsDelimeter);
                }
                //if cell type is delimeter edge moves to istelf after every block in the row
                row.AddPath(edge, x => x.IsDelimeter);
                for (var i = 0; i < currentDef.Length; i++)
                {
                    edge = row.AddPath(edge, row.CreateEdge(), x => x.Color == color);
                }
                previousDef = currentDef;
            }
            //if cell type is delimeter the last edge moves to istelf
            row.AddPath(edge, x => x.IsDelimeter);
            return row;
        }
    }
}
