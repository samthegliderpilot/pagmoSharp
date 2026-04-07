using System;
using System.Linq;

namespace pagmo
{
    public readonly record struct TopologyConnectionData(uint[] NeighborIds, double[] Weights);

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

        public static TopologyConnectionData GetConnectionsData(this topology topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        public static TopologyConnectionData GetConnectionsData(this fully_connected topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        public static TopologyConnectionData GetConnectionsData(this ring topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }

        public static TopologyConnectionData GetConnectionsData(this unconnected topologyInstance, uint vertexId)
        {
            return ToConnectionData(RequireTopology(topologyInstance, nameof(topologyInstance)).get_connections(vertexId));
        }
    }
}
