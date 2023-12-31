﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public delegate void MatchThreeDelegate(int score); //Dinh nghia ham delegate
    public static MatchThreeDelegate matchThreeDelegate; //Khai bao ham delegate

    [SerializeField] private Transform initPosition;
    [SerializeField] private Transform[] queuePositions;
    [SerializeField] private List<Texture2D> textures;
    [SerializeField] private List<Tile> tilePickups = new();
    [SerializeField] private List<Texture2D> listTextures = new();

    private bool canPlay;
    private bool canClick = true;
    private int scores;
    private int scoreWhenLose;
    private int levelNumber;
    private int levelSelected;
    private int randomTextureIndex;
    private int textureNumber;
    private int tileTotal;
    private int checkEmptyTile = -1;
    private readonly List<int> numberTileSpawns = new();

    private void Start()
    {
        if (GameManager.HasInstance)
        {
            levelNumber = GameManager.Instance.levels.level_List.Count;
            levelSelected = GameManager.Instance.CurrentLevel;
            scores = GameManager.Instance.Scores;
            scoreWhenLose = scores;
            textureNumber = GameManager.Instance.GetTextureNumber(levelSelected);
            GameManager.Instance.CanPlay = false;
            canPlay = GameManager.Instance.CanPlay;
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.levelText.text = "Level: " + (levelSelected + 1).ToString();
            UIManager.Instance.GamePanel.scoreText.text = "Score: " + scores.ToString();
        }

        if (GameManager.HasInstance)
        {
            foreach (Texture2D texture in textures)
            {
                if (GameManager.Instance.CheckTextureName(levelSelected, texture.name))
                {
                    listTextures.Add(texture);
                }
            }
        }

        SpawnTileByLevel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPlay && canClick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<Tile>())
                {
                    canClick = false;
                    checkEmptyTile--;
                    Tile tile = hitInfo.collider.gameObject.GetComponent<Tile>();
                    if (tilePickups.Count < 5)
                    {
                        tilePickups.Add(tile);

                        SortByTileName(tilePickups);

                        SortPickupTile();

                        CheckMatchThree(tilePickups);
                    }
                    if (tilePickups.Count == 5)
                    {
                        StartCoroutine(CheckFullSlot(tilePickups));
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

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    private void SpawnTileByLevel()
    {
        tileTotal = textureNumber * 6;
        checkEmptyTile = tileTotal;

        for (int i = 0; i < textureNumber; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                numberTileSpawns.Add(i);
            }
        }
        StartCoroutine(SpawnTile());
    }

    private IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(.1f);
        if (GameManager.HasInstance)
        {
            for (int i = 0; i < tileTotal; i++)
            {
                randomTextureIndex = Random.Range(0, numberTileSpawns.Count);

                yield return new WaitForSeconds(.1f);
                GameObject tile = Instantiate(GameManager.Instance.tilePrefab, initPosition.position, Quaternion.identity);
                tile.GetComponent<Renderer>().material.mainTexture = listTextures[numberTileSpawns[randomTextureIndex]];
                tile.name = listTextures[numberTileSpawns[randomTextureIndex]].name;
                tile.transform.GetComponent<Rigidbody>().AddForce(Vector3.one * 50f, ForceMode.Impulse);
                tile.transform.SetParent(this.transform, false);
                numberTileSpawns.Remove(numberTileSpawns[randomTextureIndex]);
            }
            yield return new WaitForSeconds(2f);
            GameManager.Instance.CanPlay = true;
            canPlay = GameManager.Instance.CanPlay;
        }
    }

    private void SortPickupTile()
    {
        foreach (Tile tileTMP in tilePickups)
        {
            int tileIndex = tilePickups.IndexOf(tileTMP);
            StartCoroutine(PickupTile(tileTMP.transform, queuePositions[tileIndex]));
        }
    }

    private IEnumerator PickupTile(Transform current, Transform target)
    {
        yield return null;
        current.transform.DOMove(target.position, .1f);
        current.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
        current.transform.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(.1f);
        current.transform.SetParent(target, true);
        canClick = true;
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
            UIManager.Instance.ActiveGamePanel(false);
        }

        if (GameManager.HasInstance)
        {
            PlayerPrefs.SetInt("CurrentScore", scores);
            GameManager.Instance.ChangeScene("Main");
        }

        if (levelSelected < levelNumber - 1)
        {
            DOTween.KillAll();
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveWinPanel(true);
            }
            if (GameManager.HasInstance)
            {
                GameManager.Instance.UpdateLevel(levelSelected + 1);
            }
        }
        else
        {
            DOTween.KillAll();
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveEndPanel(true);
            }

            if (GameManager.HasInstance)
            {
                GameManager.Instance.UpdateLevel(0);
            }
        }

        yield return new WaitForSeconds(.1f);
        checkEmptyTile = -1;
    }

    private IEnumerator CheckFullSlot(List<Tile> tiles)
    {
        yield return new WaitForSeconds(.7f);
        if (tiles.Count >= 5)
        {
            DOTween.KillAll();
            canPlay = false;

            if (UIManager.HasInstance)
            {
                UIManager.Instance.ActiveLosePanel(true);
                UIManager.Instance.ActiveGamePanel(false);
            }
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
            }

            if (GameManager.HasInstance)
            {
                GameManager.Instance.UpdateScores(scoreWhenLose);
                GameManager.Instance.UpdateLevel(levelSelected);
                GameManager.Instance.ChangeScene("Main");
            }
        }
    }

    private void SortByTileName(List<Tile> tiles)
    {
        tiles.Sort(new TileComparer());
    }
}