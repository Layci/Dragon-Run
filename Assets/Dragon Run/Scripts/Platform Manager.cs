using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();  // �÷�����
    public List<Transform> obstacleSpawnPoints = new List<Transform>();  // ��ֹ� ���� ����Ʈ
    public GameObject[] obstaclePrefabs;  // ��ֹ� ������ �迭
    public float platformSpeed = 5f;  // �÷��� �̵� �ӵ�
    public float screenBoundary = 15f;  // ȭ�� ������ ������ ����
    public float distanceBetweenSpawns = 2f;  // ��ֹ� �� �Ÿ�
    public float spawnInterval = 3f;  // ���� ����
    public float minObstacleDistance = 5f;  // ��ֹ� �� �ּ� �Ÿ�

    private float spawnTimer = 0f;
    private List<GameObject> activeObstacles = new List<GameObject>(); // Ȱ��ȭ�� ��ֹ��� ����Ʈ

    void Start()
    {
        // �ʱ� ���� Ÿ�̸� ����
        spawnTimer = spawnInterval;

        // �÷����� ù ��° ��ֹ� ����
        for (int i = 0; i < platforms.Count; i++)
        {
            SpawnObstacle(i);  // �� �÷����� ù ��ֹ� ����
        }
    }

    void Update()
    {
        // ��� �÷����� �̵���Ű�� �ʿ��� ��� ��ֹ� ����
        for (int i = 0; i < platforms.Count; i++)
        {
            MovePlatform(platforms[i], i);
        }

        // ���� Ÿ�̸� ����
        spawnTimer -= Time.deltaTime;

        // ���� �ð� �������� ��ֹ� ����
        if (spawnTimer <= 0)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                // �� �÷������� ��ֹ��� ������ ������ �����Ǵ��� Ȯ�� �� ����
                if (CanSpawnObstacle(i))
                {
                    SpawnObstacle(i);
                }
            }
            spawnTimer = spawnInterval;  // Ÿ�̸� ����
        }
    }

    // �÷����� �̵���Ű�� ȭ�� ������ ������ �ٽ� ���������� ��ġ
    void MovePlatform(GameObject platform, int index)
    {
        // �÷����� �������� �̵�
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        // �÷����� ȭ�� ���� ���� �Ѿ�� ���ġ
        if (platform.transform.position.x < -screenBoundary)
        {
            // �÷��� ���� ��� ��ֹ� ����
            RemoveObstaclesOnPlatform(platform);

            // �÷����� �ٽ� ������ ������ �̵� (ȭ�� ������)
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);

            // ���ġ �� ��ֹ� ����
            if (CanSpawnObstacle(index))
            {
                SpawnObstacle(index);
            }
        }
    }

    // ��ֹ� ���� ���� ���� üũ (������ ��ֹ��� ���� �Ÿ� �̻��� ���� ����)
    bool CanSpawnObstacle(int platformIndex)
    {
        // Ȱ��ȭ�� ��ֹ��� �ִٸ� ������ ��ֹ����� �Ÿ��� üũ
        if (activeObstacles.Count > 0)
        {
            GameObject lastObstacle = activeObstacles[activeObstacles.Count - 1];
            float distanceFromLastObstacle = Vector3.Distance(lastObstacle.transform.position, obstacleSpawnPoints[platformIndex].position);

            // �ּ� �Ÿ� ������ �����ؾ� ���� ����
            return distanceFromLastObstacle >= minObstacleDistance;
        }

        // ��ֹ��� ������ �ٷ� ���� ����
        return true;
    }

    // �÷��� ���� ��ֹ� ����
    void RemoveObstaclesOnPlatform(GameObject platform)
    {
        // �÷����� �ڽ� �� "Obstacle" �±׸� ���� ��ֹ����� ����
        List<Transform> obstaclesToRemove = new List<Transform>();

        foreach (Transform child in platform.transform)
        {
            if (child.CompareTag("Obstacle"))
            {
                obstaclesToRemove.Add(child);
            }
        }

        // ��ֹ� ���� �� Ȱ�� ��ֹ� ����Ʈ���� ����
        foreach (Transform obstacle in obstaclesToRemove)
        {
            activeObstacles.Remove(obstacle.gameObject);
            Destroy(obstacle.gameObject);
        }

        // Ȱ��ȭ�� ��ֹ� ����Ʈ ���� (null �� ����)
        activeObstacles.RemoveAll(obstacle => obstacle == null);
    }

    // ��ֹ� ���� �Լ�
    void SpawnObstacle(int platformIndex)
    {
        // ���� ����Ʈ�� ��ȿ���� Ȯ��
        if (obstacleSpawnPoints[platformIndex] == null)
        {
            Debug.LogError("Spawn point is null for platform " + platformIndex);
            return;
        }

        // ��ֹ� ������ �� �ϳ��� �������� ����
        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // ���� ��ġ ���� (�ణ�� ���� ������ �߰�)
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = obstacleSpawnPoints[platformIndex].position + new Vector3(randomXOffset, 0, 0);

        // ��ֹ� ����
        GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

        // ������ ��ֹ��� �ش� �÷����� �ڽ����� ����
        obstacle.transform.SetParent(platforms[platformIndex].transform);

        // Ȱ��ȭ�� ��ֹ� ����Ʈ�� �߰�
        activeObstacles.Add(obstacle);
    }
}
