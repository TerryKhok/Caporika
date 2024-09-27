using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   棘のためのScript
 * 
 * @memo    ・移動しない
 *          ・死なない
 *          ・ぶつかったプレイヤーの殻をターゲットまで飛ばす
 */
public class EnemyMoveSpike : CharaMove
{
    public Transform damageMoveTarget = null;   // ダメージ時の目標位置
    public float damagedMoveSpeed = 3.0f;       // ダメージ時の移動の速度
    public float damagedMoveHeight = 2.0f;      // ダメージ時の移動の高さの最大値（天井にぶつからないため）


    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        // 空中にはいないし死なないし止まってるだけ
        if (this.GetCharaCondition() == CharaCondition.Dead)
        {
            this.SetCharaCondition(CharaCondition.Ground);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 戻りたい場所が決まってるならプレイヤーを飛ばす
        if (damageMoveTarget != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // 放物線移動用スクリプトをつけて飛ばす
                var move = collision.gameObject.AddComponent<ParabolicMove>();
                move.Init(damageMoveTarget.transform, damagedMoveSpeed, damagedMoveHeight, true);
                move.Active();
            }
        }
    }
}
