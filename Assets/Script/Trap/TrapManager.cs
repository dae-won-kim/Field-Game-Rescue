using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public GameObject trapPrefab;
    private GameObject island;

    private float TRAP_RESPAWN_TIME = 40f;
    [SerializeField] ObjectPool trapPool;
    private float timer;

    private float trapSpawnMargin = 8f; // x, z 방향에서 얼마나 안쪽으로 제한할지 설정

    private void SpawnTrapAtRandomPosition()
    {
        Renderer islandRenderer = island.GetComponent<Renderer>();
        if (islandRenderer == null) return;

        Bounds bounds = islandRenderer.bounds;

        float x = Random.Range(bounds.min.x + trapSpawnMargin, bounds.max.x - trapSpawnMargin);
        float z = Random.Range(bounds.min.z + trapSpawnMargin, bounds.max.z - trapSpawnMargin);
        float y = bounds.max.y;

        Vector3 spawnPos = new Vector3(x, y, z);
        trapPool.GetObjectAtPosition(spawnPos);
    }

    void Start()
    {
        island = GameObject.Find("Island");
        // trapPool =  new ObjectPool(trapPrefab, this.transform, 10);
        trapPool = this.gameObject.AddComponent<ObjectPool>();
        trapPool.Init(trapPrefab, this.transform, 10);

        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= TRAP_RESPAWN_TIME)
        {
            SpawnTrapAtRandomPosition();
            SpawnTrapAtRandomPosition();
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
        Vector3 marginVec = new Vector3(trapSpawnMargin * 2f, 0f, trapSpawnMargin * 2f);
        Vector3 adjustedSize = originalBounds.size - marginVec;
        Vector3 adjustedCenter = originalBounds.center;

        // 트랩 생성 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(adjustedCenter, adjustedSize);
    }
}
