using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    
    private async UniTask Start()
    {
        await UniTask.Yield();
        _uiManager.ShowScreen<LoadingScreen>();
        await UniTask.WaitForSeconds(0.5f);
        _uiManager.ShowScreen<RhythmTapScreen>();
        _uiManager.HideScreen<LoadingScreen>();
    }
}