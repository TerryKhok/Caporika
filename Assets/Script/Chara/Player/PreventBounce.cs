using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief  �}�g�����[�V�J�����˂�̂�h���֐�
 *  @memo   �n�ʂƋ��������ȏ㗣�ꂽ��O�̍��W�ɂ���
*/
public class PreventBounce : MonoBehaviour
{
    private Vector3 prevPos = Vector3.zero;                         // �O�t���[���̍��W
    public float desiredForce = 1.0f;                               // ���E����͂̓x����
    public float groundDistance = 0.35f;                            // �n�ʂƂ̋���
    private Rigidbody2D rb = null;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        if (!this.rb)
        {
            Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
            return;
        }
    }

    private void OnEnable()
    {
        Debug.Log("On�ɂȂ���");
        if (!this.rb) this.rb = this.GetComponent<Rigidbody2D>();
        if (!this.rb)
        {
            Debug.LogError("Rigidbody2D���擾�ł��܂���ł����B");
            return;
        }
        // ���W��ێ�
        this.prevPos = this.rb.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = 0.0f;
        Vector2 origine = this.rb.worldCenterOfMass - new Vector2(0.0f, 0.0f);

        // ���g�𖳎����郌�C���[�}�X�N��ݒ�
        int playerLayer = LayerMask.NameToLayer("player");
        int triggerArea= LayerMask.NameToLayer("TriggerArea");
        int groundLayer = LayerMask.NameToLayer("ground");

        int layerMask = (1 << playerLayer) | (1 << groundLayer) | (1 << triggerArea);
        layerMask = ~layerMask;

        // �}�g�����[�V�J�̏d�S���牺������Ray�𔭎�
        RaycastHit2D hit = Physics2D.Raycast(origine, Vector2.down, 10.0f, layerMask);
        Debug.Log(hit.collider.tag);
        // ���������I�u�W�F�N�g���n�ʂ̎�
        if (hit.collider != null && hit.collider.CompareTag("realGround"))
        { 
            // �����𑪂�
            distance = hit.distance;
           // Debug.Log("hit.distance" + hit.distance);
        }
        else { Debug.Log("�������Ă��Ȃ�" + hit.distance); }
        // Ray������
        Debug.DrawRay(origine, Vector2.down * 10, Color.blue);

        // y���̃x�N�g���𑊎E����
        this.rb.AddForce(-Vector2.up * this.desiredForce, ForceMode2D.Force);

        // �n�ʂƂ̋��������ȏ㗣�ꂽ�Ƃ�
        if (distance > this.groundDistance)
        {
            // �O�t���[���̍��W�ɂ���
            this.rb.transform.position = new Vector3(this.rb.transform.position.x, this.prevPos.y - 0.1f, this.rb.transform.position.z);
            Debug.Log("�߂���");
        }
    }

     void OnCollisionStay2D(Collision2D collision)
    {
        // �n�ʂɓ������Ă���Ƃ�
        if (collision.transform.CompareTag("realGround"))
        {
            if(this.rb)
            {
                // ���W��ێ�
                this.prevPos = this.rb.transform.position;
                Debug.Log("�ێ�����");
            }
        }
    }
}