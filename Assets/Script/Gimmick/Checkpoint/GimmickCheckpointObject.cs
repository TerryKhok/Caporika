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
            Activate();
        }
    }

    /**
     * @brief   チェックポイントを有効化する
     */
    void Activate()
    {
        GimmickCheckpointParam.SetCheckpointNum(checkpointIndex, cameraSceneIndex);
    }
}
