using UnityEngine;

namespace BimiBooTest.Configs
{
    [CreateAssetMenu(fileName = nameof(AudioConfig), menuName = "BimiBooTest/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        public AudioClip GetSound => _getSound;

        public AudioClip RightSound => _rightSound;

        public AudioClip WrongSound => _wrongSound;

        public AudioClip FinalSound => _finalSound;

        [SerializeField]
        private AudioClip _getSound;

        [SerializeField]
        private AudioClip _rightSound;

        [SerializeField]
        private AudioClip _wrongSound;

        [SerializeField]
        private AudioClip _finalSound;
    }
}