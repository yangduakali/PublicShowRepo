using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityExt.pool;

namespace UnityExt.audio;

public class AudioSourceInstance : PoolObject
{
    public AudioSource source;
    private bool _playing;
    private Action _onComplete;
    public AudioSourceInstance Play(AudioClip clip,
        AudioMixerGroup mixerGroup = null,
        bool loop = false, Action onComplete = null){
        _onComplete = onComplete;
        source.clip = clip;
        source.loop = loop;
        source.outputAudioMixerGroup = mixerGroup;
        source.Play();
        _playing = true;
        return this;
    }

    private void Update()
    {
        if (!_playing || source.isPlaying)
            return;
        _playing = false;
        _onComplete?.Invoke();
        Release();
    }
}