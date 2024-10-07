using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] platforms; // �÷��� �迭
    public GameObject[] obstaclePrefabs; // ��ֹ� ������ �迭
    public Transform[] obstacleSpawnPoints; // ��ֹ� ���� ����Ʈ �迭

    public float platformSpeed = 5f; // �÷��� �̵� �ӵ�
    public float screenBoundary = 10f; // ȭ�� ������ ������ ���
    public float distanceBetweenSpawns = 5f; // ��ֹ� ����
    public float speedIncreaseRate = 0.1f; // �ӵ� ������

    public float spawnInterval = 5f; // ��ֹ� ���� ����
    private float nextSpawnTime = 0f;

    // ���ο� ����: ���������� ��ֹ��� ������ ��ġ
    private Vector3 lastObstaclePosition;

    // �ּ� ��ֹ� ����
    public float minObstacleDistance = 3f;

    void Start()
    {
        // nextSpawnTime �ʱ�ȭ
        nextSpawnTime = Time.time + spawnInterval;

        // ������ ��ֹ� ��ġ�� �ʱ�ȭ
        lastObstaclePosition = new Vector3(-Mathf.Infinity, 0, 0); // ó������ ���� �� ��ġ�� ����
    }

    void Update()
    {
        // ��� �÷����� �̵���Ű��, ���ġ�� �ʿ��� ��� ó��
        for (int i = 0; i < platforms.Length; i++)
        {
            MovePlatform(platforms[i], i);
        }

        // ���� ���࿡ ���� �÷��� �ӵ� ����
        platformSpeed += speedIncreaseRate * Time.deltaTime;

        // **��ֹ��� ���� �ð����� �����ǵ��� �߰� ����**
        if (Time.time >= nextSpawnTime)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                SpawnObstacle(i);
            }
            // **���� ���� �ð��� ������Ʈ**
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObstacle(int platformIndex)
    {
        // �迭 ���� �ʰ� ����
        if (platformIndex >= obstacleSpawnPoints.Length || platformIndex >= platforms.Length)
        {
            Debug.LogError("platformIndex out of bounds.");
            return;
        }

        // **�÷����� ���� ����Ʈ�� null���� Ȯ��**
        if (obstacleSpawnPoints[platformIndex] == null || platforms[platformIndex] == null)
        {
            Debug.LogWarning("Spawn point or platform is null. Skipping obstacle spawn.");
            return;
        }

        // ���� ���� ����Ʈ ��ġ
        Vector3 currentSpawnPoint = obstacleSpawnPoints[platformIndex].position;

        // ������ ��ֹ��� ���� ���� ����Ʈ ������ �Ÿ� ���
        float distanceFromLastObstacle = Vector3.Distance(lastObstaclePosition, currentSpawnPoint);

        // **�ּ� ������ �������� ������ ����**
        if (distanceFromLastObstacle < minObstacleDistance)
        {
            Debug.LogWarning("Not enough distance from the last obstacle. Skipping spawn.");
            return;
        }

        // **��ֹ� �������� �������� ����**
        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // ���� ��ġ�� �ణ�� ���� X ������ �߰�
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = currentSpawnPoint + new Vector3(randomXOffset, 0, 0);

        // **�÷����� null���� Ȯ�� �� ��ֹ� ����**
        if (platforms[platformIndex] != null)
        {
            // **��ֹ� ����**
            GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

            // **������ ��ֹ��� �ش� �÷����� �ڽ����� ����**
            obstacle.transform.parent = platforms[platformIndex].transform;

            // **���������� ������ ��ֹ� ��ġ�� ������Ʈ**
            lastObstaclePosition = spawnPosition;
        }
    }

    void MovePlatform(GameObject platform, int index)
    {
        // �÷��� �̵� (�������� �̵�)
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        // �÷����� ȭ�� ���� ���� �Ѿ�� ���ġ
        if (platform.transform.position.x < -screenBoundary)
        {
            // **�÷����� ���� ��ֹ��� ����**
            RemoveObstacles(platform);

            // �÷����� �ٽ� ������ ������ �̵�
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);
        }
    }

    // �÷����� ���� �ڽ� ������Ʈ(��ֹ�)�� ��� �����ϴ� �Լ�
    void RemoveObstacles(GameObject platform)
    {
        foreach (Transform child in platform.transform)
        {
            if (child != null)  // **�ڽ� ������Ʈ�� null���� Ȯ��**
            {
                // �ڽ� ������Ʈ�� ������ ����
                Destroy(child.gameObject);
            }
        }
    }
}
