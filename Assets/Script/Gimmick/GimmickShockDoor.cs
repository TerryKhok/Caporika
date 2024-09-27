using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ぶつかって倒れるドアのスクリプト
 *          ぶつかった時の速度を参照して倒れるか決める
 *          倒れる時の挙動はアニメーションで作成
 *          
 *          アニメーションパラメータの
 *          bool型Shockedがtrueになります
 * 
 * @memo    
 *          ・飛んでるか判定する処理
 *          ・倒れる時の処理（判定はアニメーションで消す予定）
 */
public class GimmickShockDoor : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponentInParent<Animator>();
        this.animator.SetBool("Shocked", false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 衝突判定
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        // プレイヤーのみ反応
        if (_collision.gameObject.CompareTag("Player"))
        {
            var state = _collision.gameObject.GetComponent<PlayerMove>();
            if (state.playerCondition == PlayerState.PlayerCondition.Flying)
            {
                // 飛んでる場合は倒す
                Shocked();
            }
        }
    }

    // アニメーション再生
    public void Shocked()
    {
        Debug.Log("Shocked");
        this.animator.SetBool("Shocked", true);
    }
}
