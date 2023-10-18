using UnityEngine;

public class Tile : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_PICKUP);
        }
    }
}