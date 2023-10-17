using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider seSlider;

    private float bgmValue;
    private float seValue;

    private void Awake()
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
        }
    }

    private void OnEnable()
    {
        if (AudioManager.HasInstance)
        {
            bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            seValue = AudioManager.Instance.AttachSESource.volume;
            bgmSlider.value = bgmValue;
            seSlider.value = seValue;
        }
    }

    public void OnSliderChangeBGMValue(float v)
    {
        bgmValue = v;
    }

    public void OnSliderChangeSEValue(float v)
    {
        seValue = v;
    }

    public void OnCancelButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(false);
        }

        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.IsPlaying == false && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }

    public void OnSubmitButtonClick()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(bgmValue);
            AudioManager.Instance.ChangeSEVolume(seValue);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(false);
        }

        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.IsPlaying == false && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }
}