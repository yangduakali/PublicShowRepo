using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityExt.attributes;
using UnityExt.pool;

namespace UnityExt.audio;

[ClassIcon("UnityEditorExt.Resources.audio_cue.png")]
public class GameAudio : MonoBehaviour {
    
    private static GameAudio _instance;
    private Pool<AudioSourceInstance> _pool;

    private static GameAudio Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = new GameObject("Game Audio", typeof (GameAudio)).GetComponent<GameAudio>();
            _instance._pool = new Pool<AudioSourceInstance>(_instance.CreateSourceInstance);
            DontDestroyOnLoad(_instance.gameObject);
            return _instance;
        }
    }

    private AudioSourceInstance CreateSourceInstance()
    {
        var component = new GameObject("Audio Instance", typeof (AudioSourceInstance)).GetComponent<AudioSourceInstance>();
        component.source = component.gameObject.AddComponent<AudioSource>();
        component.source.playOnAwake = false;
        return component;
    }

    public static AudioSourceInstance Play(AudioClip clip, AudioMixerGroup mixerGroup = null, bool loop = false, Action onComplete = null)
    {
        if (clip != null) return Instance._pool.Get().Play(clip, mixerGroup, loop, onComplete);
#pragma warning disable CS8603
        return null;
#pragma warning restore CS8603
    }
    
}