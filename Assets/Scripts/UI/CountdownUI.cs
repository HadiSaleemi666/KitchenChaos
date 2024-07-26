using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private Image timer;

    private void Update()
    {
        timer.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
