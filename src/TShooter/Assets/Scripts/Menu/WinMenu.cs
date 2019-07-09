using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] GameObject WinMenuPanel;
    [SerializeField] Text WinMenuPanelText;
    [SerializeField] public Button BackToMenuButton;

    public string MainMenuSceneName;
    public string LobbySceneName;

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

        GameManager.Instance.EventBus.AddListener("OnBlueTeamWin", () =>
        {
            GameManager.Instance.Timer.Add(() =>
            {
                GameManager.Instance.IsPaused = true;
                WinMenuPanelText.text = "Winner: Team Blue";
                WinMenuPanel.SetActive(true);
            }, 2);
        });

        GameManager.Instance.EventBus.AddListener("OnRedTeamWin", () =>
        {
            GameManager.Instance.Timer.Add(() =>
            {
                GameManager.Instance.IsPaused = true;
                WinMenuPanelText.text = "Winner: Team Red";
                WinMenuPanel.SetActive(true);
            }, 2);
        });
    }

    public void BackToMenu()
    {
        try
        {
            GameManager.Instance.Timer.Clear();
            Application.Quit();
            //SceneManager.LoadScene(LobbySceneName);
        }
        catch (System.Exception ex)
        {
            Application.Quit();
        }
    }
}
