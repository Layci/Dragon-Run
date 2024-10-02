using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float initialSpeed = 5f;           // �ʱ� �÷��� �ӵ�
    public float speedIncreaseRate = 0.1f;    // �ð��� ���� �ӵ� ���� ����
    public float resetPositionX = 10f;        // ������ ���ġ�� X ��ǥ
    public float leftBoundary = -10f;         // ������ ����� ���� �� ��ǥ
    public GameObject[] obstaclePrefabs;      // ���� ���� ��ֹ� ������ �迭
    public Transform obstacleSpawnPoint;      // ��ֹ� ���� ��ġ
    public float distanceBetweenSpawns = 5f;  // ��ֹ� ���� ���� (�Ÿ�)

    private float lastSpawnX;                 // ���������� ��ֹ��� ������ X ��ǥ
    private float currentSpeed;               // ���� �ӵ�

    void Start()
    {
        currentSpeed = initialSpeed;          // �ʱ� �ӵ��� ����
        lastSpawnX = transform.position.x;    // ���� ��ġ�� �ʱ� ���� ��ġ�� ����
    }

    void Update()
    {
        // �ð��� ���� �÷��� �ӵ� ����
        currentSpeed += speedIncreaseRate * Time.deltaTime;

        // ������ �������� �̵���Ŵ (�ӵ��� ����)
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        // ������ ���� �� ��ǥ�� �����ϸ� ���ġ
        if (transform.position.x <= leftBoundary)
        {
            // ������ ������ �� ��ǥ�� ���ġ
            Vector3 newPosition = new Vector3(resetPositionX, transform.position.y, transform.position.z);
            transform.position = newPosition;

            // ���� ��ġ�� �ʱ�ȭ
            lastSpawnX = transform.position.x;
        }

        // ���� �Ÿ� �̵� �� ��ֹ� ����
        if (Mathf.Abs(transform.position.x - lastSpawnX) >= distanceBetweenSpawns)
        {
            SpawnObstacle();
            lastSpawnX = transform.position.x;  // ���ο� ��ֹ� ���� �� ��ġ ����
        }
    }

    // ��ֹ��� �����ϴ� �Լ�
    void SpawnObstacle()
    {
        // ������ ��ֹ� ������ ����
        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // ������ X ��ǥ�� ��ֹ� ����
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = obstacleSpawnPoint.position + new Vector3(randomXOffset, 0, 0);

        // ��ֹ� ����
        GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

        // ��ֹ��� �÷����� �ڽ����� �����Ͽ� �Բ� �̵��ϰ� ��
        obstacle.transform.parent = transform;
    }
}
