using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ���̂��߂�Script
 * 
 * @memo    �E�ړ����Ȃ�
 *          �E���ȂȂ�
 */
public class EnemyMoveSpike : CharaMove
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (this.GetCharaCondition() == CharaCondition.Dead)
        {
            this.SetCharaCondition(CharaCondition.Ground);
        }
    }
}
