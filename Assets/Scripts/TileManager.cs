using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileManager : MonoBehaviour
{
    public Level_ScriptableObject level_ScriptableObject;
    public Transform[] queuePositions;

    [SerializeField] private Transform initPosition;

    private bool canPlay = false;
    private int randomTextureIndex;
    private int textureNumber;
    private int numberTileSpawn;
    private int tileTotal;
    private readonly List<int> numberTextures = new();
    [SerializeField] private List<Tile> tiles = new();

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
                if (hitInfo.collider.gameObject.GetComponent<Tile>())
                {
                    Tile tile = hitInfo.collider.gameObject.GetComponent<Tile>();
                    if (tiles.Count < 5)
                    {
                        tiles.Add(tile);
                        SortByTileName(tiles);

                        //pickup                        
                        SortPickupTile();

                        //check match3
                        for (int i = 0; i < tiles.Count - 2; i++)
                        {
                            if (tiles[i].name == tiles[i + 1].name && tiles[i].name == tiles[i + 2].name)
                            {
                                tiles[i].gameObject.SetActive(false);
                                tiles[i + 1].gameObject.SetActive(false);
                                tiles[i + 2].gameObject.SetActive(false);
                                
                                tiles.RemoveAt(i);
                                tiles.RemoveAt(i);
                                tiles.RemoveAt(i);
                            }
                        }
                    }
                    if (tiles.Count == 5)
                    {
                        Debug.Log("Lose");
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        SortPickupTile();
    }

    private void SortPickupTile()
    {
        foreach (Tile tileTMP in tiles)
        {
            int tileIndex = tiles.IndexOf(tileTMP);
            StartCoroutine(PickupTile(tileTMP.transform, queuePositions[tileIndex]));
        }
    }

    private IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < tileTotal; i++)
        {
            randomTextureIndex = UnityEngine.Random.Range(0, numberTextures.Count);
            yield return new WaitForSeconds(.1f);
            GameObject tile = Instantiate(level_ScriptableObject.tilePrefab, initPosition.position, Quaternion.identity);
            tile.AddComponent<Tile>();
            tile.GetComponent<Renderer>().material.mainTexture = level_ScriptableObject.tileTexture[numberTextures[randomTextureIndex]];
            tile.transform.GetComponent<Rigidbody>().AddForce(Vector3.one * 50f, ForceMode.Impulse);
            tile.transform.SetParent(this.transform, false);
            numberTextures.Remove(numberTextures[randomTextureIndex]);
        }
        yield return new WaitForSeconds(2f);
        canPlay = true;
    }

    private IEnumerator PickupTile(Transform current, Transform target)
    {
        yield return new WaitForSeconds(.1f);
        current.transform.DOMove(target.position, .2f);
        current.transform.SetParent(target, true);
        current.transform.GetComponent<Rigidbody>().isKinematic = true;
        current.transform.rotation = Quaternion.Euler(target.position);
    }

    private void SortByTileName(List<Tile> tiles)
    {
        tiles.Sort(new TileComparer());
    }
}

public class TileComparer : IComparer<Tile>
{
    public int Compare(Tile tile1, Tile tile2)
    {
        return string.Compare(tile1.name, tile2.name, StringComparison.Ordinal);
    }
}