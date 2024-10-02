using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float initialSpeed = 5f;           // 초기 플랫폼 속도
    public float speedIncreaseRate = 0.1f;    // 시간에 따라 속도 증가 비율
    public float resetPositionX = 10f;        // 발판이 재배치될 X 좌표
    public float leftBoundary = -10f;         // 발판이 사라질 왼쪽 끝 좌표
    public GameObject[] obstaclePrefabs;      // 여러 가지 장애물 프리팹 배열
    public Transform obstacleSpawnPoint;      // 장애물 스폰 위치
    public float distanceBetweenSpawns = 5f;  // 장애물 간의 간격 (거리)

    private float lastSpawnX;                 // 마지막으로 장애물이 스폰된 X 좌표
    private float currentSpeed;               // 현재 속도

    void Start()
    {
        currentSpeed = initialSpeed;          // 초기 속도로 설정
        lastSpawnX = transform.position.x;    // 시작 위치를 초기 스폰 위치로 설정
    }

    void Update()
    {
        // 시간에 따라 플랫폼 속도 증가
        currentSpeed += speedIncreaseRate * Time.deltaTime;

        // 발판을 왼쪽으로 이동시킴 (속도가 증가)
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        // 발판이 왼쪽 끝 좌표에 도달하면 재배치
        if (transform.position.x <= leftBoundary)
        {
            // 발판을 오른쪽 끝 좌표로 재배치
            Vector3 newPosition = new Vector3(resetPositionX, transform.position.y, transform.position.z);
            transform.position = newPosition;

            // 스폰 위치도 초기화
            lastSpawnX = transform.position.x;
        }

        // 일정 거리 이동 시 장애물 스폰
        if (Mathf.Abs(transform.position.x - lastSpawnX) >= distanceBetweenSpawns)
        {
            SpawnObstacle();
            lastSpawnX = transform.position.x;  // 새로운 장애물 스폰 후 위치 갱신
        }
    }

    // 장애물을 스폰하는 함수
    void SpawnObstacle()
    {
        // 랜덤한 장애물 프리팹 선택
        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 랜덤한 X 좌표에 장애물 스폰
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = obstacleSpawnPoint.position + new Vector3(randomXOffset, 0, 0);

        // 장애물 생성
        GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

        // 장애물을 플랫폼의 자식으로 설정하여 함께 이동하게 함
        obstacle.transform.parent = transform;
    }
}
