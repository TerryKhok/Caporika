using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ûôÇÃÇΩÇﬂÇÃScript
 * 
 * @memo    ÅEà⁄ìÆÇµÇ»Ç¢
 *          ÅEéÄÇ»Ç»Ç¢
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
