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
    public UnityEvent onTriggerExitEvent;   ///< トリガーから離れたときに実行するイベント
    private Collider2D triggeredCollider;   ///< トリガーされたコライダー
    private bool triggeredEnter = false;    ///< すでにトリガーされたか
    private bool triggeredExit = false;
    private bool enemyCanTriggered = false; ///< 敵でもトリガー出来るか

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") || (enemyCanTriggered && _other.CompareTag("Enemy")))  // プレイヤーに対してトリガーを発動
        {
            if (this.onTriggerEnterEvent != null)
            {
                if (!this.triggeredEnter || !this.triggerOnce)    // 1回も発動していないか1回のみと指定されていないなら
                {
                    triggeredCollider = _other;
                    this.onTriggerEnterEvent.Invoke();  // イベントを実行
                    triggeredEnter = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") || (enemyCanTriggered && _other.CompareTag("Enemy")))  // プレイヤーに対してトリガーを発動
        {
            if (this.onTriggerExitEvent != null)
            {
                if (!this.triggeredExit || !this.triggerOnce)    // 1回も発動していないか1回のみと指定されていないなら
                {
                    triggeredCollider = _other;
                    this.onTriggerExitEvent.Invoke();  // イベントを実行
                    triggeredExit = true;
                }
            }
        }
    }

    public Collider2D GetTriggeredCollider() { return triggeredCollider; }
}
