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
    }

    void MovePlatform(GameObject platform, int index)
    {
        // �÷��� �̵� (�������� �̵�)
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        // �÷����� ȭ�� ���� ���� �Ѿ�� ���ġ
        if (platform.transform.position.x < -screenBoundary)
        {
            // �÷����� �ٽ� ������ ������ �̵�
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);

            // �÷����� ���ġ�� �� ��ֹ� ����
            SpawnObstacle(index);
        }
    }

    void SpawnObstacle(int platformIndex)
    {
        // ���� �������� ��ֹ� ����
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;

            // �迭 ���� �ʰ� ����
            if (platformIndex >= obstacleSpawnPoints.Length || platformIndex >= platforms.Length)
            {
                Debug.LogError("platformIndex out of bounds.");
                return;
            }

            // ������ ��ֹ��� X ��ǥ�� ���� ���� ����Ʈ ������ �Ÿ� ���
            float distanceFromLastObstacle = Vector3.Distance(lastObstaclePosition, obstacleSpawnPoints[platformIndex].position);

            // ���� ������ ��ֹ����� �Ÿ��� �ּ� ���ݺ��� �۴ٸ� ��ֹ� ������ ���� ����
            if (distanceFromLastObstacle < minObstacleDistance)
            {
                // ���� ������ ������ �ʾ����Ƿ� �������� �ʰ� ����
                return;
            }

            // ������ ��ֹ� ����
            GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // ��ֹ� ���� ��ġ (���� ����Ʈ�� �ణ�� X ��ǥ �������� ����)
            float randomXOffset = Random.Range(-2f, 2f);
            Vector3 spawnPosition = obstacleSpawnPoints[platformIndex].position + new Vector3(randomXOffset, 0, 0);

            // ��ֹ� ����
            GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

            // ��ֹ��� �ش� �÷����� �ڽ����� ���� (�÷����� �Բ� �̵�)
            obstacle.transform.parent = platforms[platformIndex].transform;

            // ������ ��ֹ��� ��ġ�� ������Ʈ
            lastObstaclePosition = spawnPosition;
        }
    }
}
