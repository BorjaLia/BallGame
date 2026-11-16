using UnityEngine;

public class TestEntity : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private MeshRenderer meshRenderer;

    public float timeLimit = 3.0f;

    bool changed = false;
    private float accumulatedTime = 0.0f;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (changed) return;
        accumulatedTime += Time.deltaTime;
        if (accumulatedTime > timeLimit)
        {
            Debug.Log("I AM YELLOW");
            meshRenderer.material.color = Color.yellow;
            changed = true;
        }
    }

    public void OnSpawn()
    {
        accumulatedTime = 0.0f;
        changed = false;
        meshRenderer.material.color = Color.white;
        Debug.Log("Entity has spawned!");
    }
    public void OnDespawn()
    {
        accumulatedTime = 0.0f;
        changed = false;
        meshRenderer.material.color = Color.red;
        Debug.Log("Entity has despawned!");
    }
}
