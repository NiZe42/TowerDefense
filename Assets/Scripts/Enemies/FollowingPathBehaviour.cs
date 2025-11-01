using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "FollowingPathBehaviour", menuName = "MoveBehaviour/FollowingPathBehaviour")]
public class FollowingPathBehaviour : MoveBehaviour
{
    private Enemy enemy;
    private List<Vector3> path = new List<Vector3>();
    private int currentWaypointIndex = 0;

    public override void Initialize(Enemy enemy)
    {
        path = PathFindingManager.Instance.currentPath.Select(tile => tile.worldPosition).ToList();
        EventBus.Instance.Subscribe<PathHasChanged>(UpdatePath);
        this.enemy = enemy;
    }

    public override void Move(float speed)
    {
        if (path == null || path.Count == 0)
            return;

        Vector3 targetWaypoint = path[currentWaypointIndex];

        Vector3 direction = 
            (targetWaypoint - enemy.transform.position).normalized;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 5f * speed; 
        enemy.transform.rotation = Quaternion.Slerp(
            enemy.transform.rotation,
            targetRotation,
            rotationSpeed);
        
        enemy.transform.position += speed * Time.deltaTime * enemy.transform.forward;

        if (Vector3.Distance(enemy.transform.position, targetWaypoint) < 0.1f)
        {
            currentWaypointIndex++;
        }
    }

    // TODO: make it so enemy looks for the closest waypoint from previous closest waypoint index, not from the start.
    private void UpdatePath(PathHasChanged @event) {
        path = @event.newPath;
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < path.Count; i++)
        {
            float dist = Vector3.Distance(enemy.transform.position, path[i]);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestIndex = i;
            }
        }
        currentWaypointIndex = closestIndex;
    }

}
