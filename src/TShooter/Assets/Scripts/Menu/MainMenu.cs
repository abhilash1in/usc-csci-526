using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public Button StartGameButton;
    [SerializeField] public Button QuitGameButton;
    public string levelName;

    private void Start()
    {
        StartGameButton.onClick.AddListener(() => { StartGame(); });
        StartGameButton.onClick.AddListener(() => { QuitGame(); });
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
