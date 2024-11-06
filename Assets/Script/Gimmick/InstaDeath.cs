using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaDeath : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D _other)
    {
        Debug.Log("unti");
        // マトリョーシカの後ろに来たとき
        if (_other.CompareTag("Player"))
        {
            // マトリョーシカが死んでいなければ
            PlayerMove playerMove = _other.GetComponent<PlayerMove>();
            if(playerMove.playerCondition!=PlayerState.PlayerCondition.Dead)
            {

                // プレイヤーをゲームオーバーにする
                MatryoshkaManager playerManager = FindAnyObjectByType<MatryoshkaManager>();
                if (!playerManager)
                {
                    Debug.LogError("MatryoshkaManagerを取得できませんでした。");
                    return;
                }
                int currentLife = playerManager.GetCurrentLife();
                playerMove.ChangePlayerCondition(PlayerState.PlayerCondition.Dead);
                playerManager.GameOver();
            }
        }
    }
}
