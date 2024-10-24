using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float score;
    public float scoreIncreaseRate = 50f;   // 스코어 증가 속도 (1초에 50씩 증가)
    public float baseThreshold = 500f;      // 기본 점수 기준 (예: 500점)
    public float rateIncreaseAmount = 50f;  // 점수 증가율을 올릴 양
    public float thresholdMultiplier = 2f;  // 다음 기준으로 적용할 배수
    private float currentThreshold;         // 현재 점수 기준
    private float timeSinceLastUpdate = 0f; // 마지막 스코어 업데이트 이후 경과 시간
    public float life = 3;                  // 플레이어 목숨
    public Image[] lifeImages;              // 목숨을 나타낼 이미지 배열
    public Text scoreText;                  // 스코어를 나타낼 텍스트

    public static UIManager instance;

    void Start()
    {
        // 처음 점수 기준 설정
        currentThreshold = baseThreshold;

        // 초기 상태에서 목숨 이미지를 전부 활성화
        UpdateLifeImages();

        instance = this;
    }

    private void Update()
    {
        // 스코어를 시간에 비례하여 부드럽게 증가
        score += scoreIncreaseRate * Time.deltaTime;

        // 3자리마다 쉼표를 붙인 스코어 UI 업데이트
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString("N0");

        // 현재 점수가 기준 이상이면 증가율 올리기
        if (score >= currentThreshold)
        {
            IncreaseScoreRate();    
        }
    }

    // 점수 증가율을 올리는 함수
    void IncreaseScoreRate()
    {
        scoreIncreaseRate += rateIncreaseAmount;   // 증가율을 설정한 양만큼 증가

        // 다음 점수 기준을 배율로 설정
        currentThreshold *= thresholdMultiplier;
    }

    // 목숨 이미지를 업데이트하는 함수
    public void UpdateLifeImages()
    {
        // 현재 목숨에 따라 이미지 활성화/비활성화
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < PlayerControl.instance.life)
            {
                lifeImages[i].enabled = true;  // 활성화
            }
            else
            {
                lifeImages[i].enabled = false;  // 비활성화
            }
        }
    }
}
