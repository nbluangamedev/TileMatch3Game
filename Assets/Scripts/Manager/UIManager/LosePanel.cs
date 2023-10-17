using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public void OnClickedMenuButton()
    {
        if (GameManager.HasInstance)
        {
            //GameManager.Instance.RestartGame();
            GameManager.Instance.ChangeScene("Level");
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveLosePanel(false);
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