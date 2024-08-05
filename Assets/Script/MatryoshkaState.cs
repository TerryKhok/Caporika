using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaState : MonoBehaviour
{
    public enum State
    {
        Normal,    // 通常
        Flying,    // 飛んでいる
        Dead,      // 死んでいる
    }

    public State state;     // このマトリョーシカの状態
    public int sizeState;   // このマトリョーシカの大きさ

    // 状態のセット
    public void SetMatryoshkaState(State _state)
    {
        this.state = _state;
    }

    // 状態の取得
    public State GetMatryoshkaState()
    {
        return this.state;
    }

    // 大きさの取得
    public int GetMatryoshkaSize()
    {
        return this.sizeState;
    }
}