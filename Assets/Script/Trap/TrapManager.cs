using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GameObject island;
    public List<GameObject> Traps = new List<GameObject>();

    public GameObject TrapPrefab;
    public static float TRAP_SPAWN_TIME = 30.0f;
    public float respawn_trap_time = 0.0f;

    Vector3 GetRandomPositionInIsland()
    {
        Bounds bounds = island.GetComponent<Renderer>().bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y;  // 트랩이 땅 위에 생성되도록

        return new Vector3(x, y, z);
    }
    void respawnTrap()
    {
        Vector3 spawnPos = GetRandomPositionInIsland();
        GameObject trap = Instantiate(TrapPrefab, spawnPos, Quaternion.identity);
        Traps.Add(trap);
    }
    void Start()
    {
        
    }

    void Update()
    {
        respawn_trap_time += Time.deltaTime;
        if(respawn_trap_time > TRAP_SPAWN_TIME)
        {
            respawnTrap();
            respawn_trap_time = 0.0f;
        }
    }
}
