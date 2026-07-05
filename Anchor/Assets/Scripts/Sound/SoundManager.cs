using UnityEngine;
using System.Collections.Generic;

namespace SoundManager
{
    /// 声音类型：音乐 / 音效
    public enum SoundType { Music, SFX }

    [System.Serializable]
    public class SoundItem
    {
        public string name;          // 声音名称（调用时使用）
        public SoundType type;       // 声音类型
        public AudioClip clip;       // 音频文件
        [Range(0f, 1f)]
        public float volume = 1f;    // 独立音量
        [Range(-3f, 3f)]
        public float pitch = 1f;     // 音调偏移
        public bool loop;            // 是否循环（仅SFX有效，音乐强制循环）
    }

    public class SoundManager : MonoBehaviour, ISoundService
    {
        public static SoundManager Instance { get; private set; }  // 单例

        private AudioSource musicSource;  // 音乐播放器
        private AudioSource sfxSource;    // 音效播放器

        [Header("Volume")]
        [Range(0f, 1f)]
        public float masterVolume = 1f;   // 主音量
        [Range(0f, 1f)]
        public float musicVolume = 1f;    // 音乐音量倍率
        [Range(0f, 1f)]
        public float sfxVolume = 1f;      // 音效音量倍率

        [Header("Sound Library")]
        public List<SoundItem> sounds = new List<SoundItem>();  // 声音库

        private SoundItem currentMusic;   // 当前正在播放的音乐

        private void Awake()
        {
            // 单例初始化
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 创建两个音频源
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            ApplyMusicVolume();
        }

        // 尝试通过名称查找 SoundItem
        private bool TryGetSound(string name, out SoundItem sound)
        {
            sound = sounds.Find(s => s.name == name);
            return sound != null && sound.clip != null;
        }

        // 将 SoundItem 应用到给定的 AudioSource 并播放
        private void ApplyToSource(AudioSource source, SoundItem sound)
        {
            source.clip = sound.clip;
            source.pitch = sound.pitch;
            source.loop = sound.type == SoundType.Music ? true : sound.loop;
            source.volume = GetEffectiveVolume(sound);
            source.Play();
        }

        // 通过名称播放声音
        public void Play(string name)
        {
            if (!TryGetSound(name, out var sound)) return;

            if (sound.type == SoundType.Music)
            {
                currentMusic = sound;
                source.volume = GetEffectiveVolume(sound);
                Debug.Log(source.clip);
                Debug.Log(source.loop);
                source.Play();
            }
            else
            {
                // 对于 SFX，始终使用覆盖播放模式，如果已有音效在 sfxSource 上播放，则停止它以覆盖为新的音效；否则直接播放新的音效。
                if (sfxSource.isPlaying)
                {
                    sfxSource.Stop();
                }

                sfxSource.clip = sound.clip;
                sfxSource.pitch = sound.pitch;
                sfxSource.loop = sound.loop;
                sfxSource.volume = GetEffectiveVolume(sound);
                sfxSource.Play();
                Debug.Log($"isPlaying:{name}");
            }

        }
        public void StopAll()
        {
            musicSource.Stop();
            sfxSource.Stop();
        }

        //计算最终音量 = 独立音量 × 类型音量 × 主音量
        private float GetEffectiveVolume(SoundItem sound)
        {
            if (sound == null) return 0f;

            float typeVolume = sound.type == SoundType.Music ? musicVolume : sfxVolume;
            return sound.volume * masterVolume * typeVolume;
        }

        //更新当前音乐的音量
        private void ApplyMusicVolume()
        {
            if (musicSource == null || currentMusic == null) return;

            musicSource.volume = GetEffectiveVolume(currentMusic);
        }

        //编辑器模式下修改参数时实时刷新音量
        private void OnValidate()
        {
            ApplyMusicVolume();
        }
    }
}