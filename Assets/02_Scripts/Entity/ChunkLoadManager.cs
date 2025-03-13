using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChunkLevel
{
    public GameObject[] chunkPrefabs;
}

public class ChunkLoadManager : MonoBehaviour
{
    [SerializeField] private List<ChunkLevel> chunkLevels; // 레벨별 청크 프리팹 리스트
    [SerializeField] private int poolSize = 5;  // 최대 유지할 청크 개수
    [SerializeField] private Transform target;
    [SerializeField] private int currentLevel = 0; // 현재 레벨 (0부터 시작)
    [SerializeField] private float defaultChunkLength = 100f; // 청크 길이 기본값
    [SerializeField] private float recycleDistance = 100f; // 플레이어와의 거리가 이 값보다 크면 청크 재활용

    // 현재 레벨의 청크 풀 (재사용할 수 있는 청크덜)
    private List<GameObject> chunkPool = new List<GameObject>();

    // 활성화된 청크들 (배치된 청크들)
    private List<GameObject> activeChunks = new List<GameObject>();

    private float spawnZ = 0f; // 다음 청크 생성 위치
    private float chunkLength = 100f; // 청크 하나의 길이
    private Dictionary<GameObject, int> chunkLevelMap = new Dictionary<GameObject, int>();

    private List<int> prefabIndices = new List<int>();
    private int currentPrefabIndex = 0;
    private bool needToSpawnNewChunk = false;

