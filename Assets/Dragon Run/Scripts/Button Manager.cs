using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject helpPanel;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        helpPanel.SetActive(false);
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
}
