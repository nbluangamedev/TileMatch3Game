using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public void OnClickedPlayNextLevel()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveWinPanel(false);
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