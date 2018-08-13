using System.Collections.Generic;

using UnityEngine;

public class PathFollower : AISubController {

    public float anticipationDistance = 30f;

    private Ship ship;
    private PathFinder pathFinder;
    private List<IPathNode> path;
    private int currentPathIndex = 0;

    private Vector3 previousPoint;
    private Vector3 currentPoint;
    private Vector3 nextPoint;

    private float distanceToCurrentPoint { get { return (this.currentPoint - this.ship.transform.position).magnitude; } }

    public PathFollower (Ship ship, Vector3 targetPoint) {
        this.ship = ship;
        this.pathFinder = new PathFinder(GameManager.GetPathNodesHolder());

        this.path = pathFinder.ComputePath(this.ship.transform.position, targetPoint);
        UpdatePoints();
    }

    public override SlidingModeControl.Target ComputeTarget () {
        Vector3 currentSegmentDirection = (this.currentPoint - this.previousPoint).normalized;
        float currentSegmentLength = (this.currentPoint - this.previousPoint).magnitude;
        float currentSegmentDistance = Vector3.Dot(this.ship.transform.position - this.previousPoint, currentSegmentDirection);

        // Update current and next point
        if (currentSegmentDistance > currentSegmentLength - anticipationDistance) {
            if (currentPathIndex == path.Count - 1)
                ObjectiveCompleted();

            currentPathIndex++;
            UpdatePoints();

            currentSegmentDirection = (this.currentPoint - this.previousPoint).normalized;
            currentSegmentLength = (this.currentPoint - this.previousPoint).magnitude;
            currentSegmentDistance = Vector3.Dot(this.ship.transform.position - this.previousPoint, currentSegmentDirection);
        }

        Vector3 finalPoint = previousPoint + (currentSegmentDistance + anticipationDistance) * currentSegmentDirection;
        return new SlidingModeControl.Target {
            point = finalPoint,
            aim = finalPoint
        };
    }

    private void UpdatePoints () {
        previousPoint = (currentPathIndex == 0) ? path[currentPathIndex].position : path[Mathf.Min(currentPathIndex - 1, path.Count - 1)].position;
        currentPoint = (currentPathIndex > path.Count - 1) ? path[path.Count - 1].position : path[currentPathIndex].position;
    }
}
