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
    private readonly List<int> numberTextures = new();

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            levelSelected = GameManager.Instance.CurrentLevel;
            scores = GameManager.Instance.Scores;
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.levelText.text = "Level: " + (levelSelected + 1).ToString();
            UIManager.Instance.GamePanel.scoreText.text = "Score: " + scores.ToString();
        }

        SpawnTileByLevel(levelSelected);
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
            Quaternion rotation = Quaternion.AngleAxis(60f, Vector3.up);
            GameObject tile = Instantiate(GameManager.Instance.tilePrefab, initPosition.position, rotation);
            
            tile.AddComponent<Tile>();
            if (GameManager.HasInstance)
            {
                tile.GetComponent<Renderer>().material.mainTexture = GameManager.Instance.GetTextureByLevel(levelSelected)[numberTextures[randomTextureIndex]];
            }

            tile.transform.GetComponent<Rigidbody>().AddForce(Vector3.one * 50f, ForceMode.Force);
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
        current.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
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

        if (levelSelected < 2)
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveWinPanel(true);
                UIManager.Instance.ActiveGamePanel(false);
            }
            if (GameManager.HasInstance)
            {
                PlayerPrefs.SetInt("CurrentScore", scores);
                GameManager.Instance.ChangeScene("Main");
                GameManager.Instance.UpdateLevel(levelSelected + 1);
            }
        }
        else
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveEndPanel(true);
                UIManager.Instance.ActiveGamePanel(false);
            }

            if (GameManager.HasInstance)
            {
                PlayerPrefs.DeleteKey("CurrentLevel");
                PlayerPrefs.DeleteKey("CurrentScore");
                GameManager.Instance.UpdateLevel(0);
                GameManager.Instance.ChangeScene("Main");
            }
        }
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
                UIManager.Instance.ActiveGamePanel(false);
            }
            canPlay = false;
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
            }

            if (GameManager.HasInstance)
            {
                GameManager.Instance.ChangeScene("Main");
                GameManager.Instance.UpdateLevel(levelSelected);
            }
        }
    }

    private void SortByTileName(List<Tile> tiles)
    {
        tiles.Sort(new TileComparer());
    }
}