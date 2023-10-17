using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public TextMeshProUGUI loadingPercentText;
    //public Slider loadingSlider;

    void OnEnable()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(.1f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            //loadingSlider.value = asyncOperation.progress;
            loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                //loadingSlider.value = 1f;
                loadingPercentText.SetText("Press the space bar to continue");
                
                if (Input.GetKeyDown(KeyCode.Space))
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