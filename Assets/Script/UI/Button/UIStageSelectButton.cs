using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStageSelectButton : MonoBehaviour
{
    [SerializeField] private string sceneName;  // �{�^���N���b�N���ɑJ�ڂ���V�[����
    private GameObject irisObject;
    private UIIrisScript iris;
    
    /**
    * @brief ������
    * @memo 
    */
    void Start()
    {
        irisObject = GameObject.Find("IrisCanv");
        iris = irisObject.GetComponent<UIIrisScript>();
    }


    /**
    * @brief �X�e�[�W�Z���N�g�{�^���p
    * @memo �����ɃV�[��������
    */
    public void StageSerectButton(string _str)
    {
        iris.IrisOut(_str); //���̃V�[������
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
