using System;
using System.Collections.Generic;
using UnityEngine;

namespace TileMatch.ScriptableObjects.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SoundConfig", fileName = "SoundConfig")]
    public class SoundConfig : ScriptableObject
    {
        public List<SoundFX> soundFx;
    }

    [Serializable]
    public class SoundFX
    {
        public SoundType soundType;
        public AudioClip audioClip;
        public float volume;
    }

    public enum SoundType
    {
        Match = 0,
        Collect = 1,
        Coin = 2
    }
}