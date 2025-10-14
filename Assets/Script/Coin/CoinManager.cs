using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public GameObject coinPrefab;
    private GameObject island;

    private float COIN_RESPAWN_TIME = 10f;
    [SerializeField] ObjectPool coinPool;
    private float timer;

    private float coinSpawnMargin = 4f; // x, z 방향에서 얼마나 안쪽으로 제한할지 설정
    private void SpawnCoinAtRandomPosition()
    {
        Renderer islandRenderer = island.GetComponent<Renderer>();
        if (islandRenderer == null) return;

        Bounds bounds = islandRenderer.bounds;

        float x = Random.Range(bounds.min.x + coinSpawnMargin, bounds.max.x - coinSpawnMargin);
        float z = Random.Range(bounds.min.z + coinSpawnMargin, bounds.max.z - coinSpawnMargin);
        float y = bounds.max.y;

        Vector3 spawnPos = new Vector3(x, y, z);
        coinPool.GetObjectAtPosition(spawnPos);
    }

    void Start()
    {
        island = GameObject.Find("Island");
        // coinPool =  new ObjectPool(coinPrefab, this.transform, 10);
        coinPool = this.gameObject.AddComponent<ObjectPool>();
        coinPool.Init(coinPrefab, this.transform, 10);

        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= COIN_RESPAWN_TIME)
        {
            SpawnCoinAtRandomPosition();
            SpawnCoinAtRandomPosition();
            timer = 0f;
        }
    }

    // 트랩 생성 범위 시각화
    private void OnDrawGizmos()
    {
        if (island == null) return;

        Renderer islandRenderer = island.GetComponent<Renderer>();
        if (islandRenderer == null) return;

        Bounds originalBounds = islandRenderer.bounds;

        // 제한된 트랩 생성 범위: 초록색
        Vector3 marginVec = new Vector3(coinSpawnMargin * 2f, 0f, coinSpawnMargin * 2f);
        Vector3 adjustedSize = originalBounds.size - marginVec;
        Vector3 adjustedCenter = originalBounds.center;

        // 트랩 생성 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(adjustedCenter, adjustedSize);
    }
}
