using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageSelectButton : MonoBehaviour
{
    [SerializeField] private string sceneName;  // �{�^���N���b�N���ɑJ�ڂ���V�[����

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    /**
     * @brief   �{�^���N���b�N���Ɏ��s����֐�
     *          �V�[�����[�h���ɕK�v�ȏ������L�q����
     */
    public void OnClickButton()
    {
        GimmickCheckpointParam.ResetCheckpointParams();    // �`�F�b�N�|�C���g�̃��Z�b�g
        // �V�[�������[�h���鏈��
    }
}
