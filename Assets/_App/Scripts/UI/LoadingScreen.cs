using DG.Tweening;

public class LoadingScreen : UIScreen
{
    public override void Show()
    {
        base.Show();
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, 0.5f);
    }

    public override void Hide()
    {
        _canvasGroup.DOFade(0, 0.5f).OnComplete(() => base.Hide());
    }
}