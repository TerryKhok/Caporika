using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickTrampoline : MonoBehaviour
{
    public Transform targetPoint = null;   // ダメージ時の目標位置
    public float speed = 3.0f;       // ダメージ時の移動の速度
    public float height = 2.0f;      // ダメージ時の移動の高さの最大値（天井にぶつからないため）

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 当たったのがプレイヤーか敵なら
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Enemy"))
        {
            List<string> soundList = new List<string> { "STAGE_TRAMPOLINE_1", "STAGE_TRAMPOLINE_2" };
            SoundManager.Instance.PlayRandomSE(soundList);
            // 放物線移動用スクリプトをつけて飛ばす
            var move = collider.gameObject.AddComponent<ParabolicMove>();
            move.Init(targetPoint.transform, speed, height, true);
            move.Active();
        }
    }
}
