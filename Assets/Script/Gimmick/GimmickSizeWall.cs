using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

/**
 * @brief   サイズに応じて通れる壁
 * 
 * @memo    ・壁に触れそうになるとオブジェクトごとに接触するかを判定する処理
 * 
 */
public class GimmickSizeWall : MonoBehaviour
{
    public int wallSize;    ///< 通れるサイズ（2ならCharaSize2と1が通れる

    private Collider2D wallCollider;    ///< 壁のコライダー

    // Start is called before the first frame update
    void Start()
    {
        this.wallCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 接触しそうなオブジェクトのCharaStateを取得
        CharaState charaState = collision.gameObject.GetComponent<CharaState>();
        if (charaState != null) // nullチェック
        {
            int charaSize = charaState.GetCharaSize();  // サイズ取得

            if (charaSize <= this.wallSize)
            {
                // 通れるサイズなら当たり判定を無効化
                Physics2D.IgnoreCollision(collision, this.wallCollider);
            }
        }
    }
}
