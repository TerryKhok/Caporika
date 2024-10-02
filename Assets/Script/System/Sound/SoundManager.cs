using System.Collections.Generic;
using UnityEngine;

/**
 * @brief 音声の設定や再生を司るクラス
 * 
 * @memo ・
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;     ///< BGM再生用のコンポーネント
    [SerializeField] private int seSourcePoolSize = 10; ///< 同時に再生できるSEの最大数

    private float globalVolume = 1.0f;  ///< 全体音量を設定するとこ

    private Dictionary<int, SoundData> soundDictionary = new Dictionary<int, SoundData>();  ///< 登録されてるSoundDataのリスト
    private List<AudioSource> seSources;    ///< SE再生用のコンポーネントのリスト

    private void Awake()
    {
        // シングルトン
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // 初期化
        InitializeSESources();
    }

    /**
     * @brief 初期化処理
     */
    private void InitializeSESources()
    {
        // SEが再生できるようにする数の分だけコンポーネントを追加
        this.seSources = new List<AudioSource>();

        for (int i = 0; i < this.seSourcePoolSize; i++)
        {
            AudioSource seSource = this.gameObject.AddComponent<AudioSource>();
            this.seSources.Add(seSource);
        }
    }

    /**
     * @brief 音の登録
     * 
     * @param _id 登録したいSoundDataのID
     * @param _clip 登録したいAudioClipファイル
     * @param _volume ファイル別音量。デフォルト1.0
     * 
     * @memo ・使い方
     *          SoundManager.Instance.RegisterSound(1, Resources.Load<AudioClip>("Audio/BGM"), 0.7f);
     */
    public void RegisterSound(int _id, AudioClip _clip, float _volume = 1.0f)
    {
        if (!this.soundDictionary.ContainsKey(_id))
        {
            this.soundDictionary.Add(_id, new SoundData(_clip, _volume));
        }
    }

    /**
     * @brief BGMを再生する
     * 
     * @param _id 登録されているSoundDataのID
     */
    public void PlayBGM(int _id)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // IDに音が登録されてるなら
        {
            // BGM用のやつで再生
            this.bgmSource.clip = soundData.clip;
            this.bgmSource.volume = this.globalVolume * soundData.volume;
            this.bgmSource.Play();
        }
    }

    /**
     * @brief SEを再生する
     * 
     * @param _id 登録されているSoundDataのID
     */
    public void PlaySE(int _id)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // IDに音が登録されてるなら
        {
            // 使えるコンポーネントを特定して
            AudioSource availableSource = GetAvailableSESource();
            if (availableSource != null)
            {
                // SE用のコンポーネントで再生
                availableSource.clip = soundData.clip;
                availableSource.volume = this.globalVolume * soundData.volume;
                availableSource.Play();
            }
        }
    }

    /**
     * @brief 使えるSE用のコンポーネントを探す
     * 
     * @return 使えるコンポーネントを返します。失敗したらnullを返します
     */
    private AudioSource GetAvailableSESource()
    {
        foreach (AudioSource source in this.seSources)
        {
            if (!source.isPlaying)  // 使用中か確認
            {
                return source;
            }
        }
        return null; // 全部使えなかったらnullを返す
    }

    /**
     * @brief 全体音量を設定
     * 
     * @param 設定したい音量。0~1の値
     */
    public void SetGlobalVolume(float _volume)
    {
        this.globalVolume = Mathf.Clamp01(_volume);
        this.bgmSource.volume = this.globalVolume * (this.bgmSource.clip != null ? this.soundDictionary[this.bgmSource.clip.GetInstanceID()].volume : 1.0f);

        foreach (AudioSource source in seSources)
        {
            source.volume = this.globalVolume * source.volume;
        }
    }

    /**
     * @brief BGM音量を設定
     * 
     * @param 設定したい音量。0~1の値
     */
    public void SetBGMVolume(float _volume)
    {
        this.bgmSource.volume = this.globalVolume * _volume;
    }

    /**
     * @brief SE音量を設定
     * 
     * @param 設定したい音量。0~1の値
     */
    public void SetSEVolume(float _volume)
    {
        foreach (AudioSource source in this.seSources)
        {
            source.volume = this.globalVolume * _volume;
        }
    }

    /**
     * @brief 登録される音のデータをまとめたクラス
     */
    private class SoundData
    {
        public AudioClip clip;  ///< 再生されるAudioClipファイル
        public float volume;    ///< ファイルごとの音量設定

        /**
         * @brief コンストラクタ
         */
        public SoundData(AudioClip _clip, float _volume)
        {
            this.clip = _clip;
            this.volume = Mathf.Clamp01(_volume);
        }
    }

}
