using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicGoal : MonoBehaviour
{
    bool is_goal;
    

    private void OnTriggerEnter2D(Collider2D col) 
    {
        //プレイヤーを動かなくさせる
        if (col.gameObject.CompareTag("Player"))    //プレイヤーだったら
        {
            
            //アニメーションを再生
        }
    }
}
