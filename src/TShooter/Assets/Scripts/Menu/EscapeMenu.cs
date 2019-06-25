using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] bool EnableEscapeMenu;
    [SerializeField] GameObject EscapeMenuPanel;
    [SerializeField] Button YesButton;
    [SerializeField] Button NoButton;

    public string MainMenuSceneName;

    private void Start()
    {
        EscapeMenuPanel.SetActive(false);
        YesButton.onClick.AddListener(OnYesClicked);
        NoButton.onClick.AddListener(OnNoClicked);
    }

    void OnYesClicked()
    {
        SceneManager.LoadScene(MainMenuSceneName);
        //GameManager.Instance.Destroy();
    }

    void OnNoClicked()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.IsPaused = false;
        EscapeMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (!EnableEscapeMenu)
            return;
        if (EscapeMenuPanel.activeSelf)
            return;
        if (GameManager.Instance.InputController.Escape)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            GameManager.Instance.IsPaused = true;
            EscapeMenuPanel.SetActive(true);
        }
    }
}
