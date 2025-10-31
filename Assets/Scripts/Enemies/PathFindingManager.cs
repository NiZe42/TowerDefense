using System;
using System.Collections.Generic;
using UnityEngine;

// Designed to have only 1 grid in the world.
public class PathFindingManager : MonoBehaviourSingleton<PathFindingManager> {
   internal static Action<List<Tile>> OnPathUpdated;
   
   [SerializeField]
   private Vector2 startPosition;
   [SerializeField]
   private Vector2 endPosition;
   
   private PathFinder pathFinder;
   
   internal TileGrid tileGrid;
   internal List<Tile> currentPath = new List<Tile>();
   public void Start() {
      tileGrid = FindAnyObjectByType<TileGrid>();
      pathFinder = new PathFinder(
         tileGrid, 
         new Vector2(startPosition.x, startPosition.y),
         new Vector2(endPosition.x, endPosition.y));
      currentPath = pathFinder.FindPath();
   }

   public void Update() {
      if (Input.GetKeyDown(KeyCode.Space)) {
         Debug.Log("pressed");
         currentPath = pathFinder.FindPath();
         OnPathUpdated.Invoke(currentPath);
      }
   }
}
