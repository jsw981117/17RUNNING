using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoadManager : MonoBehaviour
{
    // 청크 프리팹 배열, 최대 청크 개수, 선입선출로
    // 타겟 위치 기준으로 청크 생성 및 비활성화 + 재활용
    // z값 기준으로 재배치
    public GameObject[] chunkPrefabs;  // map01, map02 등의 청크 프리팹들
    public int poolSize = 5;  // 최대 유지할 청크 개수
    private Queue<GameObject> chunkPool = new Queue<GameObject>();
    [SerializeField] private Transform target;
    private float spawnZ = 0f;
    private float chunkLength = 100f; // 청크 하나의 길이

    void Start()
    {
        // 미리 청크 준비
        // 첫 청크 배치
        for (int i = 0; i < poolSize; i++)
        {
            GameObject chunk = Instantiate(chunkPrefabs[Random.Range(0, chunkPrefabs.Length)]);
            chunk.SetActive(false);
            chunkPool.Enqueue(chunk);
        }

        for (int i = 0; i < poolSize; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // 플레이어가 일정 거리 이상 진행하면 새로운 청크 추가(값은 임시로 적절하면 이걸로 확정)
        if (target.position.z > spawnZ - (chunkLength * (poolSize / 2)))
        {
            SpawnChunk();
            RecycleChunk();
        }
    }

    void SpawnChunk()
    {
        GameObject chunk = chunkPool.Dequeue();
        chunk.transform.position = new Vector3(0, 0, spawnZ);
        chunk.SetActive(true);
        spawnZ += chunkLength;
        chunkPool.Enqueue(chunk);
    }

    void RecycleChunk()
    {
        GameObject chunk = chunkPool.Dequeue();
        chunk.SetActive(false);
        chunkPool.Enqueue(chunk);
    }
}
