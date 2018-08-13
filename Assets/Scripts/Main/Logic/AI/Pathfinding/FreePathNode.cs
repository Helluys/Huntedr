using System.Collections.Generic;

using UnityEngine;

internal class FreePathNode : IPathNode {
    public List<IPathNode> adjacentNodes { get; private set; } = new List<IPathNode>();
    public Vector3 position { get; private set; }

    public FreePathNode (Vector3 position, List<IPathNode> network) {
        this.position = position;
        network.Add(this);
        AddAdjacentNode(FindClosestNode(position, network));
    }

    public float GetCost (IPathNode targetNode) {
        return (targetNode.position - this.position).magnitude;
    }

    private IPathNode FindClosestNode (Vector3 point, List<IPathNode> network) {
        float minDistance = Mathf.Infinity;
        IPathNode closestNode = null;

        foreach (IPathNode node in network) {
            float distance = (node.position - point).magnitude;
            if (distance < minDistance && Physics.Raycast(new Ray(point, node.position - point), Mathf.Infinity)) {
                minDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
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