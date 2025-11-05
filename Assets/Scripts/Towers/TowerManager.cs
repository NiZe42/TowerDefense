using System.Collections.Generic;

public class TowerManager : MonoBehaviourSingleton<TowerManager>
{
    public Dictionary<int, Tower> towers { get; } = new Dictionary<int, Tower>();

    private void Start() { }

    public override void OnDestroy()
    {
        towers.Clear();
    }
}