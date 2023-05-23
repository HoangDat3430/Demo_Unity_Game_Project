using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeAmountText;
    [SerializeField] private TextMeshProUGUI correctRecipeDeliveredText;


    private GameHandler.State currentState;
    private void Start()
    {
        currentState = GameHandler.Instance.GetState();
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Hide();
    }

    private void Update()
    {
    }
    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        currentState = GameHandler.Instance.GetState();
        if (currentState == GameHandler.State.gameOver)
        {
            recipeAmountText.text = DeliveryManager.Instance.GetRecipeSuccessAmount().ToString();
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

}
