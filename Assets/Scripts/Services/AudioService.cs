using BimiBooTest.Configs;
using UnityEngine;

namespace BimiBooTest.Services
{
    public class AudioService
    {
        private readonly AudioSource _audioSource;
        private readonly AudioConfig _audioConfig;

        public AudioService(AudioSource audioSource, AudioConfig audioConfig)
        {
            _audioSource = audioSource;
            _audioConfig = audioConfig;
        }

        public void PlayGetSound() => _audioSource.PlayOneShot(_audioConfig.GetSound);

        public void PlayRightSound() => _audioSource.PlayOneShot(_audioConfig.RightSound);

        public void PlayWrongSound() => _audioSource.PlayOneShot(_audioConfig.WrongSound);

        public void PlayFinalSound() => _audioSource.PlayOneShot(_audioConfig.FinalSound);
    }
}