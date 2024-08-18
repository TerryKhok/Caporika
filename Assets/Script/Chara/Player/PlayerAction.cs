using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/**
 *  @brief 	キャラの行動をまとめた
 *  
 *  @memo   ・飛び出る処理
 *          ・中に入る処理
 *          ・攻撃
*/
public class PlayerAction : MonoBehaviour
{
    //===============================================
    //                  キー
    //===============================================
    public KeyCode  popOutkey = KeyCode.Space;          // 飛び出すアクションを行う
    public KeyCode nestInsideKey = KeyCode.LeftShift;   // マトリョーシカに入るアクションを行う


    //===============================================
    //          マトリョーシカ
    //===============================================

    public float jumpForce = 10.0f;         // 力の大きさ
    public string tagName;                  // マトリョーシカの親タグ名
    public float knockbackAngle;            // 攻撃された時のノックバックする角度
    public float knockbackpForce;           // ノックバックする威力

    private PlayerMove matryoishkaMove = null;              // 自身の状態
    private MatryoshkaManager matryoshkaManager = null;     // マトリョーシカの管理
    private int matryoishkaSize = 0;                       // 自身のサイズ
    private Rigidbody2D matryoshkaRb = null;               // 自身のrigidbody2d


    //===============================================
    //          当たっているオブジェクト
    //===============================================

    private bool isSametag = false;                     // 同じTag名
    private Collider2D currentTrigger = null;           // 今当たっているオブジェクト
    private PlayerMove triggerMove = null;              // 当たっているオブジェクトの状態
    private int triggerSize = 0;                        // 今当たっているオブジェクトのサイズ

    private bool isCollEnemy = false;                   // 敵とぶつかった
    private Collider2D enemyColl=null;                  // 敵の当たり判定

    private void Start()
    {
        this.matryoishkaMove = GetComponent<PlayerMove>();                      // このマトリョーシカの動作状態
        this.matryoishkaSize = GetComponent<CharaState>().GetCharaSize();       // このマトリョーシカの大きさ
        this.matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();
        this.matryoshkaRb =GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 飛び出すアクションを行う----------------------------------------------------
        if (Input.GetKeyDown(this.popOutkey))
        {
            // 自分が一番小さくない
            if (this.matryoishkaSize > 1)
            {
                // 飛び出す処理
                PopOut();
                // このマトリョーシカの状態を「死んだ」に
                this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);

                // このスクリプトを無効化
                this.enabled = false;
            }
            return;
        }

        // マトリョーシカに入るアクションを行う----------------------------------------------------
        if (Input.GetKeyDown(this.nestInsideKey))
        {
            // タグ名が同じで、自分よりも1個大きいマトリョーシカのとき
            if (this.isSametag && this.matryoishkaSize + 1 == this.triggerSize)
            {
                // 入る処理
                NestInside();
            }
            return;
        }

