using System;
using UnityEngine;

namespace SoundManager
{
    // 声音服务接口，提供给外部调用。
    public interface ISoundService
    {
        void Play(string name);
    }
}
