using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	キャラの大きさ、重さを保持する
 * 
 *  @memo   Rigidbody2D.Massに重さを適用させられるように一応変更可能(今は適用×)
*/
public class CharaState : MonoBehaviour
{
    [SerializeField]
    private int size;   // キャラの大きさ

    [SerializeField]
    private int weight; // キャラの重さ

    private void Start()
    {
        //// rigidbody2D.Massに重さを適用する
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //if(!rb)
        //{
        //    Debug.LogError("Rigidbody2Dが見つかりませんでした。");
        //    return;
        //}
        //rb.mass = this.weight;
    }

    /**
     *  @brief 	キャラの重さのセット
     *  @param int _charaWeight  重さ
    */
    public void SetCharaWeight(int _charaWeight)
    {
        this.weight= _charaWeight;
    }

    /**
     *  @brief 	キャラの重さの取得
     *  @return int this.weight  重さ
    */
    public int GetCharaWeight()
    {
        return this.weight;
    }

    /**
     *  @brief 	キャラの大きさのセット
     *  @param int _charaSize  大きさ
    */
    public void SetCharaSize(int _charaSize)
    {
        this.size = _charaSize;
    }

    /**
     *  @brief 	キャラの大きさの取得
     *  @return int this.size  大きさ
    */
    public int GetCharaSize()
    {
        return this.size;
    }
}