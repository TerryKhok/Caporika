
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicGoal : MonoBehaviour
{
    
    bool is_goal = false;//ゴールしたかフラグ
    public GameObject result;       //ResultImageを入れる
    private Animator resultAnim;    //リザルトアニメーション用

    private GameObject TimerDispay;
    private TimerScript timerScript;
    
    public void Start()
    {
        TimerDispay = GameObject.Find("TimerDispay");  
        timerScript = TimerDispay.GetComponent<TimerScript>();
        resultAnim = result.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(!is_goal)//ゴールしていなかったら
        {
            if (col.gameObject.CompareTag("Player"))    //プレイヤーだったら
            {
                is_goal = true;
                timerScript.StopTimer();//タイマーを停止
                resultAnim.Play("ResultAnim");//アニメーションを再生
            }
        }
    }
}
