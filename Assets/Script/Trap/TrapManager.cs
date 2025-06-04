using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public Trap trapPrefab;
    private GameObject island;

    private float TRAP_RESPAWN_TIME = 60f;  // 1분 마다 트랩 2개 생성

    private ObjectPool trapPool;
    private float timer;

    private void SpawnTrapAtRandomPosition()
    {
        Renderer islandRenderer = island.GetComponent<Renderer>();
        if (islandRenderer == null) return;

        Bounds bounds = islandRenderer.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y;

        Vector3 spawnPos = new Vector3(x, y, z);
        trapPool.GetObjectAtPosition(spawnPos);
    }

    void Start()
    {
        island = GameObject.Find("Island");
        trapPool = new ObjectPool(trapPrefab, this.transform, 10);
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= TRAP_RESPAWN_TIME)
        {
            // 1분마다 2개 생성 
            SpawnTrapAtRandomPosition();
            SpawnTrapAtRandomPosition();
            timer = 0f;
        }
    }
  
}
