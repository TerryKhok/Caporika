using UnityEngine;

/**
 * @brief �G�t�F�N�g�p�C�x���g�V�X�e��
 *        ��̃I�u�W�F�N�g�ɓ����蔻��(Trigger)�����Ă��̃R���|�[�l���g��ݒ肷��
 * 
 * @memo �E�g���K�[�ɓ�������ParticleSystem���Đ�����
 */
public class EffectTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect;     ///< �������������G�t�F�N�g�I�u�W�F�N�g
    [SerializeField] private bool triggerOnce = true;   ///< 1��̂ݔ����t���O

    private bool hasTriggered = false;  ///< �������������Ǘ�����t���O

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player") && (!this.triggerOnce || !this.hasTriggered)) // �E�̏�����1��̂ݐݒ�̊m�F
        {
            this.effect.Play();
            this.hasTriggered = true;
        }
    }
}
