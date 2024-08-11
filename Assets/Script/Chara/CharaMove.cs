using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief �v���C���[�ƓG�̈ړ������̊��N���X
 */
public class CharaMove : MonoBehaviour
{
    /**
     * @brief �v���C���[�ƓG���ʂ̏�Ԃ�\��
     */
    public enum CharaCondition
    {
        Ground,
        Flying,
        Swimming,
        Dead,
    }

    CharaCondition charaCondition = CharaCondition.Ground;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
     *  @brief �L�����̏�Ԃ��Z�b�g����
     *  @param  CharaCondition _charaCondition ���
     */
    public void SetCharaCondition(CharaCondition _charaCondition)
    {
        this.charaCondition = _charaCondition;
    }

    /**
     *  @brief �L�����̏�Ԃ��擾����
     *  @return  CharaCondition this.charaCondition ���
     */
    public CharaCondition GetCharaCondition()
    {
        return this.charaCondition;
    }
}
