using System;
using UnityEngine;

namespace TileMatch.ScriptableObjects.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ParticleConfig", fileName = "ParticleConfig")]
    public class ParticleConfig : ScriptableObject
    {
        [SerializeField] private Particle[] _particles;
        public Particle[] Particles => _particles;
    }

    [Serializable]
    public class Particle
    {
        [SerializeField] private ParticleType _particleType;
        public ParticleType ParticleType => _particleType;
       
        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;
        
        [SerializeField] private float _duration;
        public float Duration => _duration;
    }

    public enum ParticleType
    {
        Match = 0
    }
}