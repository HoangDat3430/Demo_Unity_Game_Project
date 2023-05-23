using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliverySuccessUI : MonoBehaviour
{
    private const string POPUP = "Popup";

    [SerializeField] private Image background;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;


    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += DeliveryManager_OnDeliverySuccess;
        DeliveryManager.Instance.OnDeliveryFail += DeliveryManager_OnDeliveryFail;
        Hide();
    }

    private void DeliveryManager_OnDeliveryFail(object sender, System.EventArgs e)
    {
        background.color = Color.red;
        iconImage.sprite = failureSprite;
        messageText.text = "DELIVERY\nFAIL";
        animator.SetBool(POPUP, true);
        Show();
    }

    private void DeliveryManager_OnDeliverySuccess(object sender, System.EventArgs e)
    {
        background.color = Color.green;
        iconImage.sprite = successSprite;
        messageText.text = "DELIVERY\nSUCCESS";
        animator.SetBool(POPUP, true);
        Show();
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
