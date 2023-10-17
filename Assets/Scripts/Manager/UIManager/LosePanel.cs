using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public void OnClickedTryAgainButton()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveLosePanel(false);
            UIManager.Instance.ActiveLoadingPanel(true);
        }
    }

    public void OnClickedExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}