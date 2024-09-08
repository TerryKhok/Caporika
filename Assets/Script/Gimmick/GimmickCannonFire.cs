using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   大砲本体に付けるスクリプト
 * 
 * @memo    ・当たるとプレイヤーが一瞬透明になる
 *          ・透明になった一定時間後にCannonProjectileにのせて飛ばす
 *          ・飛ばすときにプレイヤーのコンポーネントをいくつか無効にする
 */
public class GimmickCannonFire : MonoBehaviour
{
    public GameObject barrel;  ///< 発射口となる子オブジェクト
    public GameObject projectilePrefab;  ///< 発射する球のプレハブ
    public float delayBeforeFire = 0.5f;  ///< 発射までの遅延時間（デフォルト 0.5秒）
    public float projectileSpeed = 10.0f;  ///< 球の速度

    private GameObject player;  ///< プレイヤーを追従させるための参照

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 透明化
            player = other.gameObject;
            player.GetComponent<Renderer>().enabled = false;

            // 発射の遅延処理を開始
            Invoke("FireProjectile", delayBeforeFire);
        }
    }

    private void FireProjectile()
    {
        // 透明化を解除
        player.GetComponent<Renderer>().enabled = true;

        // 発射口から球を生成し、向いている方向に発射
        GameObject projectile = Instantiate(projectilePrefab, this.transform.position, barrel.transform.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

        // 向きの計算
        Vector3 vec = (barrel.transform.position - projectile.transform.position).normalized * projectileSpeed;
        projectileRb.AddForce(vec, ForceMode2D.Impulse);

        // プレイヤーを球に追従させる
        player.transform.parent = projectile.transform;  // 球の子オブジェクトとしてプレイヤーを追従させる
        player.transform.position = projectile.transform.position;  // 球の位置にプレイヤーをTP
        player.GetComponent<Rigidbody2D>().simulated = false;       // 重力で落ちないようにする
        player.GetComponent<Collider2D>().enabled = false;          // プレイヤーの当たり判定で止まらないようにする

        // 球が壁に当たったら消える処理
        GimmickCannonProjectile projectileScript = projectile.GetComponent<GimmickCannonProjectile>();
        if (projectileScript != null)
        {
            projectileScript.SetPlayerFollowedObject(player);  // プレイヤーの参照を渡す
        }
    }
}
