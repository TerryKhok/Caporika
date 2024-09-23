using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmicGoal : MonoBehaviour
{
    bool is_goal;
    

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.gameObject.CompareTag("Player"))    //プレイヤーだったら
        {
            //プレイヤーを動かなくさせる
            //アニメーションを再生
        }
    }
}
