using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;

    public static GamePauseUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        mainMenuButton.onClick.AddListener(() => { Loader.LoadScene(Loader.Scene.MainMenuScene); });
        resumeButton.onClick.AddListener(() => { GameHandler.Instance.TogglePauseGame(); });
        optionsButton.onClick.AddListener(() => { 
            Hide();
            OptionsUI.Instance.Show();
        });
    }
    private void Start()
    {
        Hide();
        GameHandler.Instance.OnTogglePauseChanged += GameHandler_OnTogglePauseChanged;
    }

    private void GameHandler_OnTogglePauseChanged(object sender, System.EventArgs e)
    {
        if (GameHandler.Instance.GameIsPaused())
        {
            if (OptionsUI.Instance.gameObject.activeSelf) { OptionsUI.Instance.Hide(); }
            else { Show(); }
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
