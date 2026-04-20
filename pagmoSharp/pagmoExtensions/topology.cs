using System;
using System.Linq;

namespace pagmo
{
    /// <summary>
    /// Projected topology connectivity payload.
    /// </summary>
    public readonly record struct TopologyConnectionData(uint[] NeighborIds, double[] Weights);

    /// <summary>
    /// Extension helpers that project topology connectivity into C# arrays.
    /// </summary>
    public static class TopologyConnectionExtensions
    {
        private static T RequireTopology<T>(T topologyInstance, string parameterName) where T : class
        {
            return topologyInstance ?? throw new ArgumentNullException(parameterName);
        }

        private static TopologyConnectionData ToConnectionData(TopologyConnections rawConnections)
        {
            using (rawConnections)
            {
                var neighborIds = rawConnections.first.ToArray();
                var weights = rawConnections.second.ToArray();
                return new TopologyConnectionData(neighborIds, weights);
            }
        }

        /// <summary>
        /// Returns neighbor ids and edge weights for a vertex in a generic topology.
        /// </summary>
        public static TopologyConnectionData GetConnectionsData(this topology topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        /// <summary>
        /// Returns neighbor ids and edge weights for a vertex in a fully-connected topology.
        /// </summary>
        public static TopologyConnectionData GetConnectionsData(this fully_connected topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        /// <summary>
        /// Returns neighbor ids and edge weights for a vertex in a ring topology.
        /// </summary>
        public static TopologyConnectionData GetConnectionsData(this ring topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        /// <summary>
        /// Returns neighbor ids and edge weights for a vertex in a free-form topology.
        /// </summary>
        public static TopologyConnectionData GetConnectionsData(this free_form topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        /// <summary>
        /// Returns neighbor ids and edge weights for a vertex in an unconnected topology.
        /// </summary>
        public static TopologyConnectionData GetConnectionsData(this unconnected topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }
    }
}
