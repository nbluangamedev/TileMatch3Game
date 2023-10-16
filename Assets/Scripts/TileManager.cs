using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TileManager : MonoBehaviour
{
    public delegate void CollectDiamond(int diamond); //Dinh nghia ham delegate 
    public static CollectDiamond collectDiamondDelegate; //Khai bao ham delegate
    private int diamonds = 0;

    public Level_ScriptableObject level_ScriptableObject;
    public Transform[] queuePositions;
    [SerializeField] private Transform initPosition;
    [SerializeField] private TextMeshProUGUI levelText;

    private bool canPlay = false;
    private int randomTextureIndex;
    private int textureNumber;
    private int tileTotal;
    private int checkEmptyTile = -1;
    private List<int> numberTextures = new();
    [SerializeField] private List<Tile> tiles = new();

    private void Awake()
    {
        DOTween.SetTweensCapacity(1000, 50);
        levelText.text = "Level: " + level_ScriptableObject.level;
        textureNumber = level_ScriptableObject.tileTexture.Length;
        tileTotal = textureNumber * 6;
        checkEmptyTile = tileTotal;

        for (int i = 0; i < textureNumber; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                numberTextures.Add(i);
            }
        }
        StartCoroutine(SpawnTile());
    }

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            diamonds = GameManager.Instance.Scores;
        }
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
                    checkEmptyTile--;
                    Tile tile = hitInfo.collider.gameObject.GetComponent<Tile>();
                    if (tiles.Count < 5)
                    {
                        tiles.Add(tile);
                        //sort list
                        SortByTileName(tiles);

                        //sort position                       
                        SortPickupTile();

                        //check match3
                        CheckMatchThree(tiles);
                    }
                    if (tiles.Count == 5)
                    {
                        StartCoroutine(CheckFullSlot(tiles));
                    }
                }
            }
        }

        if (checkEmptyTile == 0)
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_WIN);
                Debug.Log("Win");
            }
            checkEmptyTile = -1;
        }
    }

    private void LateUpdate()
    {
        SortPickupTile();
    }

    private void CheckMatchThree(List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count - 2; i++)
        {
            if (tiles[i].name == tiles[i + 1].name && tiles[i].name == tiles[i + 2].name)
            {
                StartCoroutine(MatchThree(tiles, i));
                diamonds++;
                GameManager.Instance.UpdateScores(diamonds);
                collectDiamondDelegate(diamonds);
            }
        }
    }

    private void SortPickupTile()
    {
        foreach (Tile tileTMP in tiles)
        {
            int tileIndex = tiles.IndexOf(tileTMP);
            StartCoroutine(PickupTile(tileTMP.transform, queuePositions[tileIndex]));
        }
    }

    private IEnumerator CheckFullSlot(List<Tile> tiles)
    {
        yield return new WaitForSeconds(.7f);
        if (tiles.Count == 5)
        {
            Debug.Log("Lose");
            canPlay = false;
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
            }            
        }
    }

    private IEnumerator MatchThree(List<Tile> tiles, int i)
    {
        yield return new WaitForSeconds(.3f);
        tiles[i].gameObject.SetActive(false);
        tiles[i + 1].gameObject.SetActive(false);
        tiles[i + 2].gameObject.SetActive(false);
        tiles.RemoveAt(i);
        tiles.RemoveAt(i);
        tiles.RemoveAt(i);
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_MATCH3);
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
        current.transform.DOMove(target.position, .1f);
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