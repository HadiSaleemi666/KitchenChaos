using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUp;
    [SerializeField] private TextMeshProUGUI keyMoveDown;
    [SerializeField] private TextMeshProUGUI keyMoveLeft;
    [SerializeField] private TextMeshProUGUI keyMoveRight;
    [SerializeField] private TextMeshProUGUI keyInteract;
    [SerializeField] private TextMeshProUGUI keyInteractAlternate;
    [SerializeField] private TextMeshProUGUI keyPause;
    [SerializeField] private TextMeshProUGUI keyGamepadInteract;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternate;
    [SerializeField] private TextMeshProUGUI keyGamepadPause;

    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;
        UpdateVisual();
        Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountDownTimerActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUp.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDown.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeft.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRight.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteract.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyInteractAlternate.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        keyPause.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyGamepadInteract.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        keyGamepadInteractAlternate.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        keyGamepadPause.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);
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
