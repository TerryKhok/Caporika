using System.Collections.Generic;
using UnityEngine;

/**
 * @brief �����̐ݒ��Đ����i��N���X
 * 
 * @memo �E
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;     ///< BGM�Đ��p�̃R���|�[�l���g
    [SerializeField] private int seSourcePoolSize = 10; ///< �����ɍĐ��ł���SE�̍ő吔

    private float globalVolume = 1.0f;  ///< �S�̉��ʂ�ݒ肷��Ƃ�

    private Dictionary<int, SoundData> soundDictionary = new Dictionary<int, SoundData>();  ///< �o�^����Ă�SoundData�̃��X�g
    private List<AudioSource> seSources;    ///< SE�Đ��p�̃R���|�[�l���g�̃��X�g

    private void Awake()
    {
        // �V���O���g��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // ������
        InitializeSESources();
    }

    /**
     * @brief ����������
     */
    private void InitializeSESources()
    {
        // SE���Đ��ł���悤�ɂ��鐔�̕������R���|�[�l���g��ǉ�
        this.seSources = new List<AudioSource>();

        for (int i = 0; i < this.seSourcePoolSize; i++)
        {
            AudioSource seSource = this.gameObject.AddComponent<AudioSource>();
            this.seSources.Add(seSource);
        }
    }

    /**
     * @brief ���̓o�^
     * 
     * @param _id �o�^������SoundData��ID
     * @param _clip �o�^������AudioClip�t�@�C��
     * @param _volume �t�@�C���ʉ��ʁB�f�t�H���g1.0
     * 
     * @memo �E�g����
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
     * @brief BGM���Đ�����
     * 
     * @param _id �o�^����Ă���SoundData��ID
     */
    public void PlayBGM(int _id)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // ID�ɉ����o�^����Ă�Ȃ�
        {
            // BGM�p�̂�ōĐ�
            this.bgmSource.clip = soundData.clip;
            this.bgmSource.volume = this.globalVolume * soundData.volume;
            this.bgmSource.Play();
        }
    }

    /**
     * @brief SE���Đ�����
     * 
     * @param _id �o�^����Ă���SoundData��ID
     */
    public void PlaySE(int _id)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // ID�ɉ����o�^����Ă�Ȃ�
        {
            // �g����R���|�[�l���g����肵��
            AudioSource availableSource = GetAvailableSESource();
            if (availableSource != null)
            {
                // SE�p�̃R���|�[�l���g�ōĐ�
                availableSource.clip = soundData.clip;
                availableSource.volume = this.globalVolume * soundData.volume;
                availableSource.Play();
            }
        }
    }

    /**
     * @brief �g����SE�p�̃R���|�[�l���g��T��
     * 
     * @return �g����R���|�[�l���g��Ԃ��܂��B���s������null��Ԃ��܂�
     */
    private AudioSource GetAvailableSESource()
    {
        foreach (AudioSource source in this.seSources)
        {
            if (!source.isPlaying)  // �g�p�����m�F
            {
                return source;
            }
        }
        return null; // �S���g���Ȃ�������null��Ԃ�
    }

    /**
     * @brief �S�̉��ʂ�ݒ�
     * 
     * @param �ݒ肵�������ʁB0~1�̒l
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
     * @brief BGM���ʂ�ݒ�
     * 
     * @param �ݒ肵�������ʁB0~1�̒l
     */
    public void SetBGMVolume(float _volume)
    {
        this.bgmSource.volume = this.globalVolume * _volume;
    }

    /**
     * @brief SE���ʂ�ݒ�
     * 
     * @param �ݒ肵�������ʁB0~1�̒l
     */
    public void SetSEVolume(float _volume)
    {
        foreach (AudioSource source in this.seSources)
        {
            source.volume = this.globalVolume * _volume;
        }
    }

    /**
     * @brief �o�^����鉹�̃f�[�^���܂Ƃ߂��N���X
     */
    private class SoundData
    {
        public AudioClip clip;  ///< �Đ������AudioClip�t�@�C��
        public float volume;    ///< �t�@�C�����Ƃ̉��ʐݒ�

        /**
         * @brief �R���X�g���N�^
         */
        public SoundData(AudioClip _clip, float _volume)
        {
            this.clip = _clip;
            this.volume = Mathf.Clamp01(_volume);
        }
    }

}
