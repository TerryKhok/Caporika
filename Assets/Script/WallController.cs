using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public int wallSize;    // �ǂ̃T�C�Y

    private bool isTrigger = false;
    private int playerSize = 0;                 // �ʂ�}�g�����[�V�J�̑傫��
    private BoxCollider2D wallCollider = null;  // �ǂ̓����蔻��

    // Start is called before the first frame update
    void Start()
    {
        wallCollider=GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[���ʂ낤�Ƃ����Ƃ�
        if(isTrigger)
        {
            // �v���C���[�̃T�C�Y���ǂ̃T�C�Y�����������Ƃ�
            if(playerSize�@<= wallSize)
            {
                Debug.Log("playerSize" + playerSize);
                Debug.Log("wallSize" + wallSize);

                // �ǂ̓����蔻��������āu�ʂ��v
                wallCollider.enabled=false;
            }
            else
            {
                // �ǂ̓����蔻���t���āu�ʂ�Ȃ��v
                wallCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �v���C���[����������
        if(collision.CompareTag("player"))
        {
            isTrigger = true;

            // �}�g�����[�V�J�̃T�C�Y���擾
            playerSize= collision.gameObject.GetComponent<CharaState>().GetMatryoshkaSize();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �v���C���[���o���Ƃ�
        if (collision.CompareTag("player"))
        {
            // ���Z�b�g
            isTrigger = false;
            playerSize = 0;
        }
    }
}
