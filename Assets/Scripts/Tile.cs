using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer tileRenderer;
    //[HideInInspector] public string tileName;

    private void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        //tileName = tileRenderer.material.mainTexture.name;
        gameObject.name = tileRenderer.material.mainTexture.name;
    }

    private void OnMouseEnter()
    {
        tileRenderer.material.color = Color.red;        
    }

    private void OnMouseExit()
    {
        tileRenderer.material.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_PICKUP);
        }
    }
}