using UnityEngine;

/**
 * @brief エフェクト用イベントシステム
 *        空のオブジェクトに当たり判定(Trigger)をつけてこのコンポーネントを設定する
 * 
 * @memo ・トリガーに入ったらParticleSystemを再生する
 */
public class EffectTrigger : MonoBehaviour
{
    [SerializeField] private ParticleSystem effect;     ///< 発生させたいエフェクトオブジェクト
    [SerializeField] private bool triggerOnce = true;   ///< 1回のみ発生フラグ

    private bool hasTriggered = false;  ///< 発生したかを管理するフラグ

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player") && (!this.triggerOnce || !this.hasTriggered)) // 右の条件は1回のみ設定の確認
        {
            this.effect.Play();
            this.hasTriggered = true;
        }
    }
}
