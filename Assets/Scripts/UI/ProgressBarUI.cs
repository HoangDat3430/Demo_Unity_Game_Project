using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private UnityEngine.UI.Image barImage;

    private IHasProgress hasProgress;
    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("Game Object" + hasProgressGameObject + "has no progress");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgessChanged;
        barImage.fillAmount = 0f;
        Hide();
    }

    private void Update()
    {
        if (!hasProgressGameObject.GetComponent<BaseCounter>().HasKitchenObject())
        {
            Hide();
        }
    }

    private void HasProgress_OnProgessChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized; 
        if(barImage.fillAmount != 0f && barImage.fillAmount != 1 && hasProgressGameObject.GetComponent<BaseCounter>().HasKitchenObject())
        {
            Show();
        }
        else
        {
            Hide();
        }
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
