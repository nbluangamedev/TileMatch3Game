using UnityEngine;

public class CheckHaveTile : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        Tile tile = other.gameObject.GetComponent<Tile>();
        if (tile == null)
        {
            Debug.Log("You win this level");
        }
    }
}