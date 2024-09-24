using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	CharaState.cs変更で他のスクリプトの修正が終わるまでの間一時的に使うクラス
 * 
 *  @memo   ・各自スクリプトを修正(CharaMove.charaConditionを基に処理を動かすように)
 *          ・全てのスクリプト内でこのクラスを使用しなくなったときこのスクリプトを削除する
*/
public class TenpState : MonoBehaviour
{
    public enum State
    {
        Normal,     // 通常
        Flying,     // 飛んでいる
        Damaged,    // ダメージを受けている
        Dead,       // 死んでいる
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