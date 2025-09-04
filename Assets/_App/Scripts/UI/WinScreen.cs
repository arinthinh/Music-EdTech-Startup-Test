using UnityEngine;
using UnityEngine.UI;

public class WinScreen : UIScreen
{
    [SerializeField] private Button _replayButton;

    private void OnEnable()
    {
        _replayButton.onClick.AddListener(OnReplayButtonClicked);
    }

    private void OnDisable()
    {
        _replayButton.onClick.RemoveListener(OnReplayButtonClicked);
    }

    private void OnReplayButtonClicked()
    {
       
    }
}