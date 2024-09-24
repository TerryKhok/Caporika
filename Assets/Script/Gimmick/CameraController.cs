using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   カメラを指定位置まで移動させる処理
 * 
 *          ・設定方法
 *              ・cameraTargetsにカメラを移動させたい位置に置いたオブジェクトを順番に入れる
 * 
 *              ・GimmickEventTriggerをつけたオブジェクトを配置して
 *              カメラを参照->MoveToNwxtPosition()を呼び出すように設定する
 * 
 * @memo    ・設定した次の位置まで移動させる処理
 *          ・上の処理のコルーチン
 *       
 */
public class CameraController : MonoBehaviour
{
    public Transform[] cameraTargets;    ///< カメラの移動位置指定に使用するGameObjectのリスト
    private int currentTargetIndex = 0;  ///< 現在のターゲット位置インデックス

    /**
     * @brief 次の位置に移動する
     * 
     * @memo EventTriggerでトリガーさせる
     */
    public void MoveToNextPosition()
    {
        if (this.currentTargetIndex < this.cameraTargets.Length)
        {
            Transform targetPosition = this.cameraTargets[this.currentTargetIndex];
            StartCoroutine(this.MoveCamera(targetPosition.position));
            this.currentTargetIndex++;
        }
    }

    /**
     * @brief 一定時間かけて指定の位置までLerpする
     */
    private IEnumerator MoveCamera(Vector3 _targetPosition)
    {
        float duration = 1.0f;  // 移動にかかる時間
        float elapsedTime = 0;

        Vector3 startingPosition = this.transform.position;

        // 移動
        while (elapsedTime < duration)
        {
            this.transform.position = Vector3.Lerp(startingPosition, _targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.position = _targetPosition;
    }
}

