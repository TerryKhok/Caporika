using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	プレイヤーの状態毎の処理を行う基底クラス
 * 
 *  @memo   ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *  
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================       
*/
public abstract class PlayerState
{
    //===============================================
    //          マトリョーシカの揺れ
    //===============================================

    protected float tiltVelocity = 0.0f;                // 傾きの速度
    protected  float tiltAmount = 45.0f;                // 回転角の最大値
    protected  float damping = 0.8f;                    // 減衰係数(どのくらいずつ回転角度を減らしていくか)
    protected  int maxSwimg = 3;                        // 何回揺れるか 
    protected 　int swingCount = 0;                     // 揺れの回数をカウント
    protected 　float angleSwingZone = 1.0f;            // 揺れた判定内かどうか
    protected bool isInDeadZone = false;                // 揺れのデッドゾーン内にいるかどうか

    /**
     * @brief 	この状態に入るときに行う関数
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2Dやその他コンポーネントを取得するためのみに使用する
    */
    public abstract void Enter(PlayerMove _playerMove);

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public abstract void Exit();

    /**
     * @brief 	更新処理t
    */
    public abstract void Update();

    /**
     * @brief 	当たった時の処理
     *  @param  Collision _collision    当たったオブジェクト
    */
    public abstract void CollisionEnter(Collision _collision);

    ///**
    // * @brief 	当たらなくなった時の処理
    // *  @param  Collision _collision    当たったオブジェクト
    //*/
    //public abstract void CollisionExit(Collision _collision);
}