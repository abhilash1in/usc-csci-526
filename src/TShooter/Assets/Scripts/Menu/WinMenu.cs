using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] GameObject WinMenuPanel;
    [SerializeField] public Button BackToMenuButton;

    public string MainMenuSceneName;

    private void Start()
    {
        WinMenuPanel.SetActive(false);
        BackToMenuButton.onClick.AddListener(() => { BackToMenu(); });
        GameManager.Instance.EventBus.AddListener("OnAllEnemiesKilled", () =>
        {
            GameManager.Instance.Timer.Add(() =>
            {
                GameManager.Instance.IsPaused = true;
                WinMenuPanel.SetActive(true);
            }, 4);
        });
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
