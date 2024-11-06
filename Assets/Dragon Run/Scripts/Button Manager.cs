using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject pausePanel;
    bool pause = false;
    bool helpView = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !helpView)
        {
            if (!pause)
            {
                PauseBtn();
            }
            else if (pause)
            {
                StartingBtn();
            }
        }
    }

    public void StartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HelpBtn()
    {
        helpPanel.SetActive(true);
        helpView = true;
    }

    public void ExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void CancelHelp()
    {
        helpPanel.SetActive(false);
        helpView = false;
    }

    public void PauseBtn()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        pause = !pause;
        // 버튼을 누른 후 EventSystem의 현재 선택된 객체를 null로 설정
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void StartingBtn()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        pause = !pause;
    }
}
