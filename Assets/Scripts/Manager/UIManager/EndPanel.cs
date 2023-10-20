using UnityEngine;

public class EndPanel : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveEndPanel(false);
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