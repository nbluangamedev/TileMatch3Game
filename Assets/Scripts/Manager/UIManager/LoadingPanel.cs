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
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            yield return null;
            if (asyncOperation.progress > 0.8f)
            {
                loadingPercentText.SetText("Press to continue !!");
                if (Input.GetMouseButtonDown(0))
                {
                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ActiveLoadingPanel(false);
                        UIManager.Instance.ActiveGamePanel(true);
                    }
                    asyncOperation.allowSceneActivation = true;
                    if (GameManager.HasInstance)
                    {
                        GameManager.Instance.StartGame();
                    }
                }
            }
        }
    }
}