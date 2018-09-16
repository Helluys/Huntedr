using System.Collections.Generic;

using UnityEngine;

public interface IPathNode {
    List<IPathNode> adjacentNodes { get; }
    Vector3 position { get; }

    float GetCost (IPathNode targetNode);

    void AddAdjacentNode (IPathNode neighboor);
}
