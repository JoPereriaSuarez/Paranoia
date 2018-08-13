using UnityEngine;

namespace PointClick
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private SoundController musicController;
        [SerializeField] private SoundController bacgroundMusicController;
        [SerializeField] private SoundController sfxController;
        [SerializeField] private SoundController voiceController;

        public SoundController MusicController
        {
            get { return musicController; }
        }
        public SoundController BacgroundMusicController
        {
            get { return bacgroundMusicController; }
        }
        public SoundController SfxController
        {
            get { return sfxController; }
        }
        public SoundController VoiceController
        {
            get { return voiceController; }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameController.Instance.OnPaused += Instance_OnPaused;
            voiceController.OnVoicePlay += VoiceController_OnVoicePlay;
            voiceController.OnVoiceStop += VoiceController_OnVoiceStop;
        }

        private void VoiceController_OnVoiceStop(object sender, System.EventArgs e)
        {
            musicController.SetVolume(musicController.DefaultVolume);
            bacgroundMusicController.SetVolume(bacgroundMusicController.DefaultVolume);
            sfxController.SetVolume(sfxController.DefaultVolume);
        }

        private void VoiceController_OnVoicePlay(object sender, System.EventArgs e)
        {
            musicController.SetVolume(0.5F);
            bacgroundMusicController.SetVolume(0.5f);
            sfxController.SetVolume(0.5f);
        }

        private void Instance_OnPaused(object sender, bool value)
        {
            if(value)
            { PauseEverything(); }

            else
            { PlayEverything(); }
        }

        public void PauseEverything()
        {
            musicController.Pause();
            bacgroundMusicController.Pause();
            sfxController.Pause();
            voiceController.Pause();
        }

        /// <summary>
        /// Resume audio if paused 
        /// Play from beggining if has been stoped
        /// </summary>
        public void PlayEverything()
        {
            musicController.Resume();
            bacgroundMusicController.Resume();
            sfxController.Resume();
            voiceController.Resume();
        }


        public void StopEverything()
        {
            musicController.Stop();
            bacgroundMusicController.Stop();
            sfxController.Stop();
            voiceController.Stop();
        }
    }
}
