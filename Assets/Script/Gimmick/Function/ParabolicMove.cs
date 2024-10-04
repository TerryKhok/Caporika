using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   放物線移動させるスクリプト
 * 
 * @memo    
 */
public class ParabolicMove : MonoBehaviour
{
    private Transform targetPosition; ///< 移動先の座標
    public float moveSpeed = 10.0f; ///< 移動速度
    public float height = 2.0f; ///< 最大の高さ
    private bool isMoving = false; ///< 移動中かどうか
    private Vector3 startPosition; ///< 開始位置
    private float journeyLength; ///< 移動距離
    private float startTime; ///< 開始時刻
    private bool selfRemove; ///< 終了時に自動的にスクリプトを削除する

    /**
     * @brief   移動時に使う情報の初期化
     * 
     * @param   _target 移動目標のTransform
     * @param   _speed  移動するときの速度
     * @param   _height 移動時の高さの最大値
     * @param   _selfRemove 実行後にこのスクリプトを自動削除する
     */
    public void Init(Transform _target, float _speed, float _height, bool _selfRemove)
    {
        this.targetPosition = _target;
        this.moveSpeed = _speed;
        this.height = _height;
        this.selfRemove = _selfRemove;
    }

    /**
     * @brief   移動を開始する
     */
    public void Active()
    {
        this.startPosition = transform.position; // 開始位置を記録
        this.journeyLength = Vector3.Distance(this.startPosition, this.targetPosition.position); // 移動距離を計算
        this.startTime = Time.time; // 開始時刻を記録
        this.isMoving = true; // 移動を開始
    }

    /**
     * 移動の具体的な処理
     */
    public void Update()
    {
        // 移動中であれば山なりに移動
        if (this.isMoving)
        {
            float distCovered = (Time.time - this.startTime) * this.moveSpeed; // 経過した距離
            float fractionOfJourney = distCovered / this.journeyLength; // 全体に対する割合

            // 放物線を計算
            float yOffset = this.height * Mathf.Sin(fractionOfJourney * Mathf.PI); // 最大の高さまでの計算
            transform.position = Vector3.Lerp(this.startPosition, this.targetPosition.position, fractionOfJourney);
            transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

            // 目標位置に近づいたら移動を停止
            if (fractionOfJourney >= 1.0f)
            {
                this.isMoving = false; // 移動を停止
                transform.position = this.targetPosition.position; // 最終位置に設定
                if (this.TryGetComponent(out Rigidbody2D rb))
                {
                    // 終了時に変な慣性が残らないようにする
                    rb.velocity = Vector3.zero; rb.angularVelocity = 0;
                }
                if (this.selfRemove) Destroy(this); // 移動終わったら自動削除
            }
        }
    }
}