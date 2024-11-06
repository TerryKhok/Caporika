using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   �h�A�J�M�~�b�N�̃X�N���v�g
 * @memo    �E�ݒ肳�ꂽ�p�����[�^�ɉ������A�j���[�V�������쐬���čĐ�����
 *          �EOpen()�֐������s���ꂽ��J���B
 *          �EClose()�֐������s���ꂽ�����B
 *          
 *          �{�^����Prefab�o�R�Ŏ��s����z��ō쐬
 * 
 */
[RequireComponent(typeof(Animator))]  // Animator�R���|�[�l���g���K�{
public class GimmickSwitchDoor : MonoBehaviour
{
    public float deltaY = 10f;            // �A�j���[�V�����ł�Y���W�ω���
    public float duration = 2f;         // �A�j���[�V�����̎�������

    private Animator animator;          // Animator�R���|�[�l���g�̃L���b�V��
    private AnimationCurve xCurve;      // x�����̃A�j���[�V�����J�[�u�i�ω����Ȃ��j
    private AnimationCurve zCurve;      // z�����̃A�j���[�V�����J�[�u�i�ω����Ȃ��j

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�V�����쐬���Ɏg���J�[�u���쐬
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0, transform.localPosition.x);
        keys[1] = new Keyframe(duration, transform.localPosition.x);
        xCurve = new AnimationCurve(keys);

        keys[0] = new Keyframe(0, transform.localPosition.z);
        keys[1] = new Keyframe(duration, transform.localPosition.z);
        zCurve = new AnimationCurve(keys);

        // ���ʂ̏���������
        animator = GetComponent<Animator>();
        OverrideAnimationClip();
    }

    /**
     * @brief   �A�j���[�V�������쐬���ď㏑��
     * 
     * �p�����[�^����A�j���[�V�������쐬���ăf�t�H���g�̃N���b�v���㏑������
     */
    void OverrideAnimationClip()
    {
        // �A�j���[�V�����쐬
        var localY = transform.localPosition.y;
        var openAnimClip = CreateAnimationClip(localY, localY + deltaY, duration);
        var closeAnimClip = CreateAnimationClip(localY + deltaY, localY, duration);

        // �쐬����AnimationClip��Animator�ɒǉ�
        if (this.animator != null)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController(this.animator.runtimeAnimatorController);
            overrideController["SwitchDoorOpen"] = openAnimClip;
            overrideController["SwitchDoorClose"] = closeAnimClip;
            this.animator.runtimeAnimatorController = overrideController;
        }
    }

    /**
     * @brief   �A�j���[�V�����̃N���b�v���쐬���鏈��
     * @param   _startY     �A�j���[�V�����J�n����Y���W
     * @param   _endY       �A�j���[�V�����I������Y���W
     * @param   _duration   �A�j���[�V�����̎���
     * @return  �쐬����AnimationClip
     */
    private AnimationClip CreateAnimationClip(float _startY, float _endY, float _duration)
    {
        // �A�j���[�V�����N���b�v���쐬
        AnimationClip clip = new AnimationClip
        {
            frameRate = 30
        };

        // �L�[�t���[����ݒ�
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0, _startY);
        keys[1] = new Keyframe(_duration, _endY);

        // �N���b�v�ɃJ�[�u��ݒ�
        AnimationCurve curve = new AnimationCurve(keys);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

        // ���炩���ߗp�ӂ��Ă���J�[�u���K�p
        clip.SetCurve("", typeof(Transform), "localPosition.x", this.xCurve);
        clip.SetCurve("", typeof(Transform), "localPosition.z", this.zCurve);

        return clip;
    }

    public void Open()
    {
        SoundManager.Instance.PlaySE("STAGE_DOOR");
        this.animator.SetBool("Open", true);
    }

    public void Close()
    {
        this.animator.SetBool("Open", false);
    }
}
