using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �J�������w��ʒu�܂ňړ������鏈��
 * 
 *          �E�ݒ���@
 *              �EcameraTargets�ɃJ�������ړ����������ʒu�ɒu�����I�u�W�F�N�g�����Ԃɓ����
 * 
 *              �EGimmickEventTrigger�������I�u�W�F�N�g��z�u����
 *              �J�������Q��->MoveToNwxtPosition()���Ăяo���悤�ɐݒ肷��
 * 
 * @memo    �E�ݒ肵�����̈ʒu�܂ňړ������鏈��
 *          �E��̏����̃R���[�`��
 *       
 */
public class CameraController : MonoBehaviour
{
    public Transform[] cameraTargets;    ///< �J�����̈ړ��ʒu�w��Ɏg�p����GameObject�̃��X�g
    private int currentTargetIndex = 0;  ///< ���݂̃^�[�Q�b�g�ʒu�C���f�b�N�X

    /**
     * @brief ���̈ʒu�Ɉړ�����
     * 
     * @memo EventTrigger�Ńg���K�[������
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
     * @brief ��莞�Ԃ����Ďw��̈ʒu�܂�Lerp����
     */
    private IEnumerator MoveCamera(Vector3 _targetPosition)
    {
        float duration = 1.0f;  // �ړ��ɂ����鎞��
        float elapsedTime = 0;

        Vector3 startingPosition = this.transform.position;

        // �ړ�
        while (elapsedTime < duration)
        {
            this.transform.position = Vector3.Lerp(startingPosition, _targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.transform.position = _targetPosition;
    }
}

