using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   チェックポイントのオブジェクトに対して付ける
 * @memo    ・Triggerに触れたらチェックポイントのパラメータを更新する
 */
public class GimmickCheckpointObject : MonoBehaviour
{
    // MatryoshkaManagerのリストのインデックスと対応させる
    [SerializeField] private int checkpointIndex = 0;
    // CameraControllerの移すシーンのインデックスと対応させる
    [SerializeField] private int cameraSceneIndex = 0;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            // 死んでなかったらアクティベート
            var player = _collision.gameObject;
            if (player.GetComponent<PlayerMove>().playerCondition != PlayerState.PlayerCondition.Dead)
            {
                Activate();
            }
        }
    }

    /**
     * @brief   チェックポイントを有効化する
     *          その後残機回復
     */
    void Activate()
    {
        // 何回もアクティベートしない
        if (activated) return;

        SoundManager.Instance.PlaySE("STAGE_CHECKPOINT");
        GimmickCheckpointParam.SetCheckpointNum(checkpointIndex, cameraSceneIndex);
        // ここにマトリョシカの残機回復処理
        // マトリョシカを全て消して残機3のやつを生成
        // nullチェック全然してなくてクソ
        var manager = GameObject.Find("MatryoshkaManager");
        manager.GetComponent<MatryoshkaManager>().Respawn();

        activated = true;
    }
}
