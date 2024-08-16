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
    private bool isGround = false;

    void Start()
    {
        // �}�g�����[�V�J�̏d�S�����ɐݒ�
        this.rb = GetComponent<Rigidbody2D>();
        if (this.rb != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            Vector2 size = renderer.bounds.size;
            this.rb.centerOfMass = new Vector2(0.0f, -size.y * (1 - this.centerOfMassOffset));
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


    private void FixedUpdate()
    {
        this.currentState.Update();
        this.currentState.CollisionEnter(trigger);

        // ����ł��Ȃ���
        if (this.playerCondition != PlayerState.PlayerCondition.Dead)
        {
            // ���ɓ����Ă����Ԃ��D�悳���
            if(this.isInWater)
            { 
                ChangePlayerCondition(PlayerState.PlayerCondition.Swimming); 
            }
            else
            {
                if(this.isGround)
                {
                    ChangePlayerCondition(PlayerState.PlayerCondition.Ground);
                }
                else
                {
                    ChangePlayerCondition(PlayerState.PlayerCondition.Flying);
                }
            }
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("water"))
        {
            this.isInWater = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // ���ɓ����Ă���Ƃ��͔�����s��Ȃ�
        if (!this.isInWater)
        {
            if (collision.gameObject.CompareTag("ground"))
            {
                this.isGround = true;
            }
            else
            {
                this.isGround = false;
            }
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
        Debug.Log(this.playerCondition + "�ɕύX");

        if (this.currentState != null)
        {
            // �ύX��̏�Ԃ̊J�n�������s��
            this.currentState.Enter(this);
        }
    }
}
