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
    public float speedIncreaseRate = 0.1f;  // �ӵ� ������
    public float speedIncreaseInterval = 10f;  // �ӵ� ���� ���� (�� ����)

    private float timeSinceLastSpeedIncrease = 0f;  // ������ �ӵ� ���� �� ���� �ð�
    private List<GameObject> activeObstacles = new List<GameObject>(); // Ȱ��ȭ�� ��ֹ��� ����Ʈ

    void Start()
    {
        // ���� ���� ��, ��� �÷����� ó�� �� ���� ����.
        for (int i = 0; i < platforms.Count; i++)
        {
            // �÷����� �ʱ� ��ġ�� ���� ���� �������� ����
        }
    }

    void Update()
    {
        // �÷��� �̵� �� ��ֹ� ���� ó��
        for (int i = 0; i < platforms.Count; i++)
        {
            MovePlatform(platforms[i], i);
        }

        // ���� �ð����� �÷��� �ӵ� ����
        timeSinceLastSpeedIncrease += Time.deltaTime;
        if (timeSinceLastSpeedIncrease >= speedIncreaseInterval)
        {
            IncreasePlatformSpeed();
            timeSinceLastSpeedIncrease = 0f;  // ���� �� �ð� �ʱ�ȭ
        }
    }

    // �÷����� �̵���Ű�� ȭ�� ������ ������ �ٽ� ���������� ��ġ�ϰ� ��ֹ� ����
    void MovePlatform(GameObject platform, int index)
    {
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        if (platform.transform.position.x < -screenBoundary)
        {
            RemoveObstaclesOnPlatform(platform);
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);
            SpawnObstacle(index);
        }
    }

    // �÷��� ���� ��ֹ� ����
    void RemoveObstaclesOnPlatform(GameObject platform)
    {
        List<Transform> obstaclesToRemove = new List<Transform>();

        foreach (Transform child in platform.transform)
        {
            if (child.CompareTag("Obstacle"))
            {
                obstaclesToRemove.Add(child);
            }
        }

        foreach (Transform obstacle in obstaclesToRemove)
        {
            activeObstacles.Remove(obstacle.gameObject);
            Destroy(obstacle.gameObject);
        }

        activeObstacles.RemoveAll(obstacle => obstacle == null);
    }

    // ��ֹ� ���� �Լ�
    void SpawnObstacle(int platformIndex)
    {
        if (obstacleSpawnPoints[platformIndex] == null)
        {
            Debug.LogError("Spawn point is null for platform " + platformIndex);
            return;



        }

        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = obstacleSpawnPoints[platformIndex].position + new Vector3(randomXOffset, 0, 0);

        GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);
        obstacle.transform.SetParent(platforms[platformIndex].transform);
        activeObstacles.Add(obstacle);
    }

    // ���� �ð����� ȣ��Ǵ� �ӵ� ���� �Լ�
    void IncreasePlatformSpeed()
    {
        platformSpeed += speedIncreaseRate;
        Debug.Log("Platform speed increased to: " + platformSpeed);
    }
}
