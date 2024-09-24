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
    public UnityEvent onTriggerEnterEvent;  ///< トリガーに触れたときに実行するイベント

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // プレイヤーに対してトリガーを発動
        {
            if (this.onTriggerEnterEvent != null)
            {
                this.onTriggerEnterEvent.Invoke();  // イベントを実行
            }
        }
    }
}
