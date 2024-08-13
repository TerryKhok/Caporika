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
    public float jumpForce = 10.0f;                     // 力の大きさ
    public string tagName;                              // マトリョーシカの親タグ名
    public float knockbackAngle;                         // 攻撃時のノックバックする角度
    public float knockbackpForce;

    private bool isSametag = false;                     // 同じTag名
    private Collider2D currentTrigger = null;           // 今当たっているオブジェクト
    private TenpState triggerState = null;             // 当たっているオブジェクトの状態

    private TenpState matryoishkaState = null;         // 自身の状態
    private MatryoshkaManager matryoshkaManager = null; // マトリョーシカの管理
    private int sizeState = 0;                          // 自身のサイズ
    private Rigidbody2D matryoshkaRb = null;            // 自身のrigidbody2d

    private bool isCollEnemy = false;                   // 敵とぶつかった
    private Collider2D enemyColl=null;                  // 敵の当たり判定

    private void Start()
    {
        matryoishkaState = GetComponent<TenpState>();              // このマトリョーシカの状態
        sizeState = matryoishkaState.GetCharaSize();                // このマトリョーシカの大きさ
        matryoshkaManager = FindAnyObjectByType<MatryoshkaManager>();
        matryoshkaRb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 自分が一番小さくない
            if (sizeState > 1)
            {
                // 飛び出す処理
                PopOut();
                // このマトリョーシカの状態を「死んだ」に
                matryoishkaState.SetCharaState(TenpState.State.Dead);
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // タグ名が同じで、自分よりも1個大きいマトリョーシカのとき
            if (isSametag&& sizeState + 1 == triggerState.GetCharaSize())
            {
                // 入る処理
                NestInside(); 
            }
        }

        // 敵と当たっていて、敵が死んでいなければ
        if(isCollEnemy&&enemyColl.GetComponent<TenpState>().GetCharaState()!= TenpState.State.Dead)
        {
            // 自身が飛んでいるときなら
            if (matryoishkaState.state == TenpState.State.Flying)
            {
                // 攻撃する
                Attack();
            }
            else
            {
                // ダメージを受ける
                Damage();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // タグ名が同じとき、当たったオブジェクトを保持
        if (other.CompareTag(tagName))
        {
            isSametag = true;
            currentTrigger = other;
            triggerState = other.gameObject.GetComponentInParent<TenpState>(); // マトリョーシカの状態
        }

        // 敵にぶつかった時、当たったオブジェクトを保持
        if (other.CompareTag("Enemy"))
        {
            isCollEnemy = true;
            enemyColl = other;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 同じタグから出たとき、リセット
        if (other.CompareTag(tagName))
        {
            isSametag = false;
            currentTrigger = null;
            triggerState = null;
        }        

        if(other.CompareTag("Enemy"))
        {
            isCollEnemy = false;
            enemyColl = null;
        }
    }

    /**
     * @brief 	マトリョーシカの中に入る処理
    */
    void NestInside()
    {        
        // 残機を増やす
        matryoshkaManager.AddLife();

        // 親のマトリョーシカのスクリプトを取得
        TempMove triggerMove = triggerState.GetComponentInParent<TempMove>();
        PlayerAction triggerAction = triggerState.GetComponentInParent<PlayerAction>();

        // 入るマトリョーシカにアタッチされているスクリプトを全て有効に
        if (triggerMove != null) triggerMove.enabled = true;
        if (triggerAction != null) triggerAction.enabled = true;

        // 入るマトリョーシカの状態を「通常」にする
        triggerState.GetComponentInParent<TenpState>().SetCharaState(TenpState.State.Normal);

        // このオブジェクトを消す
        Destroy(gameObject);
    }

    /**
     * @brief 	マトリョーシカから飛び出る処理
    */
    void PopOut()
    {
        // 残機を減らす
        int curLife = matryoshkaManager.LoseLife();
        if (curLife <= 0) { return; }

        // 現在のマトリョーシカの位置、角度を取得
        Vector2 position = transform.position;
        Quaternion rotation = transform.rotation;

        // 自身より一段階小さいマトリョーシカを生成
        GameObject newobj = matryoshkaManager.InstanceMatryoshka(sizeState - 1);
        if (newobj == null)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }

        // 外側のマトリョーシカと同じ回転、座標にセット
        newobj.transform.position = position;
        newobj.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = newobj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = newobj.AddComponent<Rigidbody2D>();
            rb.isKinematic = false; 
        }

        // オブジェクトの傾いた方向に飛んでいく
        Vector2 jumpDirection = transform.up * jumpForce;
        rb.AddForce(jumpDirection, ForceMode2D.Impulse);

        // 飛び出たマトリョーシカを「飛んだ」状態に
        TenpState newState = newobj.GetComponent<TenpState>();
        if (newState != null)
        {
            newState.SetCharaState(TenpState.State.Flying);
        }
        else{ Debug.LogError("newStateがnull"); }

        // 自身の状態を「死んだ」に
        matryoishkaState.SetCharaState(TenpState.State.Dead);
        matryoshkaRb.velocity = Vector2.zero;

        // このスクリプトを無効化
        enabled = false;
    }

    /**
     * @brief 	攻撃を受けたときの処理
     * 
     * @memo     攻撃を受けた方とは反対方向に中身を飛ばす
    */
    void Damage()
    {
        // 残機を減らす
        int curLife = matryoshkaManager.LoseLife();
        if (curLife <= 0){ return; }

        // 攻撃を受けた向きから飛び出る角度を決定
        float direction = transform.position.x - enemyColl.gameObject.transform.position.x;
        float moveAngle = 0.0f;
        float angle = 0.0f;

        // 左向き
        if (direction < 0) 
        { 
            moveAngle = 180.0f - knockbackAngle;
            angle = knockbackAngle;
        }
        // 右向き
        else
        { 
            moveAngle = knockbackAngle;
            angle = 360.0f - knockbackAngle;
        }

        // 角度を設定
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);

        // 自身より一段階小さいマトリョーシカを生成
        if (sizeState <= 0) { return; }
        GameObject newobj = matryoshkaManager.InstanceMatryoshka(sizeState - 1);
        if (newobj == null)
        {
            Debug.LogError("マトリョーシカの生成に失敗");
            return;
        }

        // 現在のマトリョーシカの位置、角度を取得
        Vector2 position = transform.position;
        Quaternion rotation = transform.rotation;

        // 生成したマトリョーシカを現在のマトリョーシカと同じ回転、座標にセット
        newobj.transform.position = position;
        newobj.transform.rotation = rotation;

        // 生成したマトリョーシカのRigidbody2Dを取得
        Rigidbody2D rb = newobj.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = newobj.AddComponent<Rigidbody2D>();
            rb.isKinematic = false; // 物理演算を有効化
        }

        // 角度をラジアンに変換
        float angleInRadians = moveAngle * Mathf.Deg2Rad;

        // 飛び出す方向ベクトルを計算する
        Vector2 knockbackDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized * knockbackpForce;   
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // 飛び出たマトリョーシカは「ダメージ」状態に
        TenpState newState = newobj.GetComponent<TenpState>();
        if (newState != null)
        {
            newState.SetCharaState(TenpState.State.Damaged);
        }
        else { Debug.LogError("newStateがnull"); }

        // 自身の状態を「死んだ」に
        matryoishkaState.SetCharaState(TenpState.State.Dead);
        matryoshkaRb.velocity = Vector2.zero;

        // このスクリプトを無効化
        enabled = false;

        Debug.Log("ダメージを受けた！");
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
        enemyState.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Debug.Log("攻撃した");
    }
}