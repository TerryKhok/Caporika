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

    /**
     *  @brief 	�v���C���[���L�̏�Ԃ̗񋓌^
    */

    public enum PlayerCondition
    {
        Ground,     // �n�ʂɂ���
        Flying,     // ���ł���
        Swimming,   // ���̒��ɂ���
        Dead,       // ����ł���

        Damaged,    // �_���[�W���󂯂Ă���
    }

    public PlayerCondition playerCondition;             // �v���C���[���L�̏��
    private PlayerState currentState = null;            // �v���C���[�̌��݂̏�Ԃ̓���


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
            case PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerCondition.Dead:
                break;
            case PlayerCondition.Damaged:
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

        // ���ɓ����Ă���
        if (trigger && trigger.CompareTag("water"))
        {
            ChangePlayerCondition(PlayerCondition.Swimming);
        }
        else
        {
            ChangePlayerCondition(PlayerCondition.Ground);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger = collision;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        trigger = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        trigger = null;
    }


    /**
     *  @brief 	�v���C���[�̏�ԑJ�ڂ��s������
     *  @param  PlayerCondition _changeCondition    �ύX��̃v���C���[�̏��
    */
    void    ChangePlayerCondition(PlayerCondition _changeCondition)
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
            case PlayerCondition.Ground:
                this.currentState = new PlayerStateGround();
                break;
            case PlayerCondition.Flying:
                this.currentState = new PlayerStateFlying();
                break;
            case PlayerCondition.Swimming:
                this.currentState = new PlayerStateSwimming();
                break;
            case PlayerCondition.Dead:
                break;
            case PlayerCondition.Damaged:
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
