using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   後ろから追いかけてくる敵
 * 
 * @memo    ・ マトリョーシカの後ろに来たとき攻撃アニメーションを実行する
 */
public class ChaseEnemyMove : MonoBehaviour
{
    public Animator animator = null;    // アニメーター
    public int layerIndex = 0;          // 停止したいレイヤーのインデックス

    public string attackAnimTrigger = "AttackTrigger";  // 攻撃トリガー
    public string attackTrigger = "enemyAttack";        // 攻撃する状態になる範囲

    private bool isAttacking = false;                   // true:攻撃する
    private bool isEndMoveAnimation = false;            // true:動作アニメーションを終了した

    private float rayDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // アニメーターの取得
        if(!this.animator)
        {
            animator = GetComponent<Animator>();
        }
        if (!this.animator)
        {
            Debug.LogError("Animatorを取得できませんでした。");
            return;
        }
    }

     void Update()
    {
        Vector2 origin = transform.position;
        int layerMask = ~(1 << 10); // レイヤー8（HunterLayer）を無視するマスク

        // 上方向にRayを飛ばす
        RaycastHit2D hitUp = Physics2D.Raycast(origin, Vector2.up, rayDistance, layerMask);
        if (hitUp.collider != null)
        {

            if (hitUp.collider.tag == "Player")
            {
                Debug.Log("上方向にPlayerにヒット");

                PlayerMove playerMove = hitUp.collider.GetComponent<PlayerMove>();
                Debug.Log(playerMove);
                if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                {
                    this.isAttacking = true;

                    MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                    if (!playerManager)
                    {
                        Debug.LogError("MatryoshkaManagerを取得できませんでした。");
                        return;
                    }
                    int currentLife = playerManager.GetCurrentLife();
                    playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                    playerManager.GameOver();
                }
            }
            else if (hitUp.collider.tag == "TriggerArea")
            {
                Transform parentTransform = hitUp.collider.transform.parent;

                // 親オブジェクトが存在する場合
                if (parentTransform != null)
                {

                    PlayerMove playerMove = parentTransform.GetComponent<PlayerMove>();
                    Debug.Log(playerMove);
                    if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                    {
                        this.isAttacking = true;

                        MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                        if (!playerManager)
                        {
                            Debug.LogError("MatryoshkaManagerを取得できませんでした。");
                            return;
                        }
                        int currentLife = playerManager.GetCurrentLife();
                        playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                        playerManager.GameOver();
                    }
                }
            }
            else
            {
                //Debug.Log("上方向: " + hitUp.collider.tag);
            }
        }

        // 下方向にRayを飛ばす
        RaycastHit2D hitDown = Physics2D.Raycast(origin, Vector2.down, rayDistance, layerMask);
        if (hitDown.collider != null)
        {
            if (hitDown.collider.tag == "Player")
            {
                Debug.Log("下方向にPlayerにヒット");

                PlayerMove playerMove = hitDown.collider.GetComponent<PlayerMove>();
                Debug.Log(playerMove);
                if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                {
                    this.isAttacking = true;

                    MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                    if (!playerManager)
                    {
                        Debug.LogError("MatryoshkaManagerを取得できませんでした。");
                        return;
                    }
                    int currentLife = playerManager.GetCurrentLife();
                    playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                    playerManager.GameOver();
                }
            }
            else if (hitDown.collider.tag == "TriggerArea")
            {
                Transform parentTransform = hitDown.collider.transform.parent;
                Debug.Log("親 :" + parentTransform);

                // 親オブジェクトが存在する場合
                if (parentTransform != null)
                {

                    PlayerMove playerMove = parentTransform.GetComponent<PlayerMove>();
                    Debug.Log(playerMove);
                    if (playerMove != null && playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
                    {
                        this.isAttacking = true;

                        MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                        if (!playerManager)
                        {
                            Debug.LogError("MatryoshkaManagerを取得できませんでした。");
                            return;
                        }
                        int currentLife = playerManager.GetCurrentLife();
                        playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                        playerManager.GameOver();
                    }
                }
                else
                {
                    Debug.Log("下方向:" + hitDown.collider.tag);
                }
            }
        }
    }


    private void FixedUpdate()
    {
        // アニメーションが終了していれば動作アニメーションを実行しない
        if (this.isEndMoveAnimation) { return; }

        // 攻撃範囲に入ったとき
        if(this.isAttacking)
        {
            // 追跡アニメーションの終了
            this.animator.SetTrigger("ChaseEndTrigger");

            // 攻撃アニメーションの再生
            this.animator.SetTrigger("AttackTrigger");
            Debug.Log("動作アニメーションレイヤーの一時停止");
            this.isAttacking = false;
        }

        // 攻撃アニメーションが終わった時、動作アニメーションレイヤーを終了する
        if (this.animator.GetCurrentAnimatorStateInfo(this.layerIndex).IsName("Wolf_Walk"))
        {
            this.isEndMoveAnimation = true;
            Debug.Log("動作アニメーションレイヤーを終了");
        }
    }

    /**
     * @brief 	子オブジェクトのトリガー判定を受け取る関数
     * @param  Collider2D _other   
     * 
     * memo    ChildTriggerNotifier.cs に親にトリガー判定を渡す処理を記載
    */
    //public void OnChildTriggerStay2D(Collider2D _other)
    //{
    //    // マトリョーシカの後ろに来たとき
    //    if (_other.CompareTag("Player"))
    //    {
    //        // マトリョーシカが死んでいなければ
    //        PlayerMove playerMove = _other.GetComponent<PlayerMove>();
    //        if(playerMove.playerCondition!=PlayerState.PlayerCondition.Dead)
    //        {
    //            // 「攻撃を行う」
    //            this.isAttacking = true;

    //            // プレイヤーをゲームオーバーにする
    //            MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
    //            if (!playerManager)
    //            {
    //                Debug.LogError("MatryoshkaManagerを取得できませんでした。");
    //                return;
    //            }
    //            int currentLife = playerManager.GetCurrentLife();
    //            playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
    //            playerManager.GameOver();
    //        }
    //    }
    //}

    /**
     * @brief 	攻撃をしたか取得する関数
     * @return  bool   this.isAttacking
     * 
     * memo    プレイヤー側で攻撃されたかに使う
    */
    public bool GetIsAttacking()
    {
        return this.isAttacking;
    }
}
