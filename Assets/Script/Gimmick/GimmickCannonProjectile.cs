using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ��C����΂����ɕt����X�N���v�g
 * 
 * @memo    �E��莞�Ԍ�ɔj�󂳂��
 *          �E�ǂɓ���������j�󂳂��
 */
public class GimmickCannonProjectile : MonoBehaviour
{
    public float lifeTime = 5.0f;  ///< �ǂɓ�����Ȃ��Ă�������܂ł̎��ԁi�f�t�H���g 5�b�j
    private GameObject playerFollowedObject; ///< ���Ă��Ă�v���C���[

    private void Start()
    {
        // ��莞�Ԍ�ɋ���j��
        Invoke("DestroyThis", lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ǂɓ��������玩��
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("realGround"))
        {
            DestroyThis();
        }
    }

    /**
     * @brief   �t���Ă�������I�u�W�F�N�g��ݒ肷��
     * 
     * @param   _playerFollowedObject ���Ă���Q�[���I�u�W�F�N�g
     */
    public void SetPlayerFollowedObject(GameObject _playerFollowedObject)
    {
        this.playerFollowedObject = _playerFollowedObject;
    }

    /**
     * @brief   ���󎞂̏���
     * 
     * @memo    GimmickCannonFire�̕��ŕύX�������ڂ����ɖ߂��Ă��玩��
     */
    void DestroyThis()
    {
        // �v���C���[�̒Ǐ]������
        if (playerFollowedObject != null)
        {
            playerFollowedObject.transform.parent = null;  // �v���C���[�����̏�Ԃɖ߂�
        }
        playerFollowedObject.GetComponent<Rigidbody2D>().simulated = true;
        playerFollowedObject.GetComponent<Collider2D>().enabled = true;
        // �ǂɓ��������狅��j��
        Destroy(this.gameObject);
    }
}

