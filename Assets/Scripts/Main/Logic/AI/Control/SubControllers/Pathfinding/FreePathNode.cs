using System.Collections.Generic;

using UnityEngine;

internal class FreePathNode : IPathNode {
    public List<IPathNode> adjacentNodes { get; private set; } = new List<IPathNode>();
    public Vector3 position { get; private set; }

    public FreePathNode (Vector3 position, List<IPathNode> network) {
        this.position = position;
        network.Add(this);
        foreach (IPathNode node in network) {
            if (!Physics.Raycast(position, (node.position - position).normalized, (node.position - position).magnitude))
               AddAdjacentNode(PathFinder.FindClosestNode(position, network));
        }
    }

    public float GetCost (IPathNode targetNode) {
        return (targetNode.position - this.position).magnitude;
    }

    public void AddAdjacentNode (IPathNode neighboor) {
        if (!adjacentNodes.Contains(neighboor)) {
            this.adjacentNodes.Add(neighboor);
            neighboor.AddAdjacentNode(this);
        }
    }

    public void ReleaseNode(List<IPathNode> network) {
        network.Remove(this);
        foreach (IPathNode neighboor in adjacentNodes)
            neighboor.adjacentNodes.Remove(this);
    }
}