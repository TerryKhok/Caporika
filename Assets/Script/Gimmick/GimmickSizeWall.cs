using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

/**
 * @brief   �T�C�Y�ɉ����Ēʂ���
 * 
 * @memo    �E�ǂɐG�ꂻ���ɂȂ�ƃI�u�W�F�N�g���ƂɐڐG���邩�𔻒肷�鏈��
 * 
 */
public class GimmickSizeWall : MonoBehaviour
{
    public int wallSize;    ///< �ʂ��T�C�Y�i2�Ȃ�CharaSize2��1���ʂ��

    private Collider2D wallCollider;    ///< �ǂ̃R���C�_�[

    // Start is called before the first frame update
    void Start()
    {
        this.wallCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �ڐG�������ȃI�u�W�F�N�g��CharaState���擾
        CharaState charaState = collision.gameObject.GetComponent<CharaState>();
        if (charaState != null) // null�`�F�b�N
        {
            int charaSize = charaState.GetCharaSize();  // �T�C�Y�擾

            if (charaSize <= this.wallSize)
            {
                // �ʂ��T�C�Y�Ȃ瓖���蔻��𖳌���
                Physics2D.IgnoreCollision(collision, this.wallCollider);
            }
        }
    }
}
