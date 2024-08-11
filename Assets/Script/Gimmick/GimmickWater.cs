using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


/**
 *  @brief ���̃M�~�b�N�̏������L�q
 *  ���I�u�W�F�N�g�ɃA�^�b�`
 *  
 *  @memo   �E�������݂̏���
 */
public class GimmickWater : MonoBehaviour
{
    private List<TenpState> objectsInWater = new List<TenpState>();    // ���ݐ����ɂ���I�u�W�F�N�g���Ǘ����邽�߂�list

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TenpState tempState;
        // TenpState�������Ă��邩(�}�g�����V�J��)����
        if (!collision.gameObject.TryGetComponent(out tempState)) return;

        AddToList(tempState);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TenpState tempState;
        // TenpState���Ȃ�������return
        if (!collision.gameObject.TryGetComponent(out tempState)) return;

        // TenpState�������Ă��邩(�}�g�����V�J��)����
        if (objectsInWater.Contains(tempState))
        {
            RemoveFromList(tempState);
        }
    }

    void AddToList(TenpState _state)
    {
        // y�����̑��x������������
        Rigidbody2D rb = _state.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 10.0f);
        if (_state.GetCharaState() != TenpState.State.Dead)    // ����łȂ��Ȃ�
        {
            _state.SetCharaState(TenpState.State.Normal);
        }

        int size = _state.GetCharaSize();
        switch (size)
        {
            case 1: // ������
                rb.gravityScale = -0.2f;
                break;
            case 2:
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                rb.gravityScale = 0.0f;
                break;
            case 3:
                rb.gravityScale = 0.2f;
                break;
        }

        objectsInWater.Add(_state);
    }

    /**
     * @brief ���X�g����폜����Ƃ��̏���
     */
    void RemoveFromList(TenpState _state)
    {
        _state.GetComponent<Rigidbody2D>().gravityScale = 1.0f;  // �f�t�H���g�l�ɂ���
        objectsInWater.Remove(_state);  // ���X�g����폜
    }
}
