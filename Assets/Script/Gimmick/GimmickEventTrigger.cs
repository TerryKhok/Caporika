using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * @brief イベントを発生させるトリガー
 * 
 * @memo ・PlayerのTriggerEnter2Dでイベント発生させる
 */
[RequireComponent(typeof(Collider2D))]
public class GimmickEventTrigger : MonoBehaviour
{
    public bool triggerOnce = false;        ///< trueだと1回しか発動しない
    public UnityEvent onTriggerEnterEvent;  ///< トリガーに触れたときに実行するイベント
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // プレイヤーに対してトリガーを発動
        {
            if (this.onTriggerEnterEvent != null)
            {
                if (!this.triggered || !this.triggerOnce)    // 1回も発動していないか1回のみと指定されていないなら
                {
                    this.onTriggerEnterEvent.Invoke();  // イベントを実行
                    triggered = true;
                }
            }
        }
    }
}
