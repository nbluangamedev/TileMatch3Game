using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public void OnClickedPlayNextLevel()
    {
        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.CurrentLevel < 3)
            {
                GameManager.Instance.UpdateLevel(GameManager.Instance.CurrentLevel + 1);
                GameManager.Instance.ChangeScene("Level");
            }
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveWinPanel(false);
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