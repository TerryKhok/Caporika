using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;  // AudioMixerをインスペクタで設定
    public Slider masterVolumeSlider;  // マスターボリュームのスライダー
    public Slider bgmVolumeSlider;     // BGMボリュームのスライダー
    public Slider seVolumeSlider;      // SEボリュームのスライダー

    private float bgmBaseVolume = 1f;  // BGMのベース音量（0～1の範囲）
    private float seBaseVolume = 1f;   // SEのベース音量（0～1の範囲）

    void Start()
    {
        // 初期値を設定
        masterVolumeSlider.value = 1.0f;  // 初期マスターボリューム
        bgmVolumeSlider.value = bgmBaseVolume;   // 初期BGMボリューム
        seVolumeSlider.value = seBaseVolume;    // 初期SEボリューム

        // スライダーの値が変わったときのイベントを設定
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        seVolumeSlider.onValueChanged.AddListener(SetSEVolume);
    }

    // マスターボリュームを設定する関数
    public void SetMasterVolume(float value)
    {
        float masterVolume = ConvertToDecibel(value);  // 対数変換
        audioMixer.SetFloat("MasterVolume", masterVolume);  // MasterVolumeの公開パラメータに反映
        UpdateBGMVolume();
        UpdateSEVolume();
        Debug.Log("Master Volume: " + masterVolume + " dB");
    }

    // BGMボリュームを設定する関数
    public void SetBGMVolume(float value)
    {
        bgmBaseVolume = value;  // スライダーの値を保存しておく
        UpdateBGMVolume();
    }

    // SEボリュームを設定する関数
    public void SetSEVolume(float value)
    {
        seBaseVolume = value;  // スライダーの値を保存しておく
        UpdateSEVolume();
    }

    // BGMの音量を更新（Master Volumeに基づいて調整）
    private void UpdateBGMVolume()
    {
        float masterVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);  // MasterVolumeを取得
        float bgmVolume = ConvertToDecibel(bgmBaseVolume);
        bgmVolume += masterVolume;  // Masterの音量に基づいて調整
        audioMixer.SetFloat("BGMVolume", bgmVolume);
        Debug.Log("BGM Volume: " + bgmVolume + " dB");
    }

    // SEの音量を更新（Master Volumeに基づいて調整）
    private void UpdateSEVolume()
    {
        float masterVolume;
        audioMixer.GetFloat("MasterVolume", out masterVolume);  // MasterVolumeを取得
        float seVolume = ConvertToDecibel(seBaseVolume);
        seVolume += masterVolume;  // Masterの音量に基づいて調整
        audioMixer.SetFloat("SEVolume", seVolume);
        Debug.Log("SE Volume: " + seVolume + " dB");
    }

    // スライダーの値を対数変換してデシベルに変換する
    private float ConvertToDecibel(float value)
    {
        // スライダーの0～1の値を対数スケールでデシベルに変換 (-80dB～0dB)
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }
}
