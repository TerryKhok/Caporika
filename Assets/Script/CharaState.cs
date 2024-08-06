using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	キャラの状態を保持する
*/
public class CharaState : MonoBehaviour
{
    public enum State
    {
        Normal,    // 通常
        Flying,    // 飛んでいる
        Dead,      // 死んでいる
    }

    public State state;     // このキャラの状態
    public int sizeState;   // このキャラの大きさ


    /**
     *  @brief 	キャラの状態のセット
     *  @param  State _state   状態
    */
    public void SetCharaState(State _state)
    {
        this.state = _state;
    }

    /**
     *  @brief 	キャラの状態の取得
     *  @return State this.this.state  状態
    */
    public State GetCharaState()
    {
        return this.state;
    }

    /**
     *  @brief 	キャラのサイズの取得
     *  @return int this.sizeState  サイズ
    */
    public int GetCharaSize()
    {
        return this.sizeState;
    }
}