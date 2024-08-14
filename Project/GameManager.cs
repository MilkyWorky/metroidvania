using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    private void Start()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_MAIN_MENU);
        }
    }

    public void StartGame()
    {
        isPlaying = true;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (isPlaying)
        {
            isPlaying = false;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            Time.timeScale = 1f;
        }
    }

    public void RestarGame()
    {
        if (UIController.HasInstance)
        {
            UIController.Instance.ActiveWinPanel(false);
            UIController.Instance.ActiveGamePanel(false);
            UIController.Instance.ActiveLosePanel(false);
            UIController.Instance.ActivePausePanel(false);
            UIController.Instance.ActiveSettingPanel(false);
            UIController.Instance.ActiveMenuPanel(true);
        }

        ChangeScene("Menu");
        PlayerController playerController = FindObjectOfType<PlayerController>();
        RespawnController respawnController = FindObjectOfType<RespawnController>();
        Destroy(playerController.gameObject);
        Destroy(respawnController.gameObject);
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
