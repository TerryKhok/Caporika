using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �`�F�b�N�|�C���g�̃I�u�W�F�N�g�ɑ΂��ĕt����
 * @memo    �ETrigger�ɐG�ꂽ��`�F�b�N�|�C���g�̃p�����[�^���X�V����
 */
public class GimmickCheckpointObject : MonoBehaviour
{
    // MatryoshkaManager�̃��X�g�̃C���f�b�N�X�ƑΉ�������
    [SerializeField] private int checkpointIndex = 0;
    // CameraController�̈ڂ��V�[���̃C���f�b�N�X�ƑΉ�������
    [SerializeField] private int cameraSceneIndex = 0;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            // ����łȂ�������A�N�e�B�x�[�g
            var player = _collision.gameObject;
            if (player.GetComponent<PlayerMove>().playerCondition != PlayerState.PlayerCondition.Dead)
            {
                Activate();
            }
        }
    }

    /**
     * @brief   �`�F�b�N�|�C���g��L��������
     *          ���̌�c�@��
     */
    void Activate()
    {
        // ������A�N�e�B�x�[�g���Ȃ�
        if (activated) return;

        SoundManager.Instance.PlaySE("STAGE_CHECKPOINT");
        GimmickCheckpointParam.SetCheckpointNum(checkpointIndex, cameraSceneIndex);
        // �����Ƀ}�g�����V�J�̎c�@�񕜏���
        // �}�g�����V�J��S�ď����Ďc�@3�̂�𐶐�
        // null�`�F�b�N�S�R���ĂȂ��ăN�\
        var manager = GameObject.Find("MatryoshkaManager");
        manager.GetComponent<MatryoshkaManager>().Respawn();

        activated = true;
    }
}
