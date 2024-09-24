using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  @brief 	�v���C���[�̓������܂Ƃ߂��N���X
*/
public class PlayerMove : MonoBehaviour
{

    public float centerOfMassOffset = 0.6f; // �d�S�̈ʒu�̊���

    private Rigidbody2D rb;
    private Collider2D trigger;

    public PlayerState.PlayerCondition playerCondition;     // �v���C���[���L�̏��
    private PlayerState currentState = null;                // �v���C���[�̌��݂̏�Ԃ̓���

    private bool isInWater = false;     // true:���̒��ɂ���
    private bool isGround = false;      // true:�n�ʂƓ������Ă���

    void Start()
    {
        // �}�g�����[�V�J�̏d�S�����ɐݒ�
        this.rb = GetComponent<Rigidbody2D>();
        if (this.rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 size = renderer.bounds.size;
            // �d�S���I�u�W�F�N�g�̍�����centerOfMassOffset�̈ʒu�ɐݒ�
            this.rb.centerOfMass = new Vector2(0.0f, -size.y * (0.5f - this.centerOfMassOffset));
        }

        // �v���C���[�̏�Ԃɍ��킹�Č��݂̓�����ݒ�
        switch (this.playerCondition)
        {
            case PlayerState.PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerState.PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerState.PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerState.PlayerCondition.Dead:
                this.currentState = new PlayerStateDead();
                break;
            case PlayerState.PlayerCondition.Damaged:
                this.currentState = new PlayerStateDamaged();
                break;
            default:
                this.currentState = null;
                break;
        }

        if (this.currentState != null)
        {
            // �ύX��̏�Ԃ̊J�n�������s��
            this.currentState.Enter(this);
        }
    }

    void OnDrawGizmos()
    {
        if (rb != null)
        {
            // �d�S�̈ʒu��Ԃ����ŕ\��
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(rb.worldCenterOfMass, 0.1f);
        }
    }


    private void FixedUpdate()
    {
        this.currentState.Update();
        this.currentState.CollisionEnter(trigger);

        // ����ł���Ƃ������s��Ȃ�
        if (this.playerCondition == PlayerState.PlayerCondition.Dead)
        {
            return;
        }

        // ���ɓ����Ă���Ƃ��u���̒��ɂ����ԁv
        if (this.isInWater)
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Swimming);
            return;
        }

        // �n�ʂƓ������Ă���Ƃ��u�n�ʂɗ����Ă����ԁv
        if (this.isGround)
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Ground);
            return;
        }
        else
        {
            this.ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            this.isInWater = true;
        }

        // ���ɓ����Ă���Ƃ��͔�����s��Ȃ�
        if (!this.isInWater)
        {
            // �n�ʂ��{�^��
            if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Button"))
            {
                this.isGround = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            this.isInWater = false;
        }
        // �n��
        if (collision.gameObject.CompareTag("ground"))
        {
            this.isGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // ���ɓ����Ă���Ƃ��͔�����s��Ȃ�
        if (!this.isInWater)
        {
            // �{�^��
            if (collision.gameObject.CompareTag("Button"))
            { 
                this.isGround = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // �{�^��
        if (collision.gameObject.CompareTag("Button"))
        {
            this.isGround = false;
        }
    }

    /**
     *  @brief 	�v���C���[�̏�ԑJ�ڂ��s������
     *  @param  PlayerState.PlayerCondition _changeCondition    �ύX��̃v���C���[�̏��
    */
    public void    ChangePlayerCondition(PlayerState.PlayerCondition _changeCondition)
    {
        // ��Ԃ������Ȃ珈�����s��Ȃ�
        if(this.playerCondition == _changeCondition) { return; }
        this.playerCondition = _changeCondition;

        if (this.currentState != null)
        {
            // ���݂̏�Ԃ̏I���������s��
            this.currentState.Exit();
        }

        // �ύX�����Ԃ̓���N���X�ɂ���
        switch (this.playerCondition)
        {
            case PlayerState.PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerState.PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerState.PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerState.PlayerCondition.Dead:
                this.currentState = new PlayerStateDead();
                break;
            case PlayerState.PlayerCondition.Damaged:
                this.currentState = new PlayerStateDead();  // ��U���񂾂��Ƃɂ���
                break;
            default:
                this.currentState = null;
                break;
        }
        //Debug.Log(this.playerCondition + "�ɕύX");

        if (this.currentState != null)
        {
            // �ύX��̏�Ԃ̊J�n�������s��
            this.currentState.Enter(this);
        }
    }
}