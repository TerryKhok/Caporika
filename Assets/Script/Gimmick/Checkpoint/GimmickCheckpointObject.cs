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
            Activate();
        }
    }

    /**
     * @brief   �`�F�b�N�|�C���g��L��������
     */
    void Activate()
    {
        GimmickCheckpointParam.SetCheckpointNum(checkpointIndex, cameraSceneIndex);
    }
}
