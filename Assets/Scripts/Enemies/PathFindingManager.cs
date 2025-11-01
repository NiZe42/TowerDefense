using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Designed to have only 1 grid in the world.
public class PathFindingManager : MonoBehaviourSingleton<PathFindingManager> {
   internal static Action<List<Tile>> OnPathUpdated;
   
   private Transform startTransform;
   private Transform finishPosition;
   
   private PathFinder pathFinder;
   
   internal TileGrid tileGrid;
   internal List<Tile> currentPath = new List<Tile>();
   public void Start() {
      tileGrid = FindAnyObjectByType<TileGrid>();
      startTransform = GameObject.FindGameObjectWithTag("StartPosition").transform;
      finishPosition = GameObject.FindGameObjectWithTag("FinishPosition").transform;
      pathFinder = new PathFinder(
         tileGrid, 
         startTransform.position,
         finishPosition.position);
      currentPath = pathFinder.FindPath();
      OnPathUpdated?.Invoke(currentPath);
   }

   public void Update() {
      if (Input.GetKeyDown(KeyCode.Space)) {
         Debug.Log("pressed");
         currentPath = pathFinder.FindPath();

         EventBus.Instance.InvokeEvent(
            new PathHasChanged(){newPath = currentPath.Select(tile => tile.worldPosition).ToList()});
         
         // We need to pass List<Tiles> into path visualizer.
         // I think Tile is strictly a grid property, so I created "local" event.
         OnPathUpdated?.Invoke(currentPath);
      }
   }
}
