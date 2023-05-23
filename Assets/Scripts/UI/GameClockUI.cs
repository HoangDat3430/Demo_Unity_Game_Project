using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] Image clockImage;

    private void Update()
    {
        clockImage.fillAmount = GameHandler.Instance.GetPlayingTimeRatio();
    }
}
