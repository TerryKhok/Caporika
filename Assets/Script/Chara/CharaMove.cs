using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * @brief �v���C���[�ƓG�̈ړ������̊��N���X
 */
public class CharaMove : MonoBehaviour
{
    /**
     * @brief �v���C���[�ƓG���ʂ̏�Ԃ�\��
     */
    public enum CharaCondition
    {
        Ground,
        Flying,
        Swimming,
        Dead,
    }

    CharaCondition charaCondition = CharaCondition.Ground;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
