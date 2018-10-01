using System.Collections.Generic;

using UnityEngine;

public class PathFinder {

    private readonly List<IPathNode> network;

    private struct NodeData {
        public float evaluation;
        public IPathNode parent;

        public NodeData (float evaluation, IPathNode parent) {
            this.evaluation = evaluation;
            this.parent = parent;
        }
    }

    public static IPathNode FindClosestNode (Vector3 point, List<IPathNode> network) {
        float minDistance = Mathf.Infinity;
        IPathNode closestNode = null;

        foreach (IPathNode node in network) {
            float distance = (node.position - point).magnitude;
            if (distance < minDistance && !Physics.Raycast(new Ray(point, node.position - point), distance)) {
                minDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public PathFinder (GameObject networkHolder) {
        this.network = new List<IPathNode>();
        networkHolder.GetComponentsInChildren(network);
    }

    public List<IPathNode> ComputePath (Vector3 startPoint, Vector3 endPoint) {
        // openList contains all nodes that need to be evaluated
        Dictionary<IPathNode, NodeData> openList = new Dictionary<IPathNode, NodeData>();
        // closedList contains all nodes that have already been evaluated
        Dictionary<IPathNode, NodeData> closedList = new Dictionary<IPathNode, NodeData>();

        // Create free nodes at start and end points
        FreePathNode startNode = new FreePathNode(startPoint, this.network);
        FreePathNode endNode = new FreePathNode(endPoint, this.network);

        // Start algorithm at startNode
        openList.Add(startNode, new NodeData(Heuristic(startNode, endNode), null));

        // A* loop
        bool goalReached = false;
        while (openList.Count > 0 && !goalReached) {
            // Get the node in the openList with the lowest evaluation
            IPathNode currentNode = PickCurrentNode(openList);
            float previousHeuristic = Heuristic(currentNode, endNode);

            // Add it to closedList and remove it from openList
            closedList.Add(currentNode, openList[currentNode]);
            openList.Remove(currentNode);
            
            if (currentNode.Equals(endNode)) {
                // Found the end : algorithm finished
                goalReached = true;
            } else {
                // Develop all neighbouring nodes
                foreach (IPathNode neighbour in currentNode.adjacentNodes) {
                    float evaluation = closedList[currentNode].evaluation - previousHeuristic + currentNode.GetCost(neighbour) + Heuristic(neighbour, endNode);
                    DevelopNode(neighbour, new NodeData(evaluation, currentNode), openList, closedList);
                }
            }
        }

        // Disconnect FreePathNodes from network
        startNode.ReleaseNode(network);
        endNode.ReleaseNode(network);

        // Goal is not reachable
        if (!goalReached)
            throw new UnreachableNodeException();
        
        // Compute final path
        List<IPathNode> finalPath = GetFinalPath(closedList, endNode);

        return finalPath;
    }

    // Returns the node in the openList with the lowest evaluation
    private IPathNode PickCurrentNode (Dictionary<IPathNode, NodeData> openList) {
        IPathNode bestNode = null;
        float bestValue = Mathf.Infinity;
        foreach (KeyValuePair<IPathNode, NodeData> kvp in openList) {
            if (kvp.Value.evaluation < bestValue) {
                bestNode = kvp.Key;
                bestValue = kvp.Value.evaluation;
            }
        }
        return bestNode;
    }

    private void DevelopNode (IPathNode node, NodeData nodeData, Dictionary<IPathNode, NodeData> openList, Dictionary<IPathNode, NodeData> closedList) {
        if (openList.ContainsKey(node)) {
            if (nodeData.evaluation < openList[node].evaluation) {
                openList[node] = nodeData;
            }
        } else if (closedList.ContainsKey(node)) {
            if (nodeData.evaluation < closedList[node].evaluation) {
                openList.Add(node, nodeData);
                closedList.Remove(node);
            }
        } else {
            openList.Add(node, nodeData);
        }
    }

    private float Heuristic (IPathNode start, IPathNode end) {
        return (end.position - start.position).magnitude;
    }

    private List<IPathNode> GetFinalPath (Dictionary<IPathNode, NodeData> closedList, IPathNode endNode) {
        List<IPathNode> path = new List<IPathNode>();
        IPathNode currentNode = endNode;

        do {
            path.Add(currentNode);
            currentNode = closedList[currentNode].parent;
        } while (currentNode != null);

        path.Reverse();
        
        return path;
    }

}