using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �������ړ�������X�N���v�g
 * 
 * @memo    
 */
public class ParabolicMove : MonoBehaviour
{
    private Transform targetPosition; ///< �ړ���̍��W
    public float moveSpeed = 10.0f; ///< �ړ����x
    public float height = 2.0f; ///< �ő�̍���
    private bool isMoving = false; ///< �ړ������ǂ���
    private Vector3 startPosition; ///< �J�n�ʒu
    private float journeyLength; ///< �ړ�����
    private float startTime; ///< �J�n����
    private bool selfRemove; ///< �I�����Ɏ����I�ɃX�N���v�g���폜����

    /**
     * @brief   �ړ����Ɏg�����̏�����
     * 
     * @param   _target �ړ��ڕW��Transform
     * @param   _speed  �ړ�����Ƃ��̑��x
     * @param   _height �ړ����̍����̍ő�l
     * @param   _selfRemove ���s��ɂ��̃X�N���v�g�������폜����
     */
    public void Init(Transform _target, float _speed, float _height, bool _selfRemove)
    {
        this.targetPosition = _target;
        this.moveSpeed = _speed;
        this.height = _height;
        this.selfRemove = _selfRemove;
    }

    /**
     * @brief   �ړ����J�n����
     */
    public void Active()
    {
        this.startPosition = transform.position; // �J�n�ʒu���L�^
        this.journeyLength = Vector3.Distance(this.startPosition, this.targetPosition.position); // �ړ��������v�Z
        this.startTime = Time.time; // �J�n�������L�^
        this.isMoving = true; // �ړ����J�n
    }

    /**
     * �ړ��̋�̓I�ȏ���
     */
    public void Update()
    {
        // �ړ����ł���ΎR�Ȃ�Ɉړ�
        if (this.isMoving)
        {
            float distCovered = (Time.time - this.startTime) * this.moveSpeed; // �o�߂�������
            float fractionOfJourney = distCovered / this.journeyLength; // �S�̂ɑ΂��銄��

            // ���������v�Z
            float yOffset = this.height * Mathf.Sin(fractionOfJourney * Mathf.PI); // �ő�̍����܂ł̌v�Z
            transform.position = Vector3.Lerp(this.startPosition, this.targetPosition.position, fractionOfJourney);
            transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

            // �ڕW�ʒu�ɋ߂Â�����ړ����~
            if (fractionOfJourney >= 1.0f)
            {
                this.isMoving = false; // �ړ����~
                transform.position = this.targetPosition.position; // �ŏI�ʒu�ɐݒ�
                if (this.TryGetComponent(out Rigidbody2D rb))
                {
                    // �I�����ɕςȊ������c��Ȃ��悤�ɂ���
                    rb.velocity = Vector3.zero; rb.angularVelocity = 0;
                }
                if (this.selfRemove) Destroy(this); // �ړ��I������玩���폜
            }
        }
    }
}