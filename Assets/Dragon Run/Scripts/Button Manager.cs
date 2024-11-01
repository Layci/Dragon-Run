using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject helpPanel;
    public GameObject pausePanel;

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

    public void StartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void HelpBtn()
    {
        helpPanel.SetActive(true);
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
    }

    public void PauseBtn()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartingBtn()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
