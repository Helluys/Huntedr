using System.Collections.Generic;

using UnityEngine;

public class PathNode : MonoBehaviour {

    public List<PathNode> adjacentNodes = new List<PathNode>();
    private Dictionary<PathNode, float> nodeCosts = new Dictionary<PathNode, float>();

    private void Start () {
        foreach(PathNode node in adjacentNodes)
            nodeCosts.Add(node, (node.transform.position - transform.position).magnitude);
    }
    
    public float GetCost (PathNode targetNode) {
        return nodeCosts[targetNode];
    }

    private void OnDrawGizmos () {
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        foreach (PathNode node in adjacentNodes) {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
