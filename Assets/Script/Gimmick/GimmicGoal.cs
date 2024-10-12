
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicGoal : MonoBehaviour
{
    
    bool is_goal = false;//ゴールしたかフラグ
    private GameObject TimerDispay; //タイマーストップ用
    private TimerScript timerScript;

    public List<GameObject> dummyPlayer;    //アニメーション用の王レイヤーダミーを三種類入れる
    private Animator goalAnim;  
    CharaState charaState;
    
    public void Start()
    {
        TimerDispay = GameObject.Find("TimerDispay");  
        if (!this.TimerDispay)
        {
            Debug.LogError("GameObjectが見つからず、取得できませんでした。");
            return;
        }
        
        timerScript = TimerDispay.GetComponent<TimerScript>();
        if (!this.timerScript)
        {
            Debug.LogError("TimerScriptが見つからず、取得できませんでした。");
            return;
        }
    }

    /**
     * @brief 衝突判定
     */  
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(!is_goal)//ゴールしていなかったら
        {
            if (col.gameObject.CompareTag("Player"))    //プレイヤーだったら
            {
                is_goal = true;
                timerScript.StopTimer();//タイマーを停止
                //resultAnim.Play("ResultAnim");//アニメーションを再生
                
                charaState = col.GetComponent<CharaState>();
                goalAnim = dummyPlayer[charaState.GetCharaSize() - 1].GetComponent<Animator>();
                StartCoroutine("CameraMoveTimer");
            }
        }
    }

     IEnumerator CameraMoveTimer()
    {
        yield return new WaitForSeconds(1);//一秒停止
        PlayGoalAnimation(charaState.GetCharaSize());
    }

    private void PlayGoalAnimation(int _playerSize)
    {
        dummyPlayer[charaState.GetCharaSize() - 1].gameObject.SetActive(true);
        // プレイヤーのサイズに応じて、異なるアニメーションクリップを再生
        switch (_playerSize)
        {
            case 1:
                goalAnim.Play("GoalAnim_Small");  // 小さいプレイヤー用アニメーション
                break;
            case 2:
                goalAnim.Play("GoalAnim_Medium");  // 中サイズのプレイヤー用アニメーション
                break;
            case 3:
                goalAnim.Play("GoalAnim_Big");  // 大きいプレイヤー用アニメーション
                break;
        }
    }
}
