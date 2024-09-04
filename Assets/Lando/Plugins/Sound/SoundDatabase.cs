using System;
using Lando.Core.Extensions;
using Lando.Plugins.Singletons.ScriptableObject;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lando.Plugins.Sound
{
    [CreateAssetMenu(fileName = "Sound Database", menuName = "Lando/Plugins/Sound/Sound Database")]
    public partial class SoundDatabase : Singleton<SoundDatabase>
    {
        [SerializeField] private AudioCategory[] _categories;
        [SerializeField] private string _filePath;

        [Serializable]
        public class AudioCategory
        {
            [SerializeField] private string _name;
            [SerializeField] private AudioData[] _audios;
            
            public string Identifier
            {
                get => _name.ReplaceWhitespace();
                set => _name = value;
            }

            public AudioData[] Audios => _audios;
        }
        
        [Serializable]
        public class AudioData
        {
            [SerializeField] private string _name;
            [SerializeField] private bool _loop;
            [SerializeField] private AudioClip[] _clips;
            
            public string Identifier
            {
                get => _name.ReplaceWhitespace();
                set => _name = value;
            }

            public bool Loop
            {
                get => _loop;
                set => _loop = value;
            }

            public AudioClip[] Clips => _clips;
            public AudioClip Clip => _clips[Random.Range(0, _clips.Length)];
            
            public AudioData(string name, bool loop, AudioClip[] clips)
            {
                _name = name;
                _loop = loop;
                _clips = clips;
            }
        }
    }
}