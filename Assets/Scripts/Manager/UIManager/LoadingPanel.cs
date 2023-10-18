using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour
{
    public TextMeshProUGUI loadingPercentText;

    void OnEnable()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(.5f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingPercentText.SetText("Press to continue !!!");

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    asyncOperation.allowSceneActivation = true;
                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ActiveGamePanel(true);
                        UIManager.Instance.ActiveLoadingPanel(false);
                    }
                    if (GameManager.HasInstance)
                    {
                        GameManager.Instance.StartGame();
                    }
                }
            }
            yield return null;
        }
    }
}