using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * @brief 	プレイヤーが「マトリョーシカに入ろうとしている状態」の処理を行うクラス
 * 
 *  @memo   ・PlayerStateを基底クラスに持つ
 *          ・プレイヤーの状態は、後に実装するPlayerMove.cs内で列挙型(CharaCondition、PlayerCondition)を使用して切り替える
 *          
 *          ・この状態の時移動はできない
 *          ・この時敵に当たっても攻撃判定にはならない
 *          ・入る処理が終了次第「Ground」に遷移する
 *          
 *  ========================================================================================================
 *  
 *          MonoBehaviourを継承しないためパラメータは直接いじってください！！！！！！！！！
 *          
 *  ========================================================================================================
*/
public class PlayerStateNestIn : PlayerState
{
    //private Animator animator = null;       // プレイヤーのアニメーター
    private PlayerMove playerMove = null;   // プレイヤーの動き
    private MatryoshkaManager matryoshkaManager = null;     // マトリョーシカの管理
    private Collider2D targetColl = null;        // 入るオブジェクトのコライダー

    //private Transform target;            // ジャンプ先のターゲット
    //private AnimationCurve jumpCurve;    // ジャンプの高さを制御するアニメーションカーブ
    //private float jumpDuration = 1f;     // ジャンプの時間

    //private bool isJumping = false;
    //private Vector3 startPosition;
    //private float jumpTime;

    /**
     * @brief 	この状態に入るときに行う関数
     * @paraam  PlayerMove _playerMove  
     * 
     * memo    RigidBody2Dやその他コンポーネントを取得するためのみに使用する
    */
    public override void Enter(PlayerMove _playerMove)
    {
        if (!_playerMove)
        {
            Debug.LogError("PlayerMoveが存在しません。");
        }
        this.playerMove = _playerMove;

        this.matryoshkaManager = _playerMove.GetComponent<MatryoshkaManager>();
        if (!this.matryoshkaManager)
        {
            Debug.LogError("Rigidbody2Dを取得できませんでした。");
            return;
        }
        // 残機を増やす
        this.matryoshkaManager.AddLife();

        //this.rb = _playerMove.GetComponent<Rigidbody2D>();
        //if (!this.rb)
        //{
        //    Debug.LogError("Rigidbody2Dを取得できませんでした。");
        //    return;
        //}

        //// プレイヤーのアニメーター
        //this.animator = _playerMove.GetComponent<Animator>();
        //if (!this.animator)
        //{
        //    Debug.LogError("Animatorを取得できませんでした。");
        //    return;
        //}

        //// 
        //startPosition = _playerMove.transform.position;
        //jumpTime = 0f;
        //isJumping = true;
    }

    /**
     * @brief 	この状態から出るときに行う関数
    */
    public override void Exit()
    {
       
    }

    /**
     * @brief 	更新処理
    */
    public override void Update()
    {
        //// 移動は無し

        //if (isJumping)
        //{
        //    jumpTime += Time.deltaTime;
        //    float t = jumpTime / jumpDuration;

        //    // 入ったら処理を止める
        //    if (t > 1.0f)
        //    {
        //        t = 1.0f;
        //        isJumping = false;
        //    }

        //    // パラボラ軌道を計算して対象のオブジェクトに向かってジャンプさせる
        //    Vector3 currentPosition = Vector3.Lerp(startPosition, target.position, t);
        //    currentPosition.y += jumpCurve.Evaluate(t);
        //    this.playerMove.transform.position = currentPosition;
        //}
    }

    /**
     * @brief 	当たった時の処理
     *  @param  Collider2D _collision    当たったオブジェクト
    */
    public override void CollisionEnter(Collider2D _collision)
    {
        // 同じオブジェクトの時、そのオブジェクトに向かって飛んでいく
        if (_collision.CompareTag("Player"))
        {
            this.targetColl = _collision;
        }
    }
}