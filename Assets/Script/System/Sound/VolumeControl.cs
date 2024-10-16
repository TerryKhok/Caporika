using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public CustomSlider masterVolumeSlider;
    public CustomSlider bgmVolumeSlider;
    public CustomSlider seVolumeSlider;

    private float bgmBaseVolume = 1f;
    private float seBaseVolume = 1f;

    void Start()
    {
        masterVolumeSlider = GameObject.Find("MainVolSlider").GetComponent<CustomSlider>();
        if (!this.masterVolumeSlider)
        {
            Debug.LogError("CustomSliderが見つからず、取得できませんでした。");
            return;
        }

        bgmVolumeSlider = GameObject.Find("BGMVolSlider").GetComponent<CustomSlider>();
        if (!this.bgmVolumeSlider)
        {
            Debug.LogError("CustomSliderが見つからず、取得できませんでした。");
            return;
        }

        seVolumeSlider = GameObject.Find("SEVolSlider").GetComponent<CustomSlider>();
        if (!this.seVolumeSlider)
        {
            Debug.LogError("CustomSliderが見つからず、取得できませんでした。");
            return;
        }

        // 初期値を設定
        masterVolumeSlider.value = 1.0f;
        bgmVolumeSlider.value = bgmBaseVolume;
        seVolumeSlider.value = seBaseVolume;

        // スライダー値変更時のイベント設定
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        seVolumeSlider.onValueChanged.AddListener(SetSEVolume);

        Debug.Log("Listener added for masterVolumeSlider");
    }

    public void SetMasterVolume(float value)
    {
        float masterVolume = ConvertToDecibel(value);
        audioMixer.SetFloat("MasterVolume", masterVolume);
        UpdateBGMVolume();
        UpdateSEVolume();
    }

    public void SetBGMVolume(float value)
    {
        bgmBaseVolume = value;
        UpdateBGMVolume();
    }

    public void SetSEVolume(float value)
    {
        seBaseVolume = value;
        UpdateSEVolume();
    }

    private void UpdateBGMVolume()
    {
        float masterVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        float bgmVolume = ConvertToDecibel(bgmBaseVolume);
        bgmVolume += masterVolume;
        audioMixer.SetFloat("BGMVolume", bgmVolume);
    }

    private void UpdateSEVolume()
    {
        float masterVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);
        float seVolume = ConvertToDecibel(seBaseVolume);
        seVolume += masterVolume;
        audioMixer.SetFloat("SEVolume", seVolume);
    }

    private float ConvertToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }
}
