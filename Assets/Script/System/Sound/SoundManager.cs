using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/**
 * @brief   音声の設定や再生を司るクラス
 *          これ一つをオブジェクトにアタッチする
 * 
 * @memo    ・SoundManagerの初期化
 *          ・再生したい音の登録
 *          ・BGMの再生
 *          ・SEの再生
 *          
 *          ・音データを表すクラス
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioMixerGroup bgmGroup = null;
    [SerializeField] AudioMixerGroup seGroup = null;

    private AudioSource bgmSource;     ///< BGM再生用のコンポーネント
    [SerializeField] private int seSourcePoolSize = 10; ///< 同時に再生できるSEの最大数

    [SerializeField] List<SoundRegistration> soundRegistration = new List<SoundRegistration>();
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();  ///< 登録されてるSoundDataのリスト
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
        RegisterSoundRegistrations();

        // 動作確認用
        //PlayBGM("BGM01");
    }

    /**
     * @brief 初期化処理
     */
    private void InitializeSESources()
    {
        // BGM再生用のAudioSourceを追加
        this.bgmSource = gameObject.AddComponent<AudioSource>();
        this.bgmSource.outputAudioMixerGroup = bgmGroup;
        this.bgmSource.loop = true;

        // SEが再生できるようにする数の分だけコンポーネントを追加
        this.seSources = new List<AudioSource>();

        for (int i = 0; i < this.seSourcePoolSize; i++)
        {
            AudioSource seSource = gameObject.AddComponent<AudioSource>();
            seSource.outputAudioMixerGroup = seGroup;
            this.seSources.Add(seSource);
        }
    }

    /**
     * @brief   Inspectorで設定したSoundRegistrationを登録していく
     */
    private void RegisterSoundRegistrations()
    {
        foreach (var regi in this.soundRegistration)
        {
            RegisterSound(regi.id, regi.audioClip, regi.volume);
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
    public void RegisterSound(string _id, AudioClip _clip, float _volume = 1.0f)
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
    public void PlayBGM(string _id)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // IDに音が登録されてるなら
        {
            // 今再生してるやつを止める
            this.bgmSource.Stop();
            // BGM用のやつで再生
            this.bgmSource.clip = soundData.clip;
            this.bgmSource.volume = soundData.volume;
            this.bgmSource.Play();
        }
    }

    /**
     * @brief SEを再生する
     * 
     * @param _id 登録されているSoundDataのID
     */
    public void PlaySE(string _id)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // IDに音が登録されてるなら
        {
            // 使えるコンポーネントを特定して
            AudioSource availableSource = GetAvailableSESource();
            if (availableSource != null)
            {
                // SE用のコンポーネントで再生
                availableSource.clip = soundData.clip;
                availableSource.volume = soundData.volume;
                availableSource.Play();
            }
        }
    }

    /**
     * @brief 使えるSE用のコンポーネントを探す
     * 
     * @return 使えるSoundSourceを返します。失敗したらnullを返します
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

    /**
     * @brief   SoundDataをInspectorから登録するための構造体
     */
    [System.Serializable]
    private struct SoundRegistration
    {
        public string id;           // 再生時に指定するときのID
        public AudioClip audioClip; // 再生データ
        public float volume;        // 0.0f ~ 1.0f
    }

}
