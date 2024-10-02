using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] platforms; // 플랫폼 배열
    public GameObject[] obstaclePrefabs; // 장애물 프리팹 배열
    public Transform[] obstacleSpawnPoints; // 장애물 스폰 포인트 배열

    public float platformSpeed = 5f; // 플랫폼 이동 속도
    public float screenBoundary = 10f; // 화면 밖으로 나가는 경계
    public float distanceBetweenSpawns = 5f; // 장애물 간격
    public float speedIncreaseRate = 0.1f; // 속도 증가율

    public float spawnInterval = 5f; // 장애물 생성 간격
    private float nextSpawnTime = 0f;

    // 새로운 변수: 마지막으로 장애물이 스폰된 위치
    private Vector3 lastObstaclePosition;

    // 최소 장애물 간격
    public float minObstacleDistance = 3f;

    void Start()
    {
        // nextSpawnTime 초기화
        nextSpawnTime = Time.time + spawnInterval;

        // 마지막 장애물 위치를 초기화
        lastObstaclePosition = new Vector3(-Mathf.Infinity, 0, 0); // 처음에는 아주 먼 위치로 설정
    }

    void Update()
    {
        // 모든 플랫폼을 이동시키고, 재배치가 필요한 경우 처리
        for (int i = 0; i < platforms.Length; i++)
        {
            MovePlatform(platforms[i], i);
        }

        // 게임 진행에 따라 플랫폼 속도 증가
        platformSpeed += speedIncreaseRate * Time.deltaTime;
    }

    void MovePlatform(GameObject platform, int index)
    {
        // 플랫폼 이동 (왼쪽으로 이동)
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        // 플랫폼이 화면 왼쪽 끝을 넘어가면 재배치
        if (platform.transform.position.x < -screenBoundary)
        {
            // 플랫폼을 다시 오른쪽 끝으로 이동
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);

            // 플랫폼이 재배치될 때 장애물 생성
            SpawnObstacle(index);
        }
    }

    void SpawnObstacle(int platformIndex)
    {
        // 일정 간격으로 장애물 생성
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;

            // 배열 범위 초과 방지
            if (platformIndex >= obstacleSpawnPoints.Length || platformIndex >= platforms.Length)
            {
                Debug.LogError("platformIndex out of bounds.");
                return;
            }

            // 마지막 장애물의 X 좌표와 현재 스폰 포인트 사이의 거리 계산
            float distanceFromLastObstacle = Vector3.Distance(lastObstaclePosition, obstacleSpawnPoints[platformIndex].position);

            // 만약 마지막 장애물과의 거리가 최소 간격보다 작다면 장애물 스폰을 하지 않음
            if (distanceFromLastObstacle < minObstacleDistance)
            {
                // 일정 간격이 지나지 않았으므로 스폰하지 않고 리턴
                return;
            }

            // 랜덤한 장애물 선택
            GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // 장애물 스폰 위치 (스폰 포인트에 약간의 X 좌표 오프셋을 더함)
            float randomXOffset = Random.Range(-2f, 2f);
            Vector3 spawnPosition = obstacleSpawnPoints[platformIndex].position + new Vector3(randomXOffset, 0, 0);

            // 장애물 생성
            GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

            // 장애물을 해당 플랫폼의 자식으로 설정 (플랫폼과 함께 이동)
            obstacle.transform.parent = platforms[platformIndex].transform;

            // 마지막 장애물의 위치를 업데이트
            lastObstaclePosition = spawnPosition;
        }
    }
}
