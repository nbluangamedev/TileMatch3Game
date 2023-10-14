using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer tileRenderer;

    private void Start()
    {
        tileRenderer = GetComponent<Renderer>();
    }

    private void OnMouseEnter()
    {
        tileRenderer.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        tileRenderer.material.color = Color.white;
    }
}