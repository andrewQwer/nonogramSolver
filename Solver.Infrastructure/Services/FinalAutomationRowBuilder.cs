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
            if (!definition.Items.Any())
            {
                throw new ArgumentException( "Value can't be empty", "definition");
            }

            var row = new FinitAutomationRow();
            if (definition.Items.All(x => x.Length == 0))
            {
                return row;
            }
            var edge = row.CreateEdge();
            edge.AddChild(edge, x => x.IsDelimeter);
            return new FinitAutomationRow();
        }
    }
}
