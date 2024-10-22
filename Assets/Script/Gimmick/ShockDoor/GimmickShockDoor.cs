using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �Ԃ����ē|���h�A�̃X�N���v�g
 *          �Ԃ��������̑��x���Q�Ƃ��ē|��邩���߂�
 *          �|��鎞�̋����̓A�j���[�V�����ō쐬
 *          
 *          �A�j���[�V�����p�����[�^��
 *          bool�^Shocked��true�ɂȂ�܂�
 * 
 * @memo    
 *          �E���ł邩���肷�鏈��
 *          �E�|��鎞�̏����i����̓A�j���[�V�����ŏ����\��j
 */
public class GimmickShockDoor : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponentInParent<Animator>();
        this.animator.SetBool("Shocked", false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �Փ˔���
    private void OnCollisionEnter2D(Collision2D _collision)
    {
        // �v���C���[�̂ݔ���
        if (_collision.gameObject.CompareTag("Player"))
        {
            var state = _collision.gameObject.GetComponent<PlayerMove>();
            if (state.playerCondition == PlayerState.PlayerCondition.Flying)
            {
                // ���ł�ꍇ�͓|��
                Shocked();
            }
        }
    }

    // �A�j���[�V�����Đ�
    public void Shocked()
    {
        Debug.Log("Shocked");
        this.animator.SetBool("Shocked", true);
    }
}
