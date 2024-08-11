using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 	CharaState.cs�ύX�ő��̃X�N���v�g�̏C�����I���܂ł̊Ԉꎞ�I�Ɏg���N���X
 * 
 *  @memo   �E�e���X�N���v�g���C��(CharaMove.charaCondition����ɏ����𓮂����悤��)
 *          �E�S�ẴX�N���v�g���ł��̃N���X���g�p���Ȃ��Ȃ����Ƃ����̃X�N���v�g���폜����
*/
public class TenpState : MonoBehaviour
{
    public enum State
    {
        Normal,     // �ʏ�
        Flying,     // ���ł���
        Damaged,    // �_���[�W���󂯂Ă���
        Dead,       // ����ł���
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