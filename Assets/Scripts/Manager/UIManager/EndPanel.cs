using UnityEngine;

public class EndPanel : MonoBehaviour
{
    public void OnClickedExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}