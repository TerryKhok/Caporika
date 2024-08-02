/*
 * プレイヤーの移動をつかさどるスクリプト。
 * マトリョシカが変更された場合はこいつを生成したオブジェクトに移す
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharaMove : MonoBehaviour
{

    public float moveSpeed; // 移動速度
    public float tiltAngle; // 最大傾き角度
    public float tiltSpeed; // 傾きの補間速度
    public float angulerGravity = 5.0f;

    private Rigidbody2D m_rb2;
    private float horizontalInput;

    private void Start()
    {
        m_rb2 = GetComponent<Rigidbody2D>();
        m_rb2.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
    }

    void Update()
    {

        // プレイヤーの入力を取得
        horizontalInput = Input.GetAxis("Horizontal");

        // 水平方向の移動を処理
        Vector3 moveDirection = new Vector3(horizontalInput * moveSpeed, m_rb2.velocity.y, 0);
        m_rb2.velocity = moveDirection;

        if (Input.GetButtonUp("Horizontal"))
        {
            m_rb2.velocity = new Vector2(0.0f, m_rb2.velocity.y);
        }

        // 傾く処理
        // 変数設定
        float angulerVelocity = 0.0f;
        float rotationZ = transform.rotation.z;
        float rotationEulerZ = transform.rotation.eulerAngles.z;
        if (rotationEulerZ > 180.0f) rotationEulerZ -= 360.0f;  // -180 ~ 180にする

        // 傾きの計算
        angulerVelocity = m_rb2.angularVelocity;
        angulerVelocity += tiltSpeed * horizontalInput * Time.deltaTime; // 移動方向に応じた傾き速度
        if (rotationEulerZ != 0.0f) angulerVelocity += Mathf.Clamp(-rotationZ * 3.0f, -1.0f, 1.0f) * angulerGravity * Time.deltaTime;  // 距離 * 係数

        // 傾きの設定
        m_rb2.angularVelocity = angulerVelocity;

        // 上限設定
        if (Mathf.Abs(rotationEulerZ) > tiltAngle)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, tiltAngle * Mathf.Sign(rotationZ));
        }
    }
}
