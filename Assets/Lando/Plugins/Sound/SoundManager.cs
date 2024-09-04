using System.Collections.Generic;
using Lando.Core.Extensions;
using Lando.Plugins.Singletons.MonoBehaviour;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

namespace Lando.Plugins.Sound
{
    public class SoundManager : Singleton<SoundManager>
    {
        [Header("Settings")]
        [SerializeField] private int _sfxSourceAmount = 10;
        
        private readonly List<AudioSource> _musicSources = new();
        private AudioSource[] _sfxSources;

        private int _currentSFXSourceIndex;
        
        private float _previousGlobalVolume = 1;
        
        private readonly Dictionary<SoundDatabase.AudioData, AudioSource> _loopedSounds = new();

        public static bool IsMuted => AudioListener.volume == 0;
        public int MusicLayers => _musicSources.Count;

        protected override void Awake()
        {
            base.Awake();
            
            _sfxSources = new AudioSource[_sfxSourceAmount];
            for (int i = 0; i < _sfxSourceAmount; i++)
            {
                _sfxSources[i] = gameObject.AddComponent<AudioSource>();
                _sfxSources[i].loop = false;
                _sfxSources[i].playOnAwake = false;
            }
        }

        public static void SetMusicLayerVolume(int layer, float volume)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            source.volume = volume;
        }

        public static float GetMusicLayerVolume(int layer)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            return source.volume;
        }
        
        public static bool IsLayerPlaying(int layer)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            return source.isPlaying;
        }
        
        public static bool IsMusicPlaying(AudioClip clip)
        {
            foreach (AudioSource source in _instance._musicSources)
            {
                if (source.clip == clip && source.isPlaying)
                    return true;
            }

            return false;
        }

        public static void PlayMusic(AudioClip audioClip, int layer = 0)
        {
            SoundDatabase.AudioData data = new(name: "Custom", loop: true, clips: new[] { audioClip });
            PlayMusic(data, layer);
        }
        
        public static void PlayMusic(SoundDatabase.AudioData data, int layer = 0)
        {
            GetOrCreateMusicSource(layer, out AudioSource musicSource);
            musicSource.clip = data.Clip;
            musicSource.Play();
        }
        
        public static void StopMusic(int layer = 0)
        {
            GetOrCreateMusicSource(layer, out AudioSource source);
            source.Stop();
            source.clip = null;
        }
        
        private static void GetOrCreateMusicSource(int layer, out AudioSource source)
        {
            bool isValid = layer >= 0 && layer < _instance._musicSources.Count;
            source = isValid ? _instance._musicSources[layer] : CreateLayer();
        }

        private static AudioSource CreateLayer()
        {
            AudioSource source = _instance.gameObject.AddComponent<AudioSource>();
            source.loop = true;
            source.playOnAwake = false;
            _instance._musicSources.Add(source);
            
            return source;
        }

        public static void PlaySFX(AudioClip audioClip, bool loop = false)
        {
            SoundDatabase.AudioData data = new(name: "Custom", loop, clips: new[] { audioClip });
            PlaySFX(data);
        }
        
        public static void PlaySFX(SoundDatabase.AudioData data)
        {
            AudioSource source = GetAvailableSource();
            source.loop = data.Loop;
            source.clip = data.Clip;
            source.Play();
            
            if (data.Loop)
                _instance._loopedSounds.Add(data, source);
        }
        
        public static void StopSFX(SoundDatabase.AudioData data)
        {
            Dictionary<SoundDatabase.AudioData, AudioSource> loopedSounds = _instance._loopedSounds;
            
            if (!loopedSounds.TryGetValue(data, out AudioSource source)) 
                return;
            
            source.Stop();
            loopedSounds.Remove(data);
        }

        private static AudioSource GetAvailableSource()
        {
            int checks = 0;
            IncrementIndex();
            while (_instance._sfxSources[_instance._currentSFXSourceIndex].loop)
            {
                IncrementIndex();
                checks++;
                if (checks >= _instance._sfxSourceAmount)
                    return _instance._sfxSources[0];
            }
            
            if (_instance._currentSFXSourceIndex >= _instance._sfxSourceAmount)
                _instance._currentSFXSourceIndex = 0;

            return _instance._sfxSources.GetInBounds(ref _instance._currentSFXSourceIndex);
            
            void IncrementIndex()
            {
                _instance._currentSFXSourceIndex++;
                if (_instance._currentSFXSourceIndex >= _instance._sfxSourceAmount)
                    _instance._currentSFXSourceIndex = 0;
            }
        }

        public static void SetGlobalVolume(float value)
        {
            if (!IsMuted)
                _instance._previousGlobalVolume = value;
            AudioListener.volume = value;
        }
        
        public static void Mute() => SetGlobalVolume(0);
        public static void Unmute() => AudioListener.volume = _instance._previousGlobalVolume;

        public static void DeleteLayerAt(int index)
        {
            List<AudioSource> musicSources = _instance._musicSources;
            musicSources[index].Stop();
            musicSources[index].clip = null;
            musicSources.RemoveAt(index);
        }
    }
}