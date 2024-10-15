using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;  // Text UIを使用するために必要


/**
* @brief タイマーの制御
* @memo  アイリスインが終了次第タイマーを作動させる。
*        StopTimerが呼ばれたらタイマーを止めてリザルトに表示する
*/


public class TimerScript : MonoBehaviour
{
    private TextMeshProUGUI timerText;     // 時間を表示するText UI
    public float elapsedTime; // 経過時間
    private bool isRunning;    // ストップウォッチが動作しているかどうか
    private GameObject iris;  //irisをインスペクターで入れる
    private AnimIrisEvent animIrisEvent;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        if (!this.timerText)
        {
            Debug.LogError("TextMeshProが見つからず、取得できませんでした。");
            return;
        }

        animIrisEvent =  GameObject.Find("iris").GetComponent<AnimIrisEvent>();
        if (!this.animIrisEvent)
        {
            Debug.LogError("AnimIrisEventoが見つからず、取得できませんでした。");
            return;
        }

        ResetTimer();  // スタート時にタイマーをリセット
    }

    /**
     * @brief 
     * @memo  アイリスインが終了したらタイマーを作動させる
     */
    void Update()
    {
        if(animIrisEvent.irisInFinish)
        {
            isRunning = true;
        }

        if (isRunning)
        {
            elapsedTime += Time.deltaTime;  // 経過時間を増加
        }
    }

    /**
     * @brief タイマー開始
     * @memo  
     */
    public void StartTimer()
    {
        isRunning = true;
    }


    /**
     * @brief タイマーを停止する
     * @memo  
     */
    public void StopTimer()
    {
        isRunning = false;
        TimerDisplay();
    }


    /**
     * @brief タイマーをリセットする
     * @memo  
     */
    public void ResetTimer()
    {
        elapsedTime = 0f;  // 経過時間をリセット
    }


    /**
     * @brief 経過時間を表示するメソッド
     * @memo  
     */
    public void TimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100F) % 100F);
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);  // 00:00:00形式で表示
    }
}
