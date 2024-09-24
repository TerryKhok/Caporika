using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   大砲が飛ばす球に付けるスクリプト
 * 
 * @memo    ・一定時間後に破壊される
 *          ・壁に当たったら破壊される
 */
public class GimmickCannonProjectile : MonoBehaviour
{
    public float lifeTime = 5.0f;  ///< 壁に当たらなくても消えるまでの時間（デフォルト 5秒）
    private GameObject playerFollowedObject; ///< ついてきてるプレイヤー

    private void Start()
    {
        // 一定時間後に球を破壊
        Invoke("DestroyThis", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 壁に当たったら自壊
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("realGround"))
        {
            DestroyThis();
        }
    }

    /**
     * @brief   付いてこさせるオブジェクトを設定する
     * 
     * @param   _playerFollowedObject ついてくるゲームオブジェクト
     */
    public void SetPlayerFollowedObject(GameObject _playerFollowedObject)
    {
        this.playerFollowedObject = _playerFollowedObject;
    }

    /**
     * @brief   自壊時の処理
     * 
     * @memo    GimmickCannonFireの方で変更した項目を元に戻してから自壊
     */
    void DestroyThis()
    {
        // プレイヤーの追従を解除
        if (playerFollowedObject != null)
        {
            playerFollowedObject.transform.parent = null;  // プレイヤーを元の状態に戻す
        }
        playerFollowedObject.GetComponent<Rigidbody2D>().simulated = true;
        playerFollowedObject.GetComponent<Collider2D>().enabled = true;
        // 壁に当たったら球を破壊
        Destroy(this.gameObject);
    }
}

