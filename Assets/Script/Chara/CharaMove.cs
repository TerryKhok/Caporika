using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief プレイヤーと敵の移動処理の基底クラス
 */
public class CharaMove : MonoBehaviour
{
    /**
     * @brief プレイヤーと敵共通の状態を表す
     */
    public enum CharaCondition
    {
        Ground,
        Flying,
        Swimming,
        Dead,
    }

    CharaCondition charaCondition = CharaCondition.Ground;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
     *  @brief キャラの状態をセットする
     *  @param  CharaCondition _charaCondition 状態
     */
    public void SetCharaCondition(CharaCondition _charaCondition)
    {
        this.charaCondition = _charaCondition;
    }

    /**
     *  @brief キャラの状態を取得する
     *  @return  CharaCondition this.charaCondition 状態
     */
    public CharaCondition GetCharaCondition()
    {
        return this.charaCondition;
    }
}
