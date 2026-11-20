using System;
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
    
    private Dictionary<EBgmType, AudioClip> _bgmClips = new();
    private Dictionary<ESfxType, AudioClip> _sfxClips = new();

    private void Awake()
    {
        SetSingleton();
        Init();
    }

    public void PlayBgm(EBgmType type)
    {
        if (_bgmClips.ContainsKey(type))
        {
            _bgmSource.clip = _bgmClips[type];
            _bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"{type} Bgm Not Found");
        }
    }

    public void PlaySfx(ESfxType type)
    {
        if (_sfxClips.ContainsKey(type))
        {
            _sfxSource.clip = _sfxClips[type];
            _sfxSource.PlayOneShot(_sfxClips[type]);
        }
        else
        {
            Debug.LogWarning($"{type} Sfx Not Found");
        }
    }

    public void StopBgm()
    {
        _bgmSource.Stop();
    }

    public void StopSfx()
    {
        _sfxSource.Stop();
    }
    
    public void SetBgmVolume(float volume)
    {
        _bgmSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    
    public void SetSfxVolume(float volume)
    {
        _sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    
    public void AllSoundVolume(float volume)
    {
        _bgmSource.volume = Mathf.Clamp(volume, 0f, 1f);
        _sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    
    private void Init()
    {
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
