using UnityEngine;

public class StageSpecificAnimation : MonoBehaviour
{
    public Animator animator;
    public AnimationClip stage1MoveClip;
    public AnimationClip stage2MoveClip;

    public int NowScene;

    void Start()
    {
        int currentStage = NowScene;  // �X�e�[�W�����擾����֐����쐬���Ă�������
        if (currentStage == 1)
        {
            OverrideAnimationClip("Move", stage1MoveClip);
        }
        else if (currentStage == 2)
        {
            OverrideAnimationClip("Move", stage2MoveClip);
        }
    }

    void OverrideAnimationClip(string clipName, AnimationClip newClip)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController[clipName] = newClip;
        animator.runtimeAnimatorController = overrideController;
    }
}
