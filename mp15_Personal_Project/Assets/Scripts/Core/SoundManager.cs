using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BgmAudioClip
{
    public EBgmType Type;
    public AudioClip Clip;
}

[Serializable]
public struct SfxAudioClip
{
    public ESfxType Type;
    public AudioClip Clip;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    
    [SerializeField] private BgmAudioClip[] _bgmClipList;
    [SerializeField] private SfxAudioClip[] _sfxClipList;

    [Header("Volume")]
    [Tooltip("배경음 음량. 효과음이 묻히지 않도록 낮게 둔다")]
    [SerializeField, Range(0f, 1f)] private float _bgmVolume = 0.35f;

    [Header("BGM Fade")]
    [SerializeField] private float _bgmFadeOutDuration = 0.4f;
    [SerializeField] private float _bgmFadeInDuration = 0.6f;
    
    private Dictionary<EBgmType, AudioClip> _bgmClips = new();
    private Dictionary<ESfxType, AudioClip> _sfxClips = new();

    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private float _masterVolume = 1f;

    // 크로스페이드용 두 번째 배경음 소스 (런타임에 _bgmSource 설정을 복사해 만든다)
    private AudioSource _bgmSubSource;
    private Coroutine _bgmFadeRoutine;

    public float MasterVolume => _masterVolume;

    private void Awake()
    {
        SetSingleton();
        Init();
    }

    // 플레이 중 인스펙터에서 음량을 바꾸면 바로 들리게
    private void OnValidate()
    {
        if (!Application.isPlaying || _bgmSource == null || _bgmFadeRoutine != null) return;
        _bgmSource.volume = _bgmVolume;
    }

    private void Start()
    {
        PlayBgm(EBgmType.Title);
    }

    public void PlayBgm(EBgmType type)
    {
        if (!_bgmClips.TryGetValue(type, out AudioClip clip) || clip == null)
        {
            Debug.LogWarning($"{type} Bgm Not Found");
            return;
        }

        // 같은 곡이 이미 나오고 있으면 처음부터 다시 틀지 않는다
        if (_bgmSource.clip == clip && (_bgmSource.isPlaying || _bgmFadeRoutine != null)) return;

        // 이전 페이드가 진행 중이면 사라지던 곡은 바로 멈춘다
        if (_bgmFadeRoutine != null)
        {
            StopCoroutine(_bgmFadeRoutine);
            _bgmSubSource.Stop();
        }

        AudioSource from = _bgmSource;
        AudioSource to = _bgmSubSource;

        to.clip = clip;
        to.volume = 0f;

        _bgmSource = to;
        _bgmSubSource = from;
        _bgmFadeRoutine = StartCoroutine(CrossFadeBgm(from, to));
    }

    // 이전 곡을 먼저 줄여서 끈 다음 새 곡을 키운다 (두 곡이 겹쳐 들리지 않게)
    private IEnumerator CrossFadeBgm(AudioSource from, AudioSource to)
    {
        if (from.isPlaying)
        {
            yield return FadeVolume(from, from.volume, 0f, _bgmFadeOutDuration);
        }
        from.Stop();

        to.Play();
        yield return FadeVolume(to, 0f, _bgmVolume, _bgmFadeInDuration);

        _bgmFadeRoutine = null;
    }

    private IEnumerator FadeVolume(AudioSource source, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            // 일시정지(timeScale 0)와 씬 로딩 중 긴 프레임에도 자연스럽게
            elapsed += Mathf.Min(Time.unscaledDeltaTime, 0.05f);
            source.volume = Mathf.Lerp(start, end, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
        source.volume = end;
    }

    public void PlaySfx(ESfxType type)
    {
        if (_sfxClips.ContainsKey(type))
        {
            _sfxSource.PlayOneShot(_sfxClips[type]);
        }
        else
        {
            Debug.LogWarning($"{type} Sfx Not Found");
        }
    }

    public void StopBgm()
    {
        if (_bgmFadeRoutine != null)
        {
            StopCoroutine(_bgmFadeRoutine);
            _bgmFadeRoutine = null;
        }
        _bgmSource.Stop();
        _bgmSubSource.Stop();
    }

    public void StopSfx()
    {
        _sfxSource.Stop();
    }
    
    public void SetBgmVolume(float volume)
    {
        _bgmVolume = Mathf.Clamp(volume, 0f, 1f);
        if (_bgmFadeRoutine == null) _bgmSource.volume = _bgmVolume;
    }
    
    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    
    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = _masterVolume;
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, _masterVolume);
    }
    
    private void Init()
    {
        _masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
        AudioListener.volume = _masterVolume;

        _bgmSubSource = _bgmSource.gameObject.AddComponent<AudioSource>();
        _bgmSubSource.outputAudioMixerGroup = _bgmSource.outputAudioMixerGroup;
        _bgmSubSource.loop = true;
        _bgmSubSource.playOnAwake = false;
        _bgmSubSource.priority = _bgmSource.priority;
        _bgmSubSource.spatialBlend = _bgmSource.spatialBlend;
        _bgmSubSource.volume = 0f;

        foreach (BgmAudioClip bgm in _bgmClipList)
        {
            if (!_bgmClips.ContainsKey(bgm.Type))
            {
                _bgmClips.Add(bgm.Type, bgm.Clip);
            }
        }

        foreach (SfxAudioClip sfx in _sfxClipList)
        {
            if (!_sfxClips.ContainsKey(sfx.Type))
            {
                _sfxClips.Add(sfx.Type, sfx.Clip);
            }
        }
    }
}
