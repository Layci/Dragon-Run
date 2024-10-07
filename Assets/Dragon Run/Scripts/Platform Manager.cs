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

        // **장애물이 일정 시간마다 생성되도록 추가 조건**
        if (Time.time >= nextSpawnTime)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                SpawnObstacle(i);
            }
            // **다음 스폰 시간을 업데이트**
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnObstacle(int platformIndex)
    {
        // 배열 범위 초과 방지
        if (platformIndex >= obstacleSpawnPoints.Length || platformIndex >= platforms.Length)
        {
            Debug.LogError("platformIndex out of bounds.");
            return;
        }

        // **플랫폼과 스폰 포인트가 null인지 확인**
        if (obstacleSpawnPoints[platformIndex] == null || platforms[platformIndex] == null)
        {
            Debug.LogWarning("Spawn point or platform is null. Skipping obstacle spawn.");
            return;
        }

        // 현재 스폰 포인트 위치
        Vector3 currentSpawnPoint = obstacleSpawnPoints[platformIndex].position;

        // 마지막 장애물과 현재 스폰 포인트 사이의 거리 계산
        float distanceFromLastObstacle = Vector3.Distance(lastObstaclePosition, currentSpawnPoint);

        // **최소 간격을 충족하지 않으면 리턴**
        if (distanceFromLastObstacle < minObstacleDistance)
        {
            Debug.LogWarning("Not enough distance from the last obstacle. Skipping spawn.");
            return;
        }

        // **장애물 프리팹을 무작위로 선택**
        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 스폰 위치에 약간의 랜덤 X 오프셋 추가
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = currentSpawnPoint + new Vector3(randomXOffset, 0, 0);

        // **플랫폼이 null인지 확인 후 장애물 생성**
        if (platforms[platformIndex] != null)
        {
            // **장애물 생성**
            GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

            // **생성된 장애물을 해당 플랫폼의 자식으로 설정**
            obstacle.transform.parent = platforms[platformIndex].transform;

            // **마지막으로 스폰된 장애물 위치를 업데이트**
            lastObstaclePosition = spawnPosition;
        }
    }

    void MovePlatform(GameObject platform, int index)
    {
        // 플랫폼 이동 (왼쪽으로 이동)
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        // 플랫폼이 화면 왼쪽 끝을 넘어가면 재배치
        if (platform.transform.position.x < -screenBoundary)
        {
            // **플랫폼에 속한 장애물들 삭제**
            RemoveObstacles(platform);

            // 플랫폼을 다시 오른쪽 끝으로 이동
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);
        }
    }

    // 플랫폼에 속한 자식 오브젝트(장애물)를 모두 삭제하는 함수
    void RemoveObstacles(GameObject platform)
    {
        foreach (Transform child in platform.transform)
        {
            if (child != null)  // **자식 오브젝트가 null인지 확인**
            {
                // 자식 오브젝트가 있으면 삭제
                Destroy(child.gameObject);
            }
        }
    }
}
