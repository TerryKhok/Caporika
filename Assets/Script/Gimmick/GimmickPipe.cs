using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief   パイプギミックの移動処理
 * @memo    EntryPointのEventTriggerで発火
 *          EntryPointからExitPointまでLerpする
 */
public class GimmickPipe : MonoBehaviour
{
    public Transform entryPoint;   // パイプの入口
    public Transform exitPoint;    // パイプの出口
    public float moveDuration = 2f;   // 移動にかかる時間（秒）

    public GimmickEventTrigger eventTrigger;    // トリガー
    private bool isTransporting = false;   // プレイヤーが移動中かどうか
    private Transform playerTransform;     // プレイヤーのTransform

    /**
     * @brief   イベントトリガーのOnTriggerEnter2Dで呼び出される
     *          Lerpを開始する
     */
    public void Triggered()
    {
        Collider2D collider = eventTrigger.GetTriggeredCollider();
        if (!isTransporting)
        {
            playerTransform = collider.transform;
            StartCoroutine(TransportPlayer());
        }
    }

    // プレイヤーを出口までスムーズに移動させるコルーチン
    private IEnumerator TransportPlayer()
    {
        isTransporting = true;
        // 移動系をリセットして操作を切る
        PlayerMove playerMove = playerTransform.GetComponent<PlayerMove>();
        playerMove.enabled = false;
        playerTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;


        float elapsedTime = 0f;  // 経過時間を追跡

        while (elapsedTime < moveDuration)
        {
            // 経過時間に基づいてLerpを計算
            playerTransform.position = Vector3.Lerp(entryPoint.position, exitPoint.position, elapsedTime / moveDuration);
            playerTransform.rotation = this.transform.parent.rotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最後に出口位置に正確に配置
        playerTransform.position = exitPoint.position;

        // プレイヤーが出口に到達
        // 操作復活と速度リセット
        playerMove.enabled = true;
        playerTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        isTransporting = false;
    }
}
