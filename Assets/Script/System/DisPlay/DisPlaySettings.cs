using UnityEngine;
using TMPro;  // TextMeshProを使用するためのネームスペース


/**
* @brief ディスプレイ設定を変更する関数
* @memo プルダウンで選択された値を一時保存し、適応ボタンが押された場合それらを適応する。
*/
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


/**
* @brief ディスプレイモードが選択されたときに一時的に値を保存
* @memo 
*/   
    public void OnDisplayModeSelected(int modeIndex)
    {
        selectedDisplayModeIndex = modeIndex;
    }


/**
* @brief 解像度が選択されたときに一時的に値を保存
* @memo 
*/   
    public void OnResolutionSelected(int resolutionIndex)
    {
        selectedResolutionIndex = resolutionIndex;
    }

/**
* @brief 適応ボタンが押されたときに設定を適用
* @memo 適応ボタンが押されたら実際に解像度とディスプレイモードを適応する
*/   
    public void ApplySettings()
    {
        // 選択された解像度とディスプレイモードを適用
        ApplyDisplayMode(selectedDisplayModeIndex);
        ApplyResolution(selectedResolutionIndex);
    }


/**
* @brief 選択されたディスプレイモードを適用
* @memo  適応ボタンが押されたら実際にディスプレイモードを適応する
*/
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
    }

/**
* @brief 選択された解像度を適用
* @memo  適応ボタンが押されたら実際に解像度を適応する
*/
    private void ApplyResolution(int resolutionIndex)
    {
        Resolution selectedResolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
    }
}
