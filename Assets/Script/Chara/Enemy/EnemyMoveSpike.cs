using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ���̂��߂�Script
 * 
 * @memo    �E�ړ����Ȃ�
 *          �E���ȂȂ�
 *          �E�Ԃ������v���C���[�̊k���^�[�Q�b�g�܂Ŕ�΂�
 */
public class EnemyMoveSpike : CharaMove
{
    public Transform damageMoveTarget = null;   // �_���[�W���̖ڕW�ʒu
    public float damagedMoveSpeed = 3.0f;       // �_���[�W���̈ړ��̑��x
    public float damagedMoveHeight = 2.0f;      // �_���[�W���̈ړ��̍����̍ő�l�i�V��ɂԂ���Ȃ����߁j


    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        // �󒆂ɂ͂��Ȃ������ȂȂ����~�܂��Ă邾��
        if (this.GetCharaCondition() == CharaCondition.Dead)
        {
            this.SetCharaCondition(CharaCondition.Ground);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �߂肽���ꏊ�����܂��Ă�Ȃ�v���C���[���΂�
        if (damageMoveTarget != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // �������ړ��p�X�N���v�g�����Ĕ�΂�
                var move = collision.gameObject.AddComponent<ParabolicMove>();
                move.Init(damageMoveTarget.transform, damagedMoveSpeed, damagedMoveHeight, true);
                move.Active();
            }
        }
    }
}
