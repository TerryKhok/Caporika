using UnityEngine;
using TMPro;  // TextMeshProを使用するためのネームスペース

public class DisplaySettingsTMP : MonoBehaviour
{
    public TMP_Dropdown displayModeDropdown;  // フルスクリーン/ウィンドウモードのTMP_Dropdown
    public TMP_Dropdown resolutionDropdown;   // 解像度切り替えのTMP_Dropdown
    public UnityEngine.UI.Button applyButton; // 適応ボタン (通常のUIボタン)

    private int selectedDisplayModeIndex;    // 選択されたディスプレイモードのインデックス
    private int selectedResolutionIndex;     // 選択された解像度のインデックス

    private Resolution[] availableResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 720, height = 480 }
    };

    void Start()
    {
        // ディスプレイモードのTMP_Dropdown設定
        displayModeDropdown.options.Clear();
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("FullScrean"));
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Windowed"));
        displayModeDropdown.onValueChanged.AddListener(OnDisplayModeSelected);

        // 解像度のTMP_Dropdown設定
        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1920 x 1080"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("1280 x 720"));
        resolutionDropdown.options.Add(new TMP_Dropdown.OptionData("720 x 480"));
        resolutionDropdown.onValueChanged.AddListener(OnResolutionSelected);

        // 適応ボタンにリスナーを追加
        applyButton.onClick.AddListener(ApplySettings);
    }

    // ディスプレイモードが選択されたときに一時的に値を保存
    public void OnDisplayModeSelected(int modeIndex)
    {
        selectedDisplayModeIndex = modeIndex;
        Debug.Log("Selected Display Mode: " + displayModeDropdown.options[modeIndex].text);
    }

    // 解像度が選択されたときに一時的に値を保存
    public void OnResolutionSelected(int resolutionIndex)
    {
        selectedResolutionIndex = resolutionIndex;
        Debug.Log("Selected Resolution: " + resolutionDropdown.options[resolutionIndex].text);
    }

    // 適応ボタンが押されたときに設定を適用
    public void ApplySettings()
    {
        // 選択された解像度とディスプレイモードを適用
        ApplyDisplayMode(selectedDisplayModeIndex);
        ApplyResolution(selectedResolutionIndex);

        Debug.Log("Settings Applied: Display Mode and Resolution");
    }

    // 選択されたディスプレイモードを適用
    private void ApplyDisplayMode(int modeIndex)
    {
        if (modeIndex == 0)  // フルスクリーン
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if (modeIndex == 1)  // ウィンドウモード
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        Debug.Log("Applied Display Mode: " + displayModeDropdown.options[modeIndex].text);
    }

    // 選択された解像度を適用
    private void ApplyResolution(int resolutionIndex)
    {
        Resolution selectedResolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
        Debug.Log("Applied Resolution: " + selectedResolution.width + " x " + selectedResolution.height);
    }
}
