using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaDeath : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D _other)
    {
        Debug.Log("unti");
        // �}�g�����[�V�J�̌��ɗ����Ƃ�
        if (_other.CompareTag("Player"))
        {
            // �}�g�����[�V�J������ł��Ȃ����
            PlayerMove playerMove = _other.GetComponent<PlayerMove>();
            if(playerMove.playerCondition!=PlayerState.PlayerCondition.Dead)
            {

                // �v���C���[���Q�[���I�[�o�[�ɂ���
                MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                if (!playerManager)
                {
                    Debug.LogError("MatryoshkaManager���擾�ł��܂���ł����B");
                    return;
                }
                int currentLife = playerManager.GetCurrentLife();
                playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                playerManager.GameOver();
            }
        }
    }
}
