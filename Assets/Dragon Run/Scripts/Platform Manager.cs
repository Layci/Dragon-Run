using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<GameObject> platforms = new List<GameObject>();  // 플랫폼들
    public List<Transform> obstacleSpawnPoints = new List<Transform>();  // 장애물 스폰 포인트
    public GameObject[] obstaclePrefabs;  // 장애물 프리팹 배열
    public float platformSpeed = 5f;  // 플랫폼 이동 속도
    public float screenBoundary = 15f;  // 화면 밖으로 나가는 기준
    public float distanceBetweenSpawns = 2f;  // 장애물 간 거리
    public float spawnInterval = 3f;  // 스폰 간격
    public float minObstacleDistance = 5f;  // 장애물 간 최소 거리

    private float spawnTimer = 0f;
    private List<GameObject> activeObstacles = new List<GameObject>(); // 활성화된 장애물들 리스트

    void Start()
    {
        // 초기 스폰 타이머 설정
        spawnTimer = spawnInterval;

        // 플랫폼당 첫 번째 장애물 스폰
        for (int i = 0; i < platforms.Count; i++)
        {
            SpawnObstacle(i);  // 각 플랫폼에 첫 장애물 스폰
        }
    }

    void Update()
    {
        // 모든 플랫폼을 이동시키고 필요한 경우 장애물 스폰
        for (int i = 0; i < platforms.Count; i++)
        {
            MovePlatform(platforms[i], i);
        }

        // 스폰 타이머 감소
        spawnTimer -= Time.deltaTime;

        // 일정 시간 간격으로 장애물 스폰
        if (spawnTimer <= 0)
        {
            for (int i = 0; i < platforms.Count; i++)
            {
                // 각 플랫폼에서 장애물이 스폰될 조건이 충족되는지 확인 후 스폰
                if (CanSpawnObstacle(i))
                {
                    SpawnObstacle(i);
                }
            }
            spawnTimer = spawnInterval;  // 타이머 리셋
        }
    }

    // 플랫폼을 이동시키고 화면 밖으로 나가면 다시 오른쪽으로 배치
    void MovePlatform(GameObject platform, int index)
    {
        // 플랫폼을 왼쪽으로 이동
        platform.transform.position += Vector3.left * platformSpeed * Time.deltaTime;

        // 플랫폼이 화면 왼쪽 끝을 넘어가면 재배치
        if (platform.transform.position.x < -screenBoundary)
        {
            // 플랫폼 위의 모든 장애물 제거
            RemoveObstaclesOnPlatform(platform);

            // 플랫폼을 다시 오른쪽 끝으로 이동 (화면 밖으로)
            platform.transform.position = new Vector3(screenBoundary, platform.transform.position.y, platform.transform.position.z);

            // 재배치 후 장애물 스폰
            if (CanSpawnObstacle(index))
            {
                SpawnObstacle(index);
            }
        }
    }

    // 장애물 스폰 가능 여부 체크 (마지막 장애물과 일정 거리 이상일 때만 스폰)
    bool CanSpawnObstacle(int platformIndex)
    {
        // 활성화된 장애물이 있다면 마지막 장애물과의 거리를 체크
        if (activeObstacles.Count > 0)
        {
            GameObject lastObstacle = activeObstacles[activeObstacles.Count - 1];
            float distanceFromLastObstacle = Vector3.Distance(lastObstacle.transform.position, obstacleSpawnPoints[platformIndex].position);

            // 최소 거리 조건을 만족해야 스폰 가능
            return distanceFromLastObstacle >= minObstacleDistance;
        }

        // 장애물이 없으면 바로 스폰 가능
        return true;
    }

    // 플랫폼 위의 장애물 제거
    void RemoveObstaclesOnPlatform(GameObject platform)
    {
        // 플랫폼의 자식 중 "Obstacle" 태그를 가진 장애물들을 제거
        List<Transform> obstaclesToRemove = new List<Transform>();

        foreach (Transform child in platform.transform)
        {
            if (child.CompareTag("Obstacle"))
            {
                obstaclesToRemove.Add(child);
            }
        }

        // 장애물 제거 및 활성 장애물 리스트에서 제거
        foreach (Transform obstacle in obstaclesToRemove)
        {
            activeObstacles.Remove(obstacle.gameObject);
            Destroy(obstacle.gameObject);
        }

        // 활성화된 장애물 리스트 갱신 (null 값 제거)
        activeObstacles.RemoveAll(obstacle => obstacle == null);
    }

    // 장애물 스폰 함수
    void SpawnObstacle(int platformIndex)
    {
        // 스폰 포인트가 유효한지 확인
        if (obstacleSpawnPoints[platformIndex] == null)
        {
            Debug.LogError("Spawn point is null for platform " + platformIndex);
            return;
        }

        // 장애물 프리팹 중 하나를 랜덤으로 선택
        GameObject randomObstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 스폰 위치 설정 (약간의 랜덤 오프셋 추가)
        float randomXOffset = Random.Range(-2f, 2f);
        Vector3 spawnPosition = obstacleSpawnPoints[platformIndex].position + new Vector3(randomXOffset, 0, 0);

        // 장애물 생성
        GameObject obstacle = Instantiate(randomObstaclePrefab, spawnPosition, Quaternion.identity);

        // 생성된 장애물을 해당 플랫폼의 자식으로 설정
        obstacle.transform.SetParent(platforms[platformIndex].transform);

        // 활성화된 장애물 리스트에 추가
        activeObstacles.Add(obstacle);
    }
}
