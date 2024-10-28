using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief  マトリョーシカが跳ねるのを防ぐ関数
 *  @memo   地面と距離が一定以上離れたら現在座標の少し下にする   2024/10/22更新
*/
public class PreventBounce : MonoBehaviour
{
    public float desiredForce = 1.0f;           // 相殺する力の度合い
    float groundDistance = 0.5f;                // 地面との距離
    private Rigidbody2D rb = null;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        if (!this.rb)
        {
            Debug.LogError("Rigidbody2Dを取得できませんでした。");
            return;
        }
    }

    private void OnEnable()
    {
        //Debug.Log("Onになった");
        if (!this.rb) this.rb = this.GetComponent<Rigidbody2D>();
        if (!this.rb)
        {
            Debug.LogError("Rigidbody2Dを取得できませんでした。");
            return;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = 0.0f;
        Vector2 origine = this.rb.worldCenterOfMass - new Vector2(0.0f, 0.0f);

        // 自身を無視するレイヤーマスクを設定
        int playerLayer = LayerMask.NameToLayer("player");
        int triggerArea = LayerMask.NameToLayer("TriggerArea");
        int groundLayer = LayerMask.NameToLayer("ground");

        int layerMask = (1 << playerLayer) | (1 << groundLayer) | (1 << triggerArea);
        layerMask = ~layerMask;

        // マトリョーシカの重心から下方向にRayを発射
        RaycastHit2D hit = Physics2D.Raycast(origine, Vector2.down, 10.0f, layerMask);
        //Debug.Log(hit.collider.tag);

        // 当たったオブジェクトが地面の時
        if (hit.collider != null && hit.collider.CompareTag("realGround"))
        {
            // 距離を測る
            distance = hit.distance;
            // Debug.Log("hit.distance" + hit.distance);
        }
        else { Debug.Log("当たっていない" + hit.distance); }
        // Rayを可視化
        Debug.DrawRay(origine, Vector2.down * 10, Color.blue);

        // y軸のベクトルを相殺する
        this.rb.AddForce(-Vector2.up * this.desiredForce, ForceMode2D.Force);

        // 地面との距離が一定以上離れたとき（X方向のスケールに応じて離れたと判定する距離を変化させる）
        if (distance > this.groundDistance * this.transform.localScale.x / 2)
        {
            this.rb.transform.position = new Vector3(this.rb.transform.position.x, this.rb.transform.position.y - 0.1f, this.rb.transform.position.z);
            //Debug.Log("前フレームの座標にする");
        }
    }
}