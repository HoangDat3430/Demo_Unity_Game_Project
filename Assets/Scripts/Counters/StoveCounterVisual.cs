using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burningProcessAmount = .5f;
        if (burningProcessAmount < e.progressNormalized && stoveCounter.isFried())
        {
            SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
        }
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            Show();
            audioSource.Play();
        }
        else
        {
            Hide();
            audioSource.Stop();
        }
    }


    private void Show()
    {
        foreach (GameObject gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach(GameObject gameObject in visualGameObjectArray)
        {
            gameObject.SetActive(false);
        }
    }
}
