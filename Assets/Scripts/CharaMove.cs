/*
 * �v���C���[�̈ړ��������ǂ�X�N���v�g�B
 * �}�g�����V�J���ύX���ꂽ�ꍇ�͂����𐶐������I�u�W�F�N�g�Ɉڂ�
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharaMove : MonoBehaviour
{

    public float moveSpeed; // �ړ����x
    public float tiltAngle; // �ő�X���p�x
    public float tiltSpeed; // �X���̕�ԑ��x
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

        // �v���C���[�̓��͂��擾
        horizontalInput = Input.GetAxis("Horizontal");

        // ���������̈ړ�������
        Vector3 moveDirection = new Vector3(horizontalInput * moveSpeed, m_rb2.velocity.y, 0);
        m_rb2.velocity = moveDirection;

        if (Input.GetButtonUp("Horizontal"))
        {
            m_rb2.velocity = new Vector2(0.0f, m_rb2.velocity.y);
        }

        // �X������
        // �ϐ��ݒ�
        float angulerVelocity = 0.0f;
        float rotationZ = transform.rotation.z;
        float rotationEulerZ = transform.rotation.eulerAngles.z;
        if (rotationEulerZ > 180.0f) rotationEulerZ -= 360.0f;  // -180 ~ 180�ɂ���

        // �X���̌v�Z
        angulerVelocity = m_rb2.angularVelocity;
        angulerVelocity += tiltSpeed * horizontalInput * Time.deltaTime; // �ړ������ɉ������X�����x
        if (rotationEulerZ != 0.0f) angulerVelocity += Mathf.Clamp(-rotationZ * 3.0f, -1.0f, 1.0f) * angulerGravity * Time.deltaTime;  // ���� * �W��

        // �X���̐ݒ�
        m_rb2.angularVelocity = angulerVelocity;

        // ����ݒ�
        if (Mathf.Abs(rotationEulerZ) > tiltAngle)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, tiltAngle * Mathf.Sign(rotationZ));
        }
    }
}
