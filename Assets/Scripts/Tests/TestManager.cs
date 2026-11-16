using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestManager : MonoBehaviour
{
    [SerializeField]
    public TestEntity entityPrefab;
    
    private ComponentPool<TestEntity> entityPool;
    private List<TestEntity> activeEntities = new List<TestEntity>();

    void Awake()
    {
        ServiceLocator.Instance.Register<TestManager>(this);
        Debug.Log("Initialized test Manager");
    }

    void Start()
    {
        entityPool = new ComponentPool<TestEntity>(entityPrefab, 10, this.transform);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TestEntity entity = entityPool.Get();
            entity.transform.position = Random.insideUnitSphere * 10;

            activeEntities.Add(entity);
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (activeEntities.Count > 0)
            {
                int lastIndex = activeEntities.Count - 1;
                TestEntity entityToReturn = activeEntities[lastIndex];

                activeEntities.RemoveAt(lastIndex);

                entityPool.Return(entityToReturn);
            }
            else
            {
                Debug.Log("No active entities to return!");
            }
        }
    }
}
