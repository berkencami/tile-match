using System.Linq;
using TileMatch.ScriptableObjects.Config;
using TileMatch.Utility;
using UnityEngine;

namespace TileMatch.Managers
{
    public class FXManager : Singleton<FXManager>
    {
        [SerializeField] private ParticleConfig _particleConfig;
        [SerializeField] private SoundConfig _soundConfig;
        private AudioSource _audioSource;
        private int _lastId;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public GameObject PlayParticle(ParticleType particleType, Vector3 pos, Quaternion rot, Transform parent = null)
        {
            var particle = _particleConfig.Particles.FirstOrDefault(p => p.ParticleType == particleType);
            
            if (particle == null)
            {
                return null;
            }
            
            var spawnedParticle = Instantiate(particle.Prefab, pos, rot, parent);
            if (parent != null)
            {
                spawnedParticle.transform.localPosition = pos;
            }

            if (spawnedParticle == null) return spawnedParticle;

            Destroy(spawnedParticle, particle.Duration);
            
            return spawnedParticle;
        }

        public void PlaySoundFX(SoundType soundType, int instanceID = 0)
        {
            var soundFx = _soundConfig.soundFx.Find(p => p.soundType == soundType);
           
            if (instanceID == _lastId && instanceID != 0)
                _audioSource.pitch += 0.1f;
            else
                _audioSource.pitch = 1f;

            _lastId = instanceID;
            
            _audioSource.PlayOneShot(soundFx.audioClip, soundFx.volume);
        }
    }
}