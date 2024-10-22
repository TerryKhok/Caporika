using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    public string playerObjname;                    // 生成したマトリョーシカを配置する親プレイヤーオブジェクト名
    private GameObject playerObject = null;         // 生成したマトリョーシカを配置する親プレイヤーオブジェクト
    private GameObject newMatryoshka = null;        // 新しいマトリョーシカ

    private Animator animator;              // プレイヤーのアニメーター

    public float jumpForce = 10.0f;         // 力の大きさ
    public string tagName;                  // マトリョーシカの親タグ名
    public float knockbackAngle;            // 攻撃された時のノックバックする角度
    public float knockbackpForce;           // ノックバックする威力

    public float actionTime;                // 再びジャンプできるまでの時間

    private PlayerMove matryoishkaMove = null;              // 自身の状態
    private MatryoshkaManager matryoshkaManager = null;     // マトリョーシカの管理
    private int matryoishkaSize = 0;                        // 自身のサイズ
    private Rigidbody2D matryoshkaRb = null;                // 自身のrigidbody2d

    private bool isJump = false;                            // true:ジャンプできる
    private float time = 0.0f;                              // カウント用

    [Header("ジャンプさせる角度(各々0.0~37.5で設定して)")]
    public float outerAngle;    // 外側の角度
    public float innerAngle;    // 内側の角度

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
        // このマトリョーシカの動作状態
        this.matryoishkaMove = GetComponent<PlayerMove>();                      
        if (!this.matryoishkaMove)
        {
            Debug.LogError("PlayerMoveを取得できませんでした。");
            return;
        }

        // このマトリョーシカの大きさ
        this.matryoishkaSize = GetComponent<CharaState>().GetCharaSize();

        // プレイヤーの管理
        this.matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();     
        if (!this.matryoshkaManager)
        {
            Debug.LogError("MatryoshkaManagerを取得できませんでした。");
            return;
        }

        // プレイヤーのRigidbody2D
        this.matryoshkaRb =GetComponent<Rigidbody2D>();                        
        if (!this.matryoshkaRb)
        {
            Debug.LogError("Rigidbody2Dを取得できませんでした。");
            return;
        }

        // プレイヤーのアニメーター
        this.animator = GetComponent<Animator>();                               
        if (!this.animator)
        {
            Debug.LogError("Animatorを取得できませんでした。");
            return;
        }

        // プレイヤー
        this.playerObject = GameObject.Find(this.playerObjname);
        if (!this.playerObject)
        {
            Debug.LogError("playerオブジェクトが見つからず、取得できませんでした。");
            return;
        }
    }

    private void FixedUpdate()
    {
        // 一定時間飛べないようにする
        if (!this.isJump)
        {
            this.time += Time.deltaTime;
        }
        if(this.time>this.actionTime)
        {
            this.isJump = true;
            this.time = 0.0f;
        }
    }

    void Update()
    {
        if(this.isJump)
        {
            // 飛び出すアクションを行う----------------------------------------------------
            if (Input.GetKeyDown(this.popOutkey))
            {
                // 自分が一番小さくない
                if (this.matryoishkaSize > 1)
                {
                    // 開くアニメーション
                    this.animator.SetTrigger("openTrigger");

                    // 飛び出す処理
                    PopOut();
                    this.isJump = false;
                    // このマトリョーシカの状態を「死んだ」に
                    this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                    // 攻撃不発状態
                    this.newMatryoshka.GetComponent<PlayerMove>().SetAttackState(PlayerState.AttackState.Failed);
                }
                return;
            }
        }

        // マトリョーシカに入るアクションを行う----------------------------------------------------
        if (Input.GetKeyDown(this.nestInsideKey))
        {
            // タグ名が同じで、自分よりも1個大きいマトリョーシカのとき
            if (this.isSametag && this.matryoishkaSize + 1 == this.triggerSize)
            {
                // 入るアニメーション？？

                // 入る処理
                NestInside();
            }
            return;
        }

        // 敵と当たっていて、敵が死んでいない
        if (this.isCollEnemy && enemyColl.GetComponent<CharaMove>().GetCharaCondition() != CharaMove.CharaCondition.Dead)
        {
            // 自身が飛んでいるとき
            if (this.matryoishkaMove.playerCondition == PlayerState.PlayerCondition.Flying)
            {
                // 攻撃する
                Attack();
                // 攻撃成功状態
                this.matryoishkaMove.SetAttackState(PlayerState.AttackState.Success); 
            }
            else
            {
                // ダメージを受ける
                Damage();

                // 自身の状態を「死んだ」に
                this.matryoishkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
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
        this.newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (!this.newMatryoshka)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }
        // Playerタブ内に生成したマトリョーシカを配置
        this.newMatryoshka.transform.parent = this.playerObject.transform;

       // 飛び出す範囲によって生成したマトリョーシカの飛び出す角度を決める
        Vector3  angle = this.transform.eulerAngles;
        if ((angle.z >= 0.0f && angle.z < 15.0f)|| (angle.z >= 345.0f && angle.z < 360.0f)) 
        { 
            rotation = Quaternion.Euler(angle.x, angle.y, 0.0f);
            //Debug.Log("真上");
        }
        else if (angle.z >= 15.0f && angle.z < 52.5f)
        {
            rotation = Quaternion.Euler(angle.x, angle.y, (15.0f + this.outerAngle));
            //Debug.Log("左内側");
        }
        else if (angle.z >= 52.5f && angle.z < 90.0f) 
        { 
            rotation = Quaternion.Euler(angle.x, angle.y, (52.5f + this.innerAngle));
            //Debug.Log("左外側");
        }
        else if (angle.z >= 270.0f && angle.z < 307.5) 
        { 
            rotation = Quaternion.Euler(angle.x, angle.y, (270.0f + this.outerAngle));
            //Debug.Log("右外側");
        }
        else if (angle.z >= 307.5 && angle.z < 345.0f) 
        {
            rotation = Quaternion.Euler(angle.x, angle.y, (307.5f+this.innerAngle));
            //Debug.Log("右内側");
        }

        //Debug.Log("かくど:" + this.transform.eulerAngles);

        // 回転、座標をセット
        this.newMatryoshka.transform.position = position;
        this.newMatryoshka.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = this.newMatryoshka.GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("生成したマトリョーシカにRigidbody2Dが存在しなかったので、追加しました。");
            rb = this.newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // マトリョーシカの傾いている方向に飛んでいく
        Vector2 jumpDirection = this.newMatryoshka.transform.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        // 飛び出たマトリョーシカを「飛んだ」状態に
        PlayerMove newMatoryoshkaMove = this.newMatryoshka.GetComponent<PlayerMove>();
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
        this.newMatryoshka = this.matryoshkaManager.InstanceMatryoshka(this.matryoishkaSize - 1);
        if (this.newMatryoshka == null)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }
        // Playerタブ内に生成したマトリョーシカを配置
        this.newMatryoshka.transform.parent = this.playerObject.transform;

        // 現在のマトリョーシカの位置、角度を取得
        Vector2 position = this.transform.position;
        Quaternion rotation = this.transform.rotation;

        // 生成したマトリョーシカを現在のマトリョーシカと同じ回転、座標にセット
        this.newMatryoshka.transform.position = position;
        this.newMatryoshka.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = this.newMatryoshka.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("生成したマトリョーシカにRigidbody2Dが存在しなかったので、追加しました。");
            rb = this.newMatryoshka.AddComponent<Rigidbody2D>();
        }

        // 角度をラジアンに変換
        float angleInRadians = moveAngle * Mathf.Deg2Rad;

        // 飛び出す方向ベクトルを計算する
        Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized * this.knockbackpForce;   
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // 飛び出たマトリョーシカは「飛んだ」状態に!!

        PlayerMove newMatoryoshkaMove = this.newMatryoshka.GetComponent<PlayerMove>();
        if (newMatoryoshkaMove != null)
        {
            newMatoryoshkaMove.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
        }
        else { Debug.LogError("生成したマトリョーシカのPlayerMoveを取得できませんでした。"); }
    }

    /**
     * @brief 	攻撃をしたときの処理
    */
    void Attack()
    {
        // 当たったオブジェクトの状態を取得
        CharaMove enemyState = enemyColl.GetComponent<CharaMove>();
        // 状態を「死んだ」に
        enemyState.SetCharaCondition(CharaMove.CharaCondition.Dead);

        Debug.Log("攻撃した");
    }
}