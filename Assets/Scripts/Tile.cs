using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer tileRenderer;
    public string tileName;

    private void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        tileName = tileRenderer.material.mainTexture.name;
        gameObject.name = tileName;
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