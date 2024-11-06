using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @brief   ドア開閉ギミックのスクリプト
 * @memo    ・設定されたパラメータに応じたアニメーションを作成して再生する
 *          ・Open()関数が実行されたら開く。
 *          ・Close()関数が実行されたら閉じる。
 *          
 *          ボタンのPrefab経由で実行する想定で作成
 * 
 */
[RequireComponent(typeof(Animator))]  // Animatorコンポーネントが必須
public class GimmickSwitchDoor : MonoBehaviour
{
    public float deltaY = 10f;            // アニメーションでのY座標変化量
    public float duration = 2f;         // アニメーションの持続時間

    private Animator animator;          // Animatorコンポーネントのキャッシュ
    private AnimationCurve xCurve;      // x方向のアニメーションカーブ（変化しない）
    private AnimationCurve zCurve;      // z方向のアニメーションカーブ（変化しない）

    // Start is called before the first frame update
    void Start()
    {
        // アニメーション作成時に使うカーブを作成
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0, transform.localPosition.x);
        keys[1] = new Keyframe(duration, transform.localPosition.x);
        xCurve = new AnimationCurve(keys);

        keys[0] = new Keyframe(0, transform.localPosition.z);
        keys[1] = new Keyframe(duration, transform.localPosition.z);
        zCurve = new AnimationCurve(keys);

        // 普通の初期化処理
        animator = GetComponent<Animator>();
        OverrideAnimationClip();
    }

    /**
     * @brief   アニメーションを作成して上書き
     * 
     * パラメータからアニメーションを作成してデフォルトのクリップを上書きする
     */
    void OverrideAnimationClip()
    {
        // アニメーション作成
        var localY = transform.localPosition.y;
        var openAnimClip = CreateAnimationClip(localY, localY + deltaY, duration);
        var closeAnimClip = CreateAnimationClip(localY + deltaY, localY, duration);

        // 作成したAnimationClipをAnimatorに追加
        if (this.animator != null)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController(this.animator.runtimeAnimatorController);
            overrideController["SwitchDoorOpen"] = openAnimClip;
            overrideController["SwitchDoorClose"] = closeAnimClip;
            this.animator.runtimeAnimatorController = overrideController;
        }
    }

    /**
     * @brief   アニメーションのクリップを作成する処理
     * @param   _startY     アニメーション開始時のY座標
     * @param   _endY       アニメーション終了時のY座標
     * @param   _duration   アニメーションの時間
     * @return  作成したAnimationClip
     */
    private AnimationClip CreateAnimationClip(float _startY, float _endY, float _duration)
    {
        // アニメーションクリップを作成
        AnimationClip clip = new AnimationClip
        {
            frameRate = 30
        };

        // キーフレームを設定
        Keyframe[] keys = new Keyframe[2];
        keys[0] = new Keyframe(0, _startY);
        keys[1] = new Keyframe(_duration, _endY);

        // クリップにカーブを設定
        AnimationCurve curve = new AnimationCurve(keys);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);

        // あらかじめ用意してあるカーブも適用
        clip.SetCurve("", typeof(Transform), "localPosition.x", this.xCurve);
        clip.SetCurve("", typeof(Transform), "localPosition.z", this.zCurve);

        return clip;
    }

    public void Open()
    {
        SoundManager.Instance.PlaySE("STAGE_DOOR");
        this.animator.SetBool("Open", true);
    }

    public void Close()
    {
        this.animator.SetBool("Open", false);
    }
}
