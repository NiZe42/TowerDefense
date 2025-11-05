using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private GameObject towerVisualsPrefab;

    [SerializeField]
    private ShootingBehaviour shootingBehaviour;

    private TowerVisuals towerVisualsInstance;
    public int id { get; private set; }

    public void Awake()
    {
        towerVisualsInstance =
            Instantiate(towerVisualsPrefab, transform.position, Quaternion.identity)
                .GetComponent<TowerVisuals>();
    }
}