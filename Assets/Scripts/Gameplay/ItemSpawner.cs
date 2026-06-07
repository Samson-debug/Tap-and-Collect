using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnableItem
    {
        public CollectibleItem itemPrefab;
        [Range(0, 100)] public float spawnWeight = 10f;
    }

    [Header("Spawn Configuration")]
    [SerializeField] private List<SpawnableItem> spawnableItems = new List<SpawnableItem>();
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField, Range(0f, 1f)] private float screenMargin = 0.1f; // Margin to prevent spawning exactly on the edge
    private float spawnWidth;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO gameStartedEvent;
    [SerializeField] private VoidEventChannelSO gameOverEvent;

    private Dictionary<CollectibleItem, IObjectPool<CollectibleItem>> pools = new Dictionary<CollectibleItem, IObjectPool<CollectibleItem>>();
    private List<CollectibleItem> activeItems = new List<CollectibleItem>();
    private float timer;
    private bool isSpawning = false;

    private void Awake()
    {
        InitializePools();
    }

    private void OnEnable()
    {
        if (gameStartedEvent != null) gameStartedEvent.OnEventRaised += StartSpawning;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised += StopSpawning;
    }

    private void OnDisable()
    {
        if (gameStartedEvent != null) gameStartedEvent.OnEventRaised -= StartSpawning;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised -= StopSpawning;
    }

    private void StartSpawning()
    {
        if (isSpawning)
        {
            for (int i = activeItems.Count - 1; i >= 0; i--)
            {
                var item = activeItems[i];
                if (item.gameObject.activeInHierarchy && item.Pool != null)
                {
                    item.Pool.Release(item);
                }
            }
            activeItems.Clear();
        }
        isSpawning = true;
        timer = 0; // Reset timer when starting
    }

    private void StopSpawning()
    {
        isSpawning = false;
    }

    private void Start()
    {
        CalculateSpawnWidth();
    }

    private void InitializePools()
    {
        foreach (var spawnable in spawnableItems)
        {
            if (spawnable.itemPrefab != null && !pools.ContainsKey(spawnable.itemPrefab))
            {
                pools[spawnable.itemPrefab] = CreatePool(spawnable.itemPrefab);
            }
        }
    }

    private IObjectPool<CollectibleItem> CreatePool(CollectibleItem prefab)
    {
        return new ObjectPool<CollectibleItem>(
            createFunc: () =>
            {
                CollectibleItem item = Instantiate(prefab);
                // Assign the specific pool to the item so it can release itself back
                item.Pool = pools[prefab];
                return item;
            },
            actionOnGet: item =>
            {
                item.gameObject.SetActive(true);
                activeItems.Add(item);
            },
            actionOnRelease: item =>
            {
                item.gameObject.SetActive(false);
                activeItems.Remove(item);
            },
            actionOnDestroy: item =>
            {
                Destroy(item.gameObject);
            },
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    private void CalculateSpawnWidth()
    {
        float screenHeight = Camera.main.orthographicSize;
        float screenWidth = screenHeight * Camera.main.aspect;

        // Apply a margin so it doesn't spawn exactly on the edge
        spawnWidth =  screenWidth * (1f - screenMargin);
    }

    private void Update()
    {
        if (!isSpawning) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnRandomItem();
        }
    }

    private void SpawnRandomItem()
    {
        if (spawnableItems.Count == 0) return;

        CollectibleItem prefabToSpawn = GetRandomPrefabWeighted();
        if (prefabToSpawn == null) return;

        if (pools.TryGetValue(prefabToSpawn, out var pool))
        {
            CollectibleItem item = pool.Get();
            
            float randomX = Random.Range(-spawnWidth/2f, spawnWidth/2f);
            item.transform.position = transform.position + new Vector3(randomX, 0, 0);

            // Set Speed
            if (item.TryGetComponent<FallingMovement>(out var movement))
            {
                movement.SetSpeed(fallSpeed);
            }
        }
    }

    private CollectibleItem GetRandomPrefabWeighted()
    {
        float totalWeight = 0;
        foreach (var spawnable in spawnableItems)
        {
            totalWeight += spawnable.spawnWeight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var spawnable in spawnableItems)
        {
            currentWeight += spawnable.spawnWeight;
            if (randomValue <= currentWeight)
            {
                return spawnable.itemPrefab;    
            }
        }

        return null;
    }
}
