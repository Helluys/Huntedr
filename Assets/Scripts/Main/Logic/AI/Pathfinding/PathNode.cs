using System.Collections.Generic;

using UnityEngine;

public class PathNode : MonoBehaviour, IPathNode {

    public List<IPathNode> adjacentNodes { get; private set; } = new List<IPathNode>();
    public Vector3 position { get { return this.transform.position; } }

    [SerializeField] private List<PathNode> adjacentNodeList = new List<PathNode>();

    private void Start () {
        adjacentNodes.AddRange(adjacentNodeList);
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

    private void OnDrawGizmos () {
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        foreach (PathNode node in this.adjacentNodes)
            Gizmos.DrawLine(this.transform.position, node.transform.position);
    }
}