        // 敵と当たっていて、敵が死んでいない
        if (this.isCollEnemy && enemyColl.GetComponent<TenpState>().GetCharaState() != TenpState.State.Dead)
        {
            // 自身が飛んでいるとき
            if (this.matryoishkaMove.playerCondition == PlayerState.PlayerCondition.Flying)
            {
                // 攻撃する
                Attack();
            }
            else
            {
                // ダメージを受ける
                Damage();

                // 自身の状態を「死んだ」に
                this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                // このスクリプトを無効化
                this.enabled = false;

                Debug.Log("ダメージを受けた！");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // タグ名が同じとき、当たったオブジェクトを保持
        if (other.CompareTag(tagName))
        {
            this.isSametag = true;
            this.currentTrigger = other;
            this.triggerSize = other.gameObject.GetComponentInParent<CharaState>().GetCharaSize();   // マトリョーシカのサイズ
            this.triggerMove = other.gameObject.GetComponentInParent<PlayerMove>();                  // マトリョーシカの状態
        }

        // 敵にぶつかった時、当たった敵オブジェクトを保持
        if (other.CompareTag("Enemy"))
        {
            this.isCollEnemy = true;
            this.enemyColl = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 同じタグから出たとき、リセット
        if (other.CompareTag(tagName))
        {
            this.isSametag = false;
            this.currentTrigger = null;
            this.triggerMove = null;
        }        

        // 敵と離れたらリセット
        if(other.CompareTag("Enemy"))
        {
            this.isCollEnemy = false;
            this.enemyColl = null;
        }
    }

    /**
     * @brief 	マトリョーシカの中に入る処理
    */
    void NestInside()
    {        
        // 残機を増やす
        this.matryoshkaManager.AddLife();

        // 親のマトリョーシカのスクリプトを取得
        PlayerAction triggerAction = this.currentTrigger.GetComponentInParent<PlayerAction>();

        // 入るマトリョーシカにアタッチされている行動スクリプトを有効に
        if (triggerAction)
        {
            triggerAction.enabled = true;
            this.triggerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Ground);
        }
        else { Debug.LogError("入るマトリョーシカにPlayerActionがアタッチされていません"); }


        // このオブジェクトを消す
        Destroy(this.gameObject);
    }

    /**
     * @brief 	マトリョーシカから飛び出る処理
    */
    void PopOut()
    {
        // 残機を減らす
        int curLife = this.matryoshkaManager.LoseLife();
        if (curLife <= 0) { return; }

        // 現在のマトリョーシカの位置、角度を取得
        Vector2 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        // 自身より一段階小さいマトリョーシカを生成
        GameObject newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (!newMatryoshka)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }

        // 外側のマトリョーシカと同じ回転、座標にセット
        newMatryoshka.transform.position = position;
        newMatryoshka.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = newMatryoshka.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("生成したマトリョーシカにRigidbody2Dが存在しなかったので、追加しました。");
            rb = newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // オブジェクトの傾いた方向に飛んでいく
        Vector2 jumpDirection = this.transform.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        // 飛び出たマトリョーシカを「飛んだ」状態に
        PlayerMove newMatoryoshkaMove = newMatryoshka.GetComponent<PlayerMove>();
        if (newMatoryoshkaMove)
        {
            newMatoryoshkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
        }
        else{ Debug.LogError("生成したマトリョーシカのPlayerMoveを取得できませんでした。"); }
    }

    /**
     * @brief 	攻撃を受けたときの処理
     * 
     * @memo     攻撃を受けた方とは反対方向に中身を飛ばす
    */
    void Damage()
    {
        // 残機を減らす
        int curLife = this.matryoshkaManager.LoseLife();
        if (curLife <= 0){ return; }

        // 攻撃を受けた向きから飛び出る角度を決定
        float direction = this.transform.position.x - this.enemyColl.gameObject.transform.position.x;
        float moveAngle = 0.0f;
        float angle = 0.0f;

        // 左向き
        if (direction < 0.0f) 
        { 
            moveAngle = 180.0f - this.knockbackAngle;
            angle = this.knockbackAngle;
        }
        // 右向き
        else
        { 
            moveAngle = this.knockbackAngle;
            angle = 360.0f - this.knockbackAngle;
        }

        // 角度を設定
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, this.transform.eulerAngles.y, angle);

        // 自身より一段階小さいマトリョーシカを生成
        if (this.matryoishkaSize <= 0) { return; }
        GameObject newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (newMatryoshka == null)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }

        // 現在のマトリョーシカの位置、角度を取得
        Vector2 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        // 生成したマトリョーシカを現在のマトリョーシカと同じ回転、座標にセット
        newMatryoshka.transform.position = position;
        newMatryoshka.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = newMatryoshka.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("生成したマトリョーシカにRigidbody2Dが存在しなかったので、追加しました。");
            rb = newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // 角度をラジアンに変換
        float angleInRadians = moveAngle * Mathf.Deg2Rad;

        // 飛び出す方向ベクトルを計算する
        Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized * this.knockbackpForce;   
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // 飛び出たマトリョーシカは「ダメージ」状態に
        PlayerMove newMatoryoshkaMove = newMatryoshka.GetComponent<PlayerMove>();
        if (newMatoryoshkaMove != null)
        {
            newMatoryoshkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Damaged);
        }
        else { Debug.LogError("生成したマトリョーシカのPlayerMoveを取得できませんでした。"); }
    }

    /**
     * @brief 	攻撃をしたときの処理
    */
    void Attack()
    {
        // 当たったオブジェクトの状態を取得
        TenpState enemyState = enemyColl.GetComponent<TenpState>();
        // 状態を「死んだ」に
        enemyState.SetCharaState(TenpState.State.Dead);

        Debug.Log("攻撃した");
    }
}