using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownUI : MonoBehaviour
{
    private const string COUNTDOWN_NUMBER = "CountdownNumber";

    [SerializeField] private TextMeshProUGUI countdownText;


    private GameHandler.State currentState;
    private Animator animator;
    
    private int previousCountdownNumber;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentState = GameHandler.Instance.GetState();
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        if (currentState == GameHandler.State.countdownToStart)
        {
            int countdownNumber = Mathf.CeilToInt(GameHandler.Instance.GetCountDownTime());
            countdownText.text = countdownNumber.ToString();
            if(countdownNumber != previousCountdownNumber) { 
                previousCountdownNumber = countdownNumber;
                animator.SetTrigger(COUNTDOWN_NUMBER);
                SoundManager.Instance.PlayCountdownSound();
            }
        }
        else if(currentState == GameHandler.State.onStart)
        {
            countdownText.text = "START!!!";
            countdownText.color = Color.yellow;
            SoundManager.Instance.PlayStartSound();
        }
    }
    private void GameHandler_OnStateChanged(object sender, System.EventArgs e)
    {
        currentState = GameHandler.Instance.GetState();
        if (currentState == GameHandler.State.countdownToStart || currentState == GameHandler.State.onStart || currentState == GameHandler.State.waitingToStart)
        {
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
