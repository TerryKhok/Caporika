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
    public void StageSelectButton(string _str)
    {
        SoundManager.Instance.PlaySE("MENU_SELECT");
        GimmickCheckpointParam.ResetCheckpointParams();    // �`�F�b�N�|�C���g�̃��Z�b�g
        iris.IrisOut(_str); //���̃V�[������
    }
}
