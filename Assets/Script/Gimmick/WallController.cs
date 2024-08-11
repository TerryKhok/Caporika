using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief 	壁の動き
 * 
 *  @memo   壁の当たり判定を消したりつけたりしているので複数体が通ることを想定できていない
*/
public class WallController : MonoBehaviour
{
    public int wallSize;    // 壁のサイズ

    private bool isTrigger = false;
    private int playerSize = 0;                 // 通るマトリョーシカの大きさ
    private BoxCollider2D wallCollider = null;  // 壁の当たり判定

    // Start is called before the first frame update
    void Start()
    {
        wallCollider=GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーが通ろうとしたとき
        if(isTrigger)
        {
            // プレイヤーのサイズが壁のサイズよりも小さいとき
            if(playerSize　<= wallSize)
            {
                // 壁の当たり判定を消して「通れる」
                wallCollider.enabled=false;
            }
            else
            {
                // 壁の当たり判定を付けて「通れない」
                wallCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーが入った時
        if(collision.CompareTag("TriggerArea"))
        {
            isTrigger = true;

            // マトリョーシカのサイズを取得
            playerSize= collision.gameObject.GetComponentInParent<CharaState>().GetCharaSize();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // プレイヤーが出たとき
        if (collision.CompareTag("TriggerArea"))
        {
            // リセット
            isTrigger = false;
            playerSize = 0;
        }
    }
}
