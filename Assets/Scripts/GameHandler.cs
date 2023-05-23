using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public enum State
    {
        waitingToStart,
        countdownToStart,
        onStart,
        gamePlaying,
        gameOver
    }

    public event EventHandler OnStateChanged;
    public event EventHandler OnTogglePauseChanged;

    public static GameHandler Instance { get; private set; }
    private State state;

    private float countdownTime = 3f;
    private float onStartTime = 1f;
    private float playingTime = 90f;
    private float playingTimeMax = 90f;
    private bool isGamePaused = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        state = State.waitingToStart;
        GameInput.Instance.OnTogglePauseGame += GameInput_OnTogglePauseGame;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(GetState() == State.waitingToStart)
        {
            state = State.countdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnTogglePauseGame(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.waitingToStart:
                break;
            case State.countdownToStart:
                countdownTime -= Time.deltaTime;
                if( countdownTime<= 0f )
                {
                    state = State.onStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.onStart:
                onStartTime -= Time.deltaTime;
                if (onStartTime <= 0f)
                {
                    state = State.gamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.gamePlaying:
                playingTime -= Time.deltaTime;
                if ( playingTime <= 0f )
                {
                    state = State.gameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.gameOver:
                break;
        }
    }

    public State GetState()
    {
        return state;
    }
    public float GetCountDownTime()
    {
        return countdownTime;
    }

    public bool gameIsCountingDown()
    {
        return state == State.countdownToStart;
    }
    public bool gameIsPlaying()
    {
        return state == State.gamePlaying;
    }
    public bool CanMove()
    {
        return state == State.gamePlaying || state == State.onStart || state == State.countdownToStart;
    }
    public float GetPlayingTimeRatio()
    {
        return playingTime/playingTimeMax;
    }

    public bool GameIsPaused()
    {
        return isGamePaused;
    }
    public void TogglePauseGame()
    {
        if (isGamePaused)
        {
            if(!TutorialUI.Instance.gameObject.activeSelf)Time.timeScale = 1;
            isGamePaused = false;
            OnTogglePauseChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 0;
            isGamePaused = true;
            OnTogglePauseChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
