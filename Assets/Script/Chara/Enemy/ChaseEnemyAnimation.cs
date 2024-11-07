using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChaseEnemyAnimation : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        Debug.Log("CP" + GimmickCheckpointParam.GetCheckpointNum());
        switch (currentSceneName)
        {
            case "lvl_01":
                // Scene1�̏���
                switch (GimmickCheckpointParam.GetCheckpointNum())
                {
                    case 0:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j0
                        break;
                    case 1:
                        animator.Play("Wolf_Move", 0, 0.15f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j0.15f
                        break;
                    case 2:
                        animator.Play("Wolf_Move", 0, 0.33f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j0.33f
                        break;
                    case 3:
                        animator.Play("Wolf_Move", 0, 0.47f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j0.47f
                        break;
                    case 4:
                        animator.Play("Wolf_Move", 0, 0.8f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j0.8f
                        break;
                }
                break;

            case "lev_02":
                // Scene2�̏���
                switch (GimmickCheckpointParam.GetCheckpointNum())
                {
                    case 0:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 1:
                        animator.Play("Wolf_Move", 0, 0.16f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 2:
                        animator.Play("Wolf_Move", 0, 0.27f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 3:
                        animator.Play("Wolf_Move", 0, 0.44f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 4:
                        animator.Play("Wolf_Move", 0, 0.54f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 5:
                        animator.Play("Wolf_Move", 0, 0.8f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                }
                break;

            case "lev_03":
                // Scene3�̏���
                switch (GimmickCheckpointParam.GetCheckpointNum())
                {
                    case 0:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 1:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 2:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 3:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 4:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                }
                break;

            case "lev_04":
                // 4�̃V�[�����̏���
                switch (GimmickCheckpointParam.GetCheckpointNum())
                {
                    case 0:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 1:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 2:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 3:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                    case 4:
                        animator.Play("Wolf_Move", 0, 0.0f); // 0.5�̓A�j���[�V�����̐��K���^�C���i0.0�`1.0�j
                        break;
                }
                break;
        }

    }
    
}
