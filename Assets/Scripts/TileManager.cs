using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public delegate void MatchThreeDelegate(int score); //Dinh nghia ham delegate
    public static MatchThreeDelegate matchThreeDelegate; //Khai bao ham delegate

    [SerializeField] private Transform initPosition;
    [SerializeField] private Transform[] queuePositions;
    [SerializeField] private List<Tile> tiles = new();

    private int scores;
    private bool canPlay = false;
    private int levelSelected;
    private int randomTextureIndex;
    private int textureNumber;
    private int tileTotal;
    private int checkEmptyTile = -1;
    private List<int> numberTextures = new();

    private void Awake()
    {
    }

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            levelSelected = GameManager.Instance.CurrentLevel;
        }

        SpawnTileByLevel(levelSelected);
        if (GameManager.HasInstance)
        {
            scores = GameManager.Instance.Scores;
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
            if (GameManager.HasInstance)
            {
                GameManager.Instance.UpdateLevel(0);
                GameManager.Instance.UpdateScores(0);
            }
            StartCoroutine(WinAction());
        }
    }

    private void LateUpdate()
    {
        SortPickupTile();
    }

    private void SpawnTileByLevel(int levelSelected)
    {
        if (GameManager.HasInstance)
        {
            textureNumber = GameManager.Instance.GetNumberTextureByLevel(levelSelected);
        }
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

    private IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < tileTotal; i++)
        {
            randomTextureIndex = Random.Range(0, numberTextures.Count);
            yield return new WaitForSeconds(.1f);
            GameObject tile = Instantiate(GameManager.Instance.tilePrefab, initPosition.position, Quaternion.identity);
            tile.AddComponent<Tile>();
            if (GameManager.HasInstance)
            {
                tile.GetComponent<Renderer>().material.mainTexture = GameManager.Instance.GetTextureByLevel(levelSelected)[numberTextures[randomTextureIndex]];
            }
            tile.transform.GetComponent<Rigidbody>().AddForce(Vector3.one * 50f, ForceMode.Impulse);
            tile.transform.SetParent(this.transform, false);
            numberTextures.Remove(numberTextures[randomTextureIndex]);
        }
        yield return new WaitForSeconds(2f);
        canPlay = true;
    }

    private void SortPickupTile()
    {
        foreach (Tile tileTMP in tiles)
        {
            int tileIndex = tiles.IndexOf(tileTMP);
            StartCoroutine(PickupTile(tileTMP.transform, queuePositions[tileIndex]));
        }
    }

    private IEnumerator PickupTile(Transform current, Transform target)
    {
        yield return new WaitForSeconds(.1f);
        current.transform.DOMove(target.position, .1f);
        current.transform.SetParent(target, true);
        current.transform.GetComponent<Rigidbody>().isKinematic = true;
        current.transform.rotation = Quaternion.Euler(target.position);
    }

    private void CheckMatchThree(List<Tile> tiles)
    {
        for (int i = 0; i < tiles.Count - 2; i++)
        {
            if (tiles[i].name == tiles[i + 1].name && tiles[i].name == tiles[i + 2].name)
            {
                StartCoroutine(MatchThree(tiles, i));
                scores++;
                if (GameManager.HasInstance)
                {
                    GameManager.Instance.UpdateScores(scores);
                }
                matchThreeDelegate(scores);
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

    private IEnumerator WinAction()
    {
        yield return new WaitForSeconds(.5f);
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_WIN);
        }
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveWinPanel(true);
        }
        //if (GameManager.HasInstance)
        //{
        //    GameManager.Instance.ChangeScene("Main");
        //    GameManager.Instance.UpdateLevel(levelSelected);
        //}
        yield return new WaitForSeconds(.1f);
        checkEmptyTile = -1;
    }

    private IEnumerator CheckFullSlot(List<Tile> tiles)
    {
        yield return new WaitForSeconds(.7f);
        if (tiles.Count == 5)
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveLosePanel(true);
            }
            canPlay = false;
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
            }
            //if (GameManager.HasInstance)
            //{
            //    GameManager.Instance.ChangeScene("Main");
            //    GameManager.Instance.UpdateLevel(levelSelected - 1);
            //}
        }
    }

    private void SortByTileName(List<Tile> tiles)
    {
        tiles.Sort(new TileComparer());
    }
}