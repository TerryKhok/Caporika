using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatryoshkaState : MonoBehaviour
{
    public enum State
    {
        Normal,    // �ʏ�
        Flying,    // ���ł���
        Dead,      // ����ł���
    }

    public State state;     // ���̃}�g�����[�V�J�̏��
    public int sizeState;   // ���̃}�g�����[�V�J�̑傫��

    // ��Ԃ̃Z�b�g
    public void SetMatryoshkaState(State _state)
    {
        this.state = _state;
    }

    // ��Ԃ̎擾
    public State GetMatryoshkaState()
    {
        return this.state;
    }

    // �傫���̎擾
    public int GetMatryoshkaSize()
    {
        return this.sizeState;
    }
}