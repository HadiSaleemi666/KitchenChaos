using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisualBehavior : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] gameObjectVisualArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounter += Player_OnSelectedCounter;
    }

    private void Player_OnSelectedCounter(object sender, Player.OnSelectedCounterEventArgs e)
    {
        if (e.selectedCounter == baseCounter)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        foreach (GameObject gameObjectVisual in gameObjectVisualArray)
            gameObjectVisual.SetActive(true);
    }

    private void Hide()
    {
        foreach (GameObject gameObjectVisual in gameObjectVisualArray)
            gameObjectVisual.SetActive(false);
    }
}
