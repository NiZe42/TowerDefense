using UnityEngine;

public abstract class MoveBehaviour : ScriptableObject {
    public abstract void Initialize(Enemy enemy);
    public abstract void Move(float speed);
}
