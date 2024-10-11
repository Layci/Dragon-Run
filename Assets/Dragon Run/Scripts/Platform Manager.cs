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
    public float speedIncreaseRate = 0.1f;  // 속도 증가율
    public float speedIncreaseInterval = 10f;  // 속도 증가 간격 (초 단위)

    private float timeSinceLastSpeedIncrease = 0f;  // 마지막 속도 증가 후 지난 시간
    private List<GameObject> activeObstacles = new List<GameObject>(); // 활성화된 장애물들 리스트

    void Start()
    {
        // 게임 시작 시, 모든 플랫폼에 처음 한 번만 스폰.
        for (int i = 0; i < platforms.Count; i++)
        {
            // 플랫폼이 초기 위치에 있을 때는 스폰하지 않음
        }
    }

    void Update()
    {
        // 플랫폼 이동 및 장애물 스폰 처리
        for (int i = 0; i < platforms.Count; i++)
        {
            MovePlatform(platforms[i], i);
        }

        // 일정 시간마다 플랫폼 속도 증가
        timeSinceLastSpeedIncrease += Time.deltaTime;
        if (timeSinceLastSpeedIncrease >= speedIncreaseInterval)
        {
            IncreasePlatformSpeed();
            timeSinceLastSpeedIncrease = 0f;  // 증가 후 시간 초기화
        }
    }

    // 플랫폼을 이동시키고 화면 밖으로 나가면 다시 오른쪽으로 배치하고 장애물 스폰
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

    // 플랫폼 위의 장애물 제거
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

    // 장애물 스폰 함수
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

    // 일정 시간마다 호출되는 속도 증가 함수
    void IncreasePlatformSpeed()
    {
        platformSpeed += speedIncreaseRate;
        Debug.Log("Platform speed increased to: " + platformSpeed);
    }
}
