using System.Collections.Generic;

using UnityEngine;

public class PathFinder {

    private List<PathNode> network;

    private struct NodeData {
        public float evaluation;
        public PathNode parent;

        public NodeData (float evaluation, PathNode parent) {
            this.evaluation = evaluation;
            this.parent = parent;
        }
    }
    
    public PathFinder (GameObject networkHolder) {
        this.network = new List<PathNode>();
        networkHolder.GetComponentsInChildren(network);
    }

    public List<Vector3> ComputePath (Vector3 startPoint, Vector3 endPoint) {
        // openList contains all nodes that need to be evaluated
        Dictionary<PathNode, NodeData> openList = new Dictionary<PathNode, NodeData>();
        // closedList contains all nodes that have already been evaluated
        Dictionary<PathNode, NodeData> closedList = new Dictionary<PathNode, NodeData>();

        // Find limit nodes from  world points
        PathNode startNode = FindClosestNode(startPoint);
        PathNode endNode = FindClosestNode(endPoint);

        // Start algorithm at startNode
        openList.Add(startNode, new NodeData(Heuristic(startNode, endNode), null));

        // A* loop
        bool goalReached = false;
        while (openList.Count > 0 && !goalReached) {
            // Get the node in the openList with the lowest evaluation
            PathNode currentNode = PickCurrentNode(openList);
            float previousHeuristic = Heuristic(currentNode, endNode);

            // Add it to closedList and remove it from openList
            closedList.Add(currentNode, openList[currentNode]);
            openList.Remove(currentNode);
            
            if (currentNode.Equals(endNode)) {
                // Found the end : algorithm finished
                goalReached = true;
            } else {
                // Develop all neighbouring nodes
                foreach (PathNode neighbour in currentNode.adjacentNodes) {
                    float evaluation = closedList[currentNode].evaluation - previousHeuristic + currentNode.GetCost(neighbour) + Heuristic(neighbour, endNode);
                    DevelopNode(neighbour, new NodeData(evaluation, currentNode), openList, closedList);
                }
            }
        }

        if (!goalReached)
            throw new UnreachableNodeException();

        List<Vector3> finalPath = GetFinalPath(closedList, endNode);
        finalPath.Add(endPoint);
        return finalPath;
    }

    private PathNode FindClosestNode (Vector3 point) {
        float minDistance = Mathf.Infinity;
        PathNode closestNode = null;

        foreach (PathNode node in network) {
            float distance = (node.transform.position - point).magnitude;
            if (distance < minDistance) {
                minDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    // Returns the node in the openList with the lowest evaluation
    private PathNode PickCurrentNode (Dictionary<PathNode, NodeData> openList) {
        PathNode bestNode = null;
        float bestValue = Mathf.Infinity;
        foreach (KeyValuePair<PathNode, NodeData> kvp in openList) {
            if (kvp.Value.evaluation < bestValue) {
                bestNode = kvp.Key;
                bestValue = kvp.Value.evaluation;
            }
        }
        return bestNode;
    }

    private void DevelopNode (PathNode node, NodeData nodeData, Dictionary<PathNode, NodeData> openList, Dictionary<PathNode, NodeData> closedList) {
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

    private float Heuristic (PathNode start, PathNode end) {
        return (end.transform.position - start.transform.position).magnitude;
    }
    private List<Vector3> GetFinalPath (Dictionary<PathNode, NodeData> closedList, PathNode endNode) {
        List<Vector3> path = new List<Vector3>();
        PathNode currentNode = endNode;

        do {
            path.Add(currentNode.transform.position);
            currentNode = closedList[currentNode].parent;
        } while (currentNode != null);

        path.Reverse();
        
        return path;
    }



}