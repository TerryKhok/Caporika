using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


/**
 *  @brief 水のギミックの処理を記述
 *  水オブジェクトにアタッチ
 *  
 *  @memo   ・浮き沈みの処理
 */
public class GimmickWater : MonoBehaviour
{
    private List<TenpState> objectsInWater = new List<TenpState>();    // 現在水中にいるオブジェクトを管理するためのlist

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TenpState tempState;
        // TenpStateを持っているか(マトリョシカか)判定
        if (!collision.gameObject.TryGetComponent(out tempState)) return;

        AddToList(tempState);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TenpState tempState;
        // TenpStateがなかったらreturn
        if (!collision.gameObject.TryGetComponent(out tempState)) return;

        // TenpStateを持っているか(マトリョシカか)判定
        if (objectsInWater.Contains(tempState))
        {
            RemoveFromList(tempState);
        }
    }

    void AddToList(TenpState _state)
    {
        // y方向の速度を減衰させる
        Rigidbody2D rb = _state.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 10.0f);
        if (_state.GetCharaState() != TenpState.State.Dead)    // 死んでないなら
        {
            _state.SetCharaState(TenpState.State.Normal);
        }

        int size = _state.GetCharaSize();
        switch (size)
        {
            case 1: // 小さい
                rb.gravityScale = -0.2f;
                break;
            case 2:
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.gravityScale = 0.0f;
                break;
            case 3:
                rb.gravityScale = 0.2f;
                break;
        }

        objectsInWater.Add(_state);
    }

    /**
     * @brief リストから削除するときの処理
     */
    void RemoveFromList(TenpState _state)
    {
        _state.GetComponent<Rigidbody2D>().gravityScale = 1.0f;  // デフォルト値にする
        objectsInWater.Remove(_state);  // リストから削除
    }
}
