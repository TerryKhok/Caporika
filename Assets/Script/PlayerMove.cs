using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float tiltAmount = 45.0f;        // ��]�p�̍ő�l
    public float returnSpeed = 2.0f;        // ��]�����ɖ߂����x
    public float damping = 0.1f;            // �����W��
    public float inputDeadZone = 0.1f;      // ���͂̃f�b�h�]�[��
    public float angleSwingZone = 0.3f;     // �h�ꂽ��������ǂ���
    public int maxSwimg = 3;                // ����h��邩 

    private Rigidbody2D rb;
    private float tiltVelocity = 0f;      // �X���̑��x
    private int swingCount = 0;           // �h��̉񐔂��J�E���g
    private bool isInDeadZone = false;    // �f�b�h�]�[�����ɂ��邩�ǂ���

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // �d�S�����ɐݒ�
        rb.centerOfMass = new Vector2(0, -0.6f);
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // ���͒l�̃f�b�h�]�[����K�p
        if (Mathf.Abs(moveInput) < inputDeadZone){ moveInput = 0.0f; }

        // �ړ���-----------------------------------------------------------
        if (moveInput != 0.0f)
        {
            // �ړ������Ƌt�ɌX����
            float tilt = moveInput * tiltAmount;
            tilt = Mathf.Clamp(tilt, -tiltAmount, tiltAmount);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, tilt);

          
            tiltVelocity = 0.0f;    // �X���̑��x�����Z�b�g
            swingCount = 0;         // �h��̉񐔂����Z�b�g
            isInDeadZone = false;   // �f�b�h�]�[���t���O�����Z�b�g
            rb.isKinematic = false; // �������Z�𖳌���

        }
        // �~�܂�����-----------------------------------------------------------
        else
        {
            // �߂������p�x�ƌ��݂̊p�x
            float targetRotation = 0.0f;
            float currentRotation = transform.rotation.eulerAngles.z;

            // �p�x��-180�x����180�x�͈̔͂ɕϊ����ĖڕW�p�x�Ƃ̍����o��
            if (currentRotation > 180.0f) currentRotation -= 360.0f;
            float deltaRotation = targetRotation - currentRotation;

            // �����ŗh��Ă���^�������ɖ߂�
            tiltVelocity += deltaRotation * returnSpeed * Time.deltaTime;

            // �p�x���v�Z
            float newRotation = currentRotation + tiltVelocity;
            newRotation = Mathf.Clamp(newRotation, -tiltAmount, tiltAmount);

            // �p�x���f�b�h�]�[�����ɂ��邩�ǂ������`�F�b�N
            if (Mathf.Abs(deltaRotation) < angleSwingZone)
            {
                if (!isInDeadZone)
                {
                    // �f�b�h�]�[����ʉ߂�����1��u�h�ꂽ�v
                    swingCount++;
                    isInDeadZone = true;
                }
            }
            else{ isInDeadZone = false; }

            // 3��ڂ̗h�ꂪ�I�������
            if (swingCount >= maxSwimg)
            {
                // ��]�A���x�����Z�b�g
                newRotation = 0.0f;
                tiltVelocity = 0.0f;
                rb.velocity = new Vector2(0.0f, 0.0f);
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

                // �������Z�𖳌���
                rb.isKinematic = true; 
                swingCount = maxSwimg;
            }

            // �p�x�̃Z�b�g
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, newRotation);

            // �i�X�ӂ蕝������������
            tiltVelocity *= (1 - damping);
        }

        // ���x���Z�b�g
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
}