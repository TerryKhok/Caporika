using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Audio;

/**
 * @brief   �����̐ݒ��Đ����i��N���X
 *          �������I�u�W�F�N�g�ɃA�^�b�`����
 * 
 * @memo    �ESoundManager�̏�����
 *          �E�Đ����������̓o�^
 *          �EBGM�̍Đ�
 *          �ESE�̍Đ�
 *          
 *          �E���f�[�^��\���N���X
 */
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] AudioMixerGroup bgmGroup = null;
    [SerializeField] AudioMixerGroup seGroup = null;

    private AudioSource bgmSource;     ///< BGM�Đ��p�̃R���|�[�l���g
    [SerializeField] private int seSourcePoolSize = 10; ///< �����ɍĐ��ł���SE�̍ő吔

    [SerializeField] List<SoundRegistration> soundRegistration = new List<SoundRegistration>();
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();  ///< �o�^����Ă�SoundData�̃��X�g
    private List<AudioSource> seSources;    ///< SE�Đ��p�̃R���|�[�l���g�̃��X�g

    private string currentBgm = "";
    private List<string> currentSe = new List<string> { };

    /**
     * @brief   �Đ�����bgm��ID���擾����
     */
    public string GetPlayingBgm() { return currentBgm; }

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
        RegisterSoundRegistrations();
    }

    /**
     * @brief ����������
     */
    private void InitializeSESources()
    {
        // BGM�Đ��p��AudioSource��ǉ�
        this.bgmSource = gameObject.AddComponent<AudioSource>();
        this.bgmSource.outputAudioMixerGroup = bgmGroup;
        this.bgmSource.loop = true;

        // SE���Đ��ł���悤�ɂ��鐔�̕������R���|�[�l���g��ǉ�
        this.seSources = new List<AudioSource>();

        for (int i = 0; i < this.seSourcePoolSize; i++)
        {
            AudioSource seSource = gameObject.AddComponent<AudioSource>();
            seSource.outputAudioMixerGroup = seGroup;
            this.seSources.Add(seSource);
        }
    }

    /**
     * @brief   Inspector�Őݒ肵��SoundRegistration��o�^���Ă���
     */
    private void RegisterSoundRegistrations()
    {
        foreach (var regi in this.soundRegistration)
        {
            RegisterSound(regi.id, regi.audioClip, regi.volume);
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
    public void RegisterSound(string _id, AudioClip _clip, float _volume = 1.0f)
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
     * @param _volume   �Đ�����{�����[��
     */
    public void PlayBGM(string _id, float _volume = 1.0f)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // ID�ɉ����o�^����Ă�Ȃ�
        {
            if (currentBgm == _id) return;

            // ���Đ����Ă����~�߂�
            this.bgmSource.Stop();
            // BGM�p�̂�ōĐ�
            this.bgmSource.clip = soundData.clip;
            this.bgmSource.volume = soundData.volume * _volume;
            this.bgmSource.Play();
            currentBgm = _id;
        }
        else
        {
            Debug.LogError("�T�E���h�f�[�^���o�^����Ă��܂���B" + _id);
        }
    }

    /**
     * @brief SE���Đ�����
     * 
     * @param _id �o�^����Ă���SoundData��ID
     * @param _dupe �������������ɍĐ�����̂����e���邩
     */
    public void PlaySE(string _id, bool _dupe = false)
    {
        if (this.soundDictionary.TryGetValue(_id, out SoundData soundData))  // ID�ɉ����o�^����Ă�Ȃ�
        {
            if (currentSe.Contains(_id) && !_dupe) return;

            // �g����R���|�[�l���g����肵��
            AudioSource availableSource = GetAvailableSESource();
            if (availableSource != null)
            {
                // SE�p�̃R���|�[�l���g�ōĐ�
                availableSource.clip = soundData.clip;
                availableSource.volume = soundData.volume;
                availableSource.Play();
                currentSe.Add(_id);
                StartCoroutine(SEPlayingCoroutine(availableSource, _id));
            }
        }
        else
        {
            Debug.LogError("�T�E���h�f�[�^���o�^����Ă��܂���B" + _id);
        }
    }

    /**
     * @brief   SE���Đ��I�������Ƃ��ɍĐ������X�g����ID���폜����
     * 
     * @param   _source �Ď�����AudioSource
     * @param   _id     �Đ��I�����ɍ폜����ID
     */
    IEnumerator SEPlayingCoroutine(AudioSource _source, string _id)
    {
        while (_source.isPlaying)
        {
            yield return null;
        }
        currentSe.Remove(_id);
    }

    /**
     * @brief �����_����SE���Đ�����
     * 
     * @param _ids �o�^����Ă���SoundData��ID�̔z��
     * @param _volume   �Đ����鉹�ʂɕ␳���|����B0.0f~1.0f
     */
    public void PlayRandomSE(List<string> _ids, float _volume = 1.0f)
    {
        int size = _ids.Count;
        int rand = Random.Range(0, size);
        string id = _ids[rand];
        if (this.soundDictionary.TryGetValue(id, out SoundData soundData))  // ID�ɉ����o�^����Ă�Ȃ�
        {
            // �g����R���|�[�l���g����肵��
            AudioSource availableSource = GetAvailableSESource();
            if (availableSource != null)
            {
                // SE�p�̃R���|�[�l���g�ōĐ�
                availableSource.clip = soundData.clip;
                availableSource.volume = soundData.volume * _volume;
                availableSource.Play();
            }
        }
        else
        {
            Debug.LogError("�T�E���h�f�[�^���o�^����Ă��܂���B" + id);
        }
    }

    /**
     * @brief �g����SE�p�̃R���|�[�l���g��T��
     * 
     * @return �g����SoundSource��Ԃ��܂��B���s������null��Ԃ��܂�
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

    /**
     * @brief   SoundData��Inspector����o�^���邽�߂̍\����
     */
    [System.Serializable]
    private struct SoundRegistration
    {
        public string id;           // �Đ����Ɏw�肷��Ƃ���ID
        public AudioClip audioClip; // �Đ��f�[�^
        public float volume;        // 0.0f ~ 1.0f
    }

}
