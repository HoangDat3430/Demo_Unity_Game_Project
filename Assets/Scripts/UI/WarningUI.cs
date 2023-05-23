using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;

    private void Start()
    {
        Hide();
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(stoveCounter.GetState() == StoveCounter.State.Burned)
        {
            Hide();
        }
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burningProcessAmount = .5f;
        if(burningProcessAmount <= e.progressNormalized && stoveCounter.isFried())
        {
            Show();
        }
        else { Hide(); }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
