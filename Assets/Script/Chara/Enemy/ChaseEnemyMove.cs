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

    private int stepSoundTimer = 0;     // 足音を鳴らすためのタイマー

    // Start is called before the first frame update
    void Start()
    {
        // アニメーターの取得
        if (!this.animator)
        {
            animator = GetComponent<Animator>();
        }
        if (!this.animator)
        {
            Debug.LogError("Animatorを取得できませんでした。");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    { }

    private void FixedUpdate()
    {
        // アニメーションが終了していれば動作アニメーションを実行しない
        if (this.isEndMoveAnimation) { return; }

        // 攻撃範囲に入ったとき
        if (this.isAttacking)
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

        stepSoundTimer++;
        if (stepSoundTimer > 20)
        {
            stepSoundTimer = 0;
            List<string> soundList = new List<string>
            {
                "ENEMY_STEP_1","ENEMY_STEP_2",
                "ENEMY_STEP_3","ENEMY_STEP_4",
                "ENEMY_STEP_5","ENEMY_STEP_6",
                "ENEMY_STEP_7","ENEMY_STEP_8",
            };
            SoundManager.Instance.PlayRandomSE(soundList);
        }
    }

    /**
     * @brief 	子オブジェクトのトリガー判定を受け取る関数
     * @param  Collider2D _other   
     * 
     * memo    ChildTriggerNotifier.cs に親にトリガー判定を渡す処理を記載
    */
    public void OnChildTriggerEnter2D(Collider2D _other)
    {
        // マトリョーシカの後ろに来たとき
        if (_other.CompareTag("Player"))
        {
            // マトリョーシカが死んでいなければ
            PlayerMove playerMove = _other.GetComponent<PlayerMove>();
            if (playerMove.playerCondition != PlayerState.PlayerCondition.Dead)
            {
                // 「攻撃を行う」
                this.isAttacking = true;

                List<string> soundList = new List<string> { "ENEMY_ROAR_1", "ENEMY_ROAR_2" };
                SoundManager.Instance.PlayRandomSE(soundList);

                // プレイヤーをゲームオーバーにする
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
