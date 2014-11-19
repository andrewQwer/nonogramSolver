using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Solver.Infrastructure.Models
{
    public struct ImmutableDirectedGraph<N, E>
    {
        public static readonly ImmutableDirectedGraph<N, E> Empty =
            new ImmutableDirectedGraph<N, E>(
                ImmutableDictionary<N, ImmutableDictionary<E, N>>.Empty);

        private ImmutableDictionary<N, ImmutableDictionary<E, N>> graph;

        private ImmutableDirectedGraph(
            ImmutableDictionary<N, ImmutableDictionary<E, N>> graph)
        {
            this.graph = graph;
        }

        public ImmutableDirectedGraph<N, E> AddNode(N node)
        {
            if (graph.ContainsKey(node))
                return this;
            return new ImmutableDirectedGraph<N, E>(
              graph.Add(node, ImmutableDictionary<E, N>.Empty));
        }
        public ImmutableDirectedGraph<N, E> AddEdge(N start, N finish, E edge)
        {
            var g = this.AddNode(start).AddNode(finish);
            return new ImmutableDirectedGraph<N, E>(
              g.graph.SetItem(start, g.graph[start].SetItem(edge, finish)));
        }

        public IReadOnlyDictionary<E, N> Edges(N node)
        {
            return graph.ContainsKey(node) ?
              graph[node] :
              ImmutableDictionary<E, N>.Empty;
        }

        public IEnumerable<ImmutableStack<KeyValuePair<E, N>>> AllEdgeTraversals(N start)
        {
            var edges = Edges(start);
            if (edges.Count == 0)
            {
                yield return ImmutableStack<KeyValuePair<E, N>>.Empty;
            }
            else
            {
                foreach (var pair in edges)
                    foreach (var path in AllEdgeTraversals(pair.Value))
                        yield return path.Push(pair);
            }

        }
        public IEnumerable<ImmutableStack<KeyValuePair<E, N>>>  AllEdgeTraversals<E, N>(N start,Func<N, IReadOnlyDictionary<E, N>> getEdges)
        {
            var edges = getEdges(start);
            if (edges.Count == 0)
            {
                yield return ImmutableStack<KeyValuePair<E, N>>.Empty;
            }
            else
            {
                foreach (var pair in edges)
                    foreach (var path in AllEdgeTraversals(pair.Value, getEdges))
                        yield return path.Push(pair);
            }
        }
    }
}