    void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }

        if (chunkLevels == null || chunkLevels.Count == 0)
        {
            enabled = false;
            return;
        }
        currentLevel = Mathf.Clamp(currentLevel, 0, chunkLevels.Count - 1);

        InitializePrefabIndices(currentLevel);
        InitializeChunkPool();
        for (int i = 0; i < poolSize; i++)
        {
            SpawnChunk();
        }
    }

    /// <summary>
    /// 업데이트. 플레이어가 일정 거리 진행하면 청크 추가하며 뒤에 있는 청크들은 비활성화 후 다시 앞쪽으로 배치
    /// </summary>
    void Update()
    {
        if (target.position.z > spawnZ - recycleDistance)
        {
            SpawnChunk();
        }

        bool recycledChunk = false;
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = activeChunks[i];
            if (target.position.z - chunk.transform.position.z > recycleDistance)
            {
                RecycleSpecificChunk(chunk);
                recycledChunk = true;
                needToSpawnNewChunk = true;
            }
        }

        if (needToSpawnNewChunk)
        {
            SpawnChunk();
            needToSpawnNewChunk = false;
        }
    }

    /// <summary>
    /// 레벨에 맞게 청크 프리팹 배열 생성
    /// </summary>
    /// <param name="level"></param>
    void InitializePrefabIndices(int level)
    {
        if (level < 0 || level >= chunkLevels.Count) return;

        ChunkLevel chunkLevel = chunkLevels[level];
        if (chunkLevel.chunkPrefabs == null || chunkLevel.chunkPrefabs.Length == 0) return;

        prefabIndices.Clear();

        if (chunkLevel.chunkPrefabs.Length < poolSize)
        {
            Debug.LogWarning("청크 부족");
        }

        for (int i = 0; i < chunkLevel.chunkPrefabs.Length; i++)
        {
            prefabIndices.Add(i);
        }

        // 현재 풀에 있는 청크들을 섞어줌.(같은 청크가 반복되는 게 싫어서 일단 추가)
        for (int i = 0; i < prefabIndices.Count; i++)
        {
            int temp = prefabIndices[i];
            int randomIndex = Random.Range(i, prefabIndices.Count);
            prefabIndices[i] = prefabIndices[randomIndex];
            prefabIndices[randomIndex] = temp;
        }

        currentPrefabIndex = 0;
    }

    /// <summary>
    /// 다음 청크 프리팹 인덱스 가져오기
    /// </summary>
    /// <returns></returns>
    int GetNextPrefabIndex()
    {
        if (prefabIndices.Count == 0)
        {
            InitializePrefabIndices(currentLevel);
            if (prefabIndices.Count == 0) return 0;
        }

        int index = prefabIndices[currentPrefabIndex];
        currentPrefabIndex = (currentPrefabIndex + 1) % prefabIndices.Count;

        if (currentPrefabIndex == 0) // 한 바퀴 다 돌았으면 다시 섞어줌
        {
            for (int i = 0; i < prefabIndices.Count; i++)
            {
                int temp = prefabIndices[i];
                int randomIndex = Random.Range(i, prefabIndices.Count);
                prefabIndices[i] = prefabIndices[randomIndex];
                prefabIndices[randomIndex] = temp;
            }
        }

        return index;
    }

    /// <summary>
    /// 청크 풀 초기화
    /// </summary>
    void InitializeChunkPool()
    {
        ChunkLevel currentChunkLevel = chunkLevels[currentLevel];
        if (currentChunkLevel.chunkPrefabs == null || currentChunkLevel.chunkPrefabs.Length == 0)
        {
            return;
        }

        int prefabCount = currentChunkLevel.chunkPrefabs.Length;

        for (int i = 0; i < prefabCount; i++)
        {
            GameObject prefab = currentChunkLevel.chunkPrefabs[i];
            GameObject chunk = Instantiate(prefab);

            Collider chunkCollider = chunk.GetComponent<Collider>();
            if (chunkCollider != null)
            {
                chunkLength = chunkCollider.bounds.size.z;
            }
            else
            {
                chunkLength = defaultChunkLength;
            }

            chunk.SetActive(false);
            chunkPool.Add(chunk);

            chunkLevelMap[chunk] = currentLevel;
        }
    }

    /// <summary>
    /// 레벨에 따라 청크 생성
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    GameObject CreateChunkForLevel(int level)
    {
        if (level < 0 || level >= chunkLevels.Count)
        {
            return null;
        }

        ChunkLevel chunkLevel = chunkLevels[level];
        if (chunkLevel.chunkPrefabs == null || chunkLevel.chunkPrefabs.Length == 0)
        {
            return null;
        }

        if (level != currentLevel)
        {
            InitializePrefabIndices(level);
        }

        int prefabIndex = GetNextPrefabIndex();

        if (prefabIndex >= chunkLevel.chunkPrefabs.Length)
        {
            prefabIndex = prefabIndex % chunkLevel.chunkPrefabs.Length;
        }

        GameObject prefab = chunkLevel.chunkPrefabs[prefabIndex];
        GameObject newChunk = Instantiate(prefab);

        chunkLevelMap[newChunk] = level;

        Collider chunkCollider = newChunk.GetComponent<Collider>();
        if (chunkCollider != null)
        {
            chunkLength = chunkCollider.bounds.size.z;
        }

        return newChunk;
    }

    /// <summary>
    /// 오브젝트 풀링으로 청크 가져옴
    /// </summary>
    /// <returns></returns>
    GameObject GetChunkFromPool()
    {
        if (chunkPool.Count == 0)
        {
            GameObject newChunk = CreateChunkForLevel(currentLevel);
            return newChunk;
        }

        GameObject chunk = chunkPool[0];
        chunkPool.RemoveAt(0);

        if (!chunkLevelMap.ContainsKey(chunk) || chunkLevelMap[chunk] != currentLevel)
        {
            Destroy(chunk);
            chunk = CreateChunkForLevel(currentLevel);
        }
        return chunk;
    }

    /// <summary>
    /// 청크 생성
    /// </summary>
    void SpawnChunk()
    {
        GameObject chunk = GetChunkFromPool();
        if (chunk == null) return;

        chunk.transform.position = new Vector3(0, 0, spawnZ);
        chunk.SetActive(true);

        activeChunks.Add(chunk);
        spawnZ += chunkLength;
    }

    /// <summary>
    /// 청크들 재활용(오브젝트 풀링)
    /// </summary>
    /// <param name="chunk"></param>
    void RecycleSpecificChunk(GameObject chunk)
    {
        if (!activeChunks.Contains(chunk)) return;

        chunk.SetActive(false);
        activeChunks.Remove(chunk);

        // 현재 레벨과 다른 청크는 파괴
        if (!chunkLevelMap.ContainsKey(chunk) || chunkLevelMap[chunk] != currentLevel)
        {
            Destroy(chunk);
        }
        else
        {
            // 같은 레벨의 청크는 그대로 풀에 반환
            chunkPool.Add(chunk);
        }

        // 여기서 풀에 있는 청크들을 섞는 로직 추가
        if (chunkPool.Count > 1)
        {
            // 풀에 있는 청크들 섞기
            for (int i = 0; i < chunkPool.Count; i++)
            {
                GameObject temp = chunkPool[i];
                int randomIndex = Random.Range(0, chunkPool.Count);
                chunkPool[i] = chunkPool[randomIndex];
                chunkPool[randomIndex] = temp;
            }
        }
    }

    /// <summary>
    /// 레벨 변경 메서드
    /// </summary>
    /// <param name="newLevel"></param>
    public void ChangeLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel >= chunkLevels.Count) return;
        if (currentLevel == newLevel) return;

        Debug.Log($"레벨 변경 {currentLevel} -> {newLevel}");

        int previousLevel = currentLevel;

        currentLevel = newLevel;

        InitializePrefabIndices(newLevel);

        int chunksToKeep = Mathf.CeilToInt(poolSize / 2f);
        int activeCount = activeChunks.Count;

        List<GameObject> chunksToReplace = new List<GameObject>();
        for (int i = chunksToKeep; i < activeCount; i++)
        {
            chunksToReplace.Add(activeChunks[i]);
        }

        foreach (GameObject oldChunk in chunksToReplace)
        {
            int index = activeChunks.IndexOf(oldChunk);
            if (index >= 0)
            {
                Vector3 chunkPosition = oldChunk.transform.position;

                oldChunk.SetActive(false);
                activeChunks.RemoveAt(index);

                GameObject newChunk = CreateChunkForLevel(newLevel);

                newChunk.transform.position = chunkPosition;
                newChunk.SetActive(true);

                activeChunks.Add(newChunk);
            }
        }

        List<GameObject> oldLevelChunks = new List<GameObject>();
        foreach (GameObject poolChunk in chunkPool)
        {
            if (!chunkLevelMap.ContainsKey(poolChunk) || chunkLevelMap[poolChunk] != newLevel)
            {
                oldLevelChunks.Add(poolChunk);
            }
        }

        foreach (GameObject oldChunk in oldLevelChunks)
        {
            chunkPool.Remove(oldChunk);
            Destroy(oldChunk);
        }

        chunkPool.Clear();
        InitializeChunkPool();

        if (activeChunks.Count > 0)
        {
            GameObject lastChunk = activeChunks[activeChunks.Count - 1];
            spawnZ = lastChunk.transform.position.z + chunkLength;
        }
    }

    /// <summary>
    /// 게임 종료 시 메서드
    /// </summary>
    public void OnGameEnd()
    {
        foreach (GameObject chunk in chunkPool)
        {
            Destroy(chunk);
        }

        foreach (GameObject chunk in activeChunks)
        {
            Destroy(chunk);
        }

        chunkPool.Clear();
        activeChunks.Clear();
        chunkLevelMap.Clear();
        prefabIndices.Clear();
    }
}