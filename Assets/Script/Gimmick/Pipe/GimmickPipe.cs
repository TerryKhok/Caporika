using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief   �p�C�v�M�~�b�N�̈ړ�����
 * @memo    EntryPoint��EventTrigger�Ŕ���
 *          EntryPoint����ExitPoint�܂�Lerp����
 */
public class GimmickPipe : MonoBehaviour
{
    public Transform entryPoint;   // �p�C�v�̓���
    public Transform exitPoint;    // �p�C�v�̏o��
    public float moveDuration = 2f;   // �ړ��ɂ����鎞�ԁi�b�j

    public GimmickEventTrigger eventTrigger;    // �g���K�[
    private bool isTransporting = false;   // �v���C���[���ړ������ǂ���
    private Transform playerTransform;     // �v���C���[��Transform

    /**
     * @brief   �C�x���g�g���K�[��OnTriggerEnter2D�ŌĂяo�����
     *          Lerp���J�n����
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

    // �v���C���[���o���܂ŃX���[�Y�Ɉړ�������R���[�`��
    private IEnumerator TransportPlayer()
    {
        isTransporting = true;
        // �ړ��n�����Z�b�g���đ����؂�
        PlayerMove playerMove = playerTransform.GetComponent<PlayerMove>();
        playerMove.enabled = false;
        playerTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;


        float elapsedTime = 0f;  // �o�ߎ��Ԃ�ǐ�

        while (elapsedTime < moveDuration)
        {
            // �o�ߎ��ԂɊ�Â���Lerp���v�Z
            playerTransform.position = Vector3.Lerp(entryPoint.position, exitPoint.position, elapsedTime / moveDuration);
            playerTransform.rotation = this.transform.parent.rotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �Ō�ɏo���ʒu�ɐ��m�ɔz�u
        playerTransform.position = exitPoint.position;

        // �v���C���[���o���ɓ��B
        // ���앜���Ƒ��x���Z�b�g
        playerMove.enabled = true;
        playerTransform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        isTransporting = false;
    }
}
