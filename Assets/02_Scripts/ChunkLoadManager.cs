using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoadManager : MonoBehaviour
{
    // ûũ ������ �迭, �ִ� ûũ ����, ���Լ����
    // Ÿ�� ��ġ �������� ûũ ���� �� ��Ȱ��ȭ + ��Ȱ��
    // z�� �������� ���ġ
    public GameObject[] chunkPrefabs;  // map01, map02 ���� ûũ �����յ�
    public int poolSize = 5;  // �ִ� ������ ûũ ����
    private Queue<GameObject> chunkPool = new Queue<GameObject>();
    [SerializeField] private Transform target;
    private float spawnZ = 0f;
    private float chunkLength = 100f; // ûũ �ϳ��� ����

    void Start()
    {
        // �̸� ûũ �غ�
        // ù ûũ ��ġ
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
        // �÷��̾ ���� �Ÿ� �̻� �����ϸ� ���ο� ûũ �߰�(���� �ӽ÷� �����ϸ� �̰ɷ� Ȯ��)
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
