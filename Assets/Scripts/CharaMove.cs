/*
 * プレイヤーの移動をつかさどるスクリプト。
 * マトリョシカが変更された場合はこいつを生成したオブジェクトに移す
 */

using System.Collections;
using System.Collections.Generic;
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
        float angulerVelocity = 0.0f;
        float rotationZ = transform.rotation.z;
        float rotationEulerZ = transform.rotation.eulerAngles.z;
        if (rotationEulerZ > 180.0f) rotationEulerZ -= 360.0f;  // -180 ~ 180にする

        // プレイヤーの入力を取得
        horizontalInput = Input.GetAxis("Horizontal");

        // 水平方向の移動を処理
        Vector3 moveDirection = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);
        m_rb2.AddForce(moveDirection, ForceMode2D.Impulse);

        angulerVelocity = m_rb2.angularVelocity;
        angulerVelocity += tiltSpeed * horizontalInput; // 移動方向に応じた傾き速度
        if (rotationEulerZ != 0.0f) angulerVelocity += -rotationZ * angulerGravity;  // 距離 * 係数

        m_rb2.angularVelocity = angulerVelocity;

        if (Mathf.Abs(rotationEulerZ) > tiltAngle)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, tiltAngle * Mathf.Sign(rotationZ));
        }
    }
}
