using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public float score;
    public float scoreIncreaseRate = 50f;   // ���ھ� ���� �ӵ� (1�ʿ� 50�� ����)
    public float baseThreshold = 500f;      // �⺻ ���� ���� (��: 500��)
    public float rateIncreaseAmount = 50f;  // ���� �������� �ø� ��
    public float thresholdMultiplier = 2f;  // ���� �������� ������ ���
    private float currentThreshold;         // ���� ���� ����
    private float timeSinceLastUpdate = 0f; // ������ ���ھ� ������Ʈ ���� ��� �ð�
    public Image cooldownImage;             // ��Ÿ���� ǥ���� Image ������Ʈ
    public float cooldownTime = 5f;         // ��Ÿ�� ���� �ð�
    private float cooldownTimer = 0f;
    public bool isCooldown = false;
    public float life = 3;                  // �÷��̾� ���
    public Image[] lifeImages;              // ����� ��Ÿ�� �̹��� �迭
    public Text scoreText;                  // ���ھ ��Ÿ�� �ؽ�Ʈ
    public Text gameScoreText;              // ���ӿ��� �гο� ��Ÿ�� �ؽ�Ʈ
    public GameObject gameOverPanel;        // ���ӿ����� ��Ÿ�� �г�

    public static UIManager instance;

    void Start()
    {
        // ó�� ���� ���� ����
        currentThreshold = baseThreshold;

        // �ʱ� ���¿��� ��� �̹����� ���� Ȱ��ȭ
        UpdateLifeImages();

        instance = this;

        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);

        StartCooldown();
    }

    private void Update()
    {
        // ���ھ �ð��� ����Ͽ� ����
        score += scoreIncreaseRate * Time.deltaTime;

        // 3�ڸ����� ��ǥ�� ���� ���ھ� UI ������Ʈ
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString("N0");

        // 3�ڸ����� ��ǥ�� ���� ���ھ� UI ������Ʈ
        gameScoreText.text = "Score: " + Mathf.FloorToInt(score).ToString("N0");

        // ���� ������ ���� �̻��̸� ������ �ø���
        if (score >= currentThreshold)
        {
            IncreaseScoreRate();    
        }

        // ��Ÿ���� Ȱ��ȭ�� ��� Ÿ�̸� ������Ʈ
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = cooldownTimer / cooldownTime;

            // ��Ÿ���� �Ϸ�Ǹ� �ʱ�ȭ
            if (cooldownTimer <= 0f)
            {
                isCooldown = false;
                cooldownImage.fillAmount = 0f;  // �̹��� �ʱ�ȭ
                PlayerControl.instance.isFire = true;
                Debug.Log("��Ÿ�ӳ�");
            }
        }
    }

    // ���� �������� �ø��� �Լ�
    void IncreaseScoreRate()
    {
        scoreIncreaseRate += rateIncreaseAmount;   // �������� ������ �縸ŭ ����

        // ���� ���� ������ ������ ����
        currentThreshold *= thresholdMultiplier;
    }

    // ��� �̹����� ������Ʈ�ϴ� �Լ�
    public void UpdateLifeImages()
    {
        // ���� ����� ���� �̹��� Ȱ��ȭ/��Ȱ��ȭ
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < PlayerControl.instance.life)
            {
                lifeImages[i].enabled = true;  // Ȱ��ȭ
            }
            else
            {
                lifeImages[i].enabled = false;  // ��Ȱ��ȭ
            }
        }
    }

    // ��Ÿ�� ���� �Լ�
    public void StartCooldown()
    {
        if (!isCooldown)
        {
            isCooldown = true;
            cooldownTimer = cooldownTime;
            cooldownImage.fillAmount = 1f;  // �̹��� ��ü ä���
            Debug.Log("��Ÿ�� ����");
        }
    }

    // ���ӿ����� �����ϴ� �Լ�
    public void GameOver()
    {
        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
    }

    public void ReStartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HomeBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
