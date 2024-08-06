using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	�L�����̏�Ԃ�ێ�����
*/
public class CharaState : MonoBehaviour
{
    public enum State
    {
        Normal,    // �ʏ�
        Flying,    // ���ł���
        Dead,      // ����ł���
    }

    public State state;     // ���̃L�����̏��
    public int sizeState;   // ���̃L�����̑傫��


    /**
     *  @brief 	�L�����̏�Ԃ̃Z�b�g
     *  @param  State _state   ���
    */
    public void SetCharaState(State _state)
    {
        this.state = _state;
    }

    /**
     *  @brief 	�L�����̏�Ԃ̎擾
     *  @return State this.this.state  ���
    */
    public State GetCharaState()
    {
        return this.state;
    }

    /**
     *  @brief 	�L�����̃T�C�Y�̎擾
     *  @return int this.sizeState  �T�C�Y
    */
    public int GetCharaSize()
    {
        return this.sizeState;
    }
}