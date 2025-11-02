using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowingPathBehaviour : MoveBehaviour
{
    private int currentWaypointIndex;
    private Enemy enemy;
    private bool hasFinishedMoving;
    private List<Vector3> path = new List<Vector3>();

    private void OnDestroy()
    {
        EventBus.Instance?.Unsubscribe<OnPathHasChanged>(UpdatePath);
    }

    public override event Action OnReachedFinish;

    public override void Initialize(Enemy enemy)
    {
        if (PathFindingManager.Instance != null)
        {
            path = PathFindingManager.Instance.GetCurrentPath();
        }

        this.enemy = enemy;
        EventBus.Instance.Subscribe<OnPathHasChanged>(UpdatePath);
    }

    public override void Move(float speed)
    {
        if (path == null || path.Count == 0 || hasFinishedMoving)
        {
            return;
        }

        Vector3 targetWaypoint = path[currentWaypointIndex];

        Vector3    direction      = (targetWaypoint - enemy.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float      rotationSpeed  = 7f * speed;
        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            rotationSpeed);

        enemy.transform.position += speed * enemy.transform.forward;

        if (Vector3.Distance(enemy.transform.position, targetWaypoint) < 0.1f)
        {
            if (currentWaypointIndex == path.Count - 1)
            {
                OnReachedFinish?.Invoke();
                hasFinishedMoving = true;
                return;
            }

            currentWaypointIndex++;
        }
    }

    // TODO: make it so enemy looks for the closest waypoint from previous closest waypoint index, not from the start.
    private void UpdatePath(OnPathHasChanged @event)
    {
        path = @event.newPath;
        var closestDistance = float.MaxValue;
        var closestIndex    = 0;

        for (var i = 0; i < path.Count; i++)
        {
            float dist = Vector3.Distance(enemy.transform.position, path[i]);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestIndex    = i;
            }
        }

        currentWaypointIndex = closestIndex;
    }
}