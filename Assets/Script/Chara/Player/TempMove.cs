using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/**
 *  @brief 	プレイヤーの移動(一時的)
 *  
 *  @memo   ・移動方向と反対の向きに傾く
 *          ・止まった時指定回数揺れて止まる
 *          ・飛んでいるときはゆっくり移動できる
 *          
 *          止まっているときはisKinematicを無効にしている
 *          
 *          新しいPlayerMoveが完成したらこのスクリプトは消す
*/
public class TempMove : MonoBehaviour
{
    protected float moveFactor = 1.0f;              // プレイヤーの全体の動きを調整する係数(0.0f～1.0f)、1.0fの時100%力が影響される

    private float moveSpeed = 2.0f;
    private Rigidbody2D rb = null;
    protected float inputDeadZone = 0.1f;               // 入力のデッドゾーン


    private void Start()
    {
        this.rb=GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 左右移動入力
        float moveInput = Input.GetAxis("Horizontal");
        // 入力値のデッドゾーンを適用
        if (Mathf.Abs(moveInput) < this.inputDeadZone) { moveInput = 0.0f; }

        // 速度を計算
        float speed = moveInput * moveSpeed * this.moveFactor;
        this.rb.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
    }
}