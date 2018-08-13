using System;
using System.Collections.Generic;
using UnityEngine;

namespace PointClick
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        public event EventHandler OnVoicePlay;
        public event EventHandler OnVoiceStop;

        private AudioSource source;

        public float DefaultVolume { get; private set; }

        /// <summary>
        /// Puede servir en caso de hacer que el audio se reprodusca en lista
        /// </summary>
        private List<AudioClip> audios = new List<AudioClip>();

        public bool IsPlaying
        {
            get { return source.isPlaying; }
        }

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            DefaultVolume = source.volume;
        }

        public void Play(AudioClip clip, bool loop = false)
        {
            if(clip == null ) { return; }
            if(clip != source.clip)
            {
                source.clip = clip;
            }
            source.loop = loop;
            source.Play();
            OnVoicePlay?.Invoke(this, EventArgs.Empty);
        }
        public void Stop()
        {
            source.Stop();
            OnVoiceStop?.Invoke(this, EventArgs.Empty);
        }
        public void Pause()
        {
            if (!IsPlaying || source.clip == null) { return; }

            source.Pause();
        }
        public void Resume()
        {
            if (IsPlaying || source.clip == null) { return; }

            if(source.time == 0.0f)
            { source.Play(); }
            else
            { source.UnPause(); }
        }

        public void SetVolume(float value)
        {
            value = Mathf.Clamp(value, 0.0f, 1.0f);
            source.volume = value;
        }
    }
}
