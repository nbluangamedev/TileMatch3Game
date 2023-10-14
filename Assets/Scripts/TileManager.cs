using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public Level_ScriptableObject level_ScriptableObject;

    [SerializeField] private Transform initPosition;
    [SerializeField] private Transform[] queuePosition;

    private bool canPlay = false;
    private int randomTextureIndex;
    private int textureNumber;
    private int numberTileSpawn;
    private int tileTotal;
    private readonly List<int> numberTextures = new();

    private void Start()
    {
        textureNumber = level_ScriptableObject.tileTexture.Length;
        numberTileSpawn = 3 * level_ScriptableObject.level;
        tileTotal = level_ScriptableObject.level * textureNumber * 3;

        for (int i = 0; i < textureNumber; i++)
        {
            for (int j = 0; j < numberTileSpawn; j++)
            {
                numberTextures.Add(i);
            }
        }
        StartCoroutine(SpawnTile());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPlay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<Tile>() != null)
                {
                    for (int i = 0; i < queuePosition.Length; i++)
                    {
                        if (queuePosition[i].childCount == 0)
                        {
                            hitInfo.collider.gameObject.transform.DOMove(queuePosition[i].position, .5f);
                            hitInfo.collider.gameObject.transform.SetParent(queuePosition[i], true);
                            hitInfo.collider.gameObject.transform.GetComponent<Rigidbody>().isKinematic = true;
                            hitInfo.collider.gameObject.transform.rotation = Quaternion.Euler(queuePosition[i].position);
                        }

                        if (i == queuePosition.Length - 1 && queuePosition[i].childCount != 0)
                        {
                            Debug.Log("You lose");
                        }
                    }
                }
            }
        }
    }

    private IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < tileTotal; i++)
        {
            randomTextureIndex = Random.Range(0, numberTextures.Count);
            yield return new WaitForSeconds(.1f);
            GameObject tile = Instantiate(level_ScriptableObject.tilePrefab, initPosition.position, Quaternion.identity);
            tile.AddComponent<Tile>();
            tile.GetComponent<Renderer>().material.mainTexture = level_ScriptableObject.tileTexture[numberTextures[randomTextureIndex]];
            //Debug.Log(tile.GetComponent<Renderer>().material.mainTexture.name);
            tile.transform.GetComponent<Rigidbody>().AddForce(Vector3.one * 50f, ForceMode.Impulse);

            numberTextures.Remove(numberTextures[randomTextureIndex]);
        }
        yield return new WaitForSeconds(3f);
        canPlay = true;
    }
}