﻿using UnityEngine;

namespace SoundManager
{
    /// <summary>
    /// A sound bank asset. Used to link a collection of AudioClips and provide data about how they should be played.
    /// </summary>
    public abstract class SoundBank : ScriptableObject
    {
        public float LastPlayedTime => _lastPlayedTime;
        public float TimeSinceLastPlayed => GetCurrentTime() - _lastPlayedTime;
        public SoundInstance LastPlayedSound => _lastPlayedSound;

#if UNITY_EDITOR
        [SerializeField]
        [HideInInspector]
        private bool _expandFilterGUI;
#endif

        [SerializeField]
        [HideInInspector]
        protected DistortionSoundBankFilter _distortionFilter;

        [SerializeField]
        [HideInInspector]
        protected LowPassSoundBankFilter _lowPassFilter;

        [SerializeField]
        [HideInInspector]
        protected HighPassSoundBankFilter _highPassFilter;

        [SerializeField]
        [HideInInspector]
        protected ChorusSoundBankFilter _chorusFilter;

        [SerializeField]
        [HideInInspector]
        protected EchoSoundBankFilter _echoFilter;

        [SerializeField]
        [HideInInspector]
        protected ReverbSoundBankFilter _reverbFilter;

        [SerializeField]
        [HideInInspector]
        protected StartFadeSoundBankFilter _startFadeFilter;

        [SerializeField]
        [HideInInspector]
        protected EndFadeSoundBankFilter _endFadeFilter;

        [SerializeField]
        [HideInInspector]
        protected VolumeCurveSoundBankFilter _volumeCurveFilter;

        [SerializeField]
        [HideInInspector]
        protected PitchCurveSoundBankFilter _pitchCurveFilter;

        private SoundInstance _lastPlayedSound;
        private float _lastPlayedTime = Mathf.NegativeInfinity;

#if UNITY_EDITOR
        public abstract SoundInstance TestInEditor(ISoundPool soundPool);
#endif

        public void OnPlayed(SoundInstance sound)
        {
            _lastPlayedTime = GetCurrentTime();
            _lastPlayedSound = sound;
        }

        protected void AddFilters(SoundInstance sound)
        {
            sound.AddFilter(_distortionFilter);
            sound.AddFilter(_lowPassFilter);
            sound.AddFilter(_highPassFilter);
            sound.AddFilter(_chorusFilter);
            sound.AddFilter(_echoFilter);
            sound.AddFilter(_reverbFilter);
            sound.AddFilter(_startFadeFilter);
            sound.AddFilter(_endFadeFilter);
            sound.AddFilter(_volumeCurveFilter);
            sound.AddFilter(_pitchCurveFilter);
        }

        float GetCurrentTime()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying || UnityEditor.EditorApplication.isPaused)
            {
                return (float)UnityEditor.EditorApplication.timeSinceStartup;
            }
#endif

            return Time.unscaledTime;
        }

        protected virtual void OnEnable()
        {
            _lastPlayedSound = null;
            _lastPlayedTime = Mathf.NegativeInfinity;
        }

        public abstract void ApplyDefaultValues();


#if UNITY_EDITOR
        [ContextMenu("Edit Default Values")]
        void EditDefaultValues()
        {
            UnityEditor.Selection.activeObject = SoundManagerSettings.Instance;
        }
#endif

        void Reset()
        {
            ApplyDefaultValues();
        }

    }

    public static class SoundBankExtenstions
    {
        /// <summary>
        /// Fetches and then plays a non-spatial sound, ie. one that does not emit from a specific location and rolloff.
        /// </summary>
        /// <param name="soundBank">The sound bank used to set-up the sound instance</param>
        public static SoundInstance Play(this SoundBank soundBank)
        {
            SoundInstance sound = (soundBank as EffectSoundBank).Play();
            if (sound != null) return sound;

            sound = (soundBank as ImpactSoundBank).Play(0f);
            if (sound != null) return sound;

            sound = (soundBank as BlendSoundBank).Play();
            if (sound != null) return sound;

            sound = (soundBank as AmbienceSoundBank).Play();
            if (sound != null) return sound;

            sound = (soundBank as SequenceSoundBank).Play();

            return sound;
        }

        /// <summary>
        /// Fetches and then plays a sound at a specific position.
        /// </summary>
        /// <param name="position">The world-space position of the sound emission</param>
        public static SoundInstance Play(this SoundBank soundBank, Vector3 position)
        {
            SoundInstance sound = (soundBank as EffectSoundBank).Play(position);
            if (sound != null) return sound;

            sound = (soundBank as ImpactSoundBank).Play(position, 0f);
            if (sound != null) return sound;

            sound = (soundBank as BlendSoundBank).Play(position);
            if (sound != null) return sound;

            sound = (soundBank as AmbienceSoundBank).Play(position);
            if (sound != null) return sound;

            sound = (soundBank as SequenceSoundBank).Play(position);

            return sound;
        }

        /// <summary>
        /// Fetches and then plays a sound that will follow a transform around.
        /// </summary>
        /// <param name="followTransform">The transform to follow</param>
        public static SoundInstance Play(this SoundBank soundBank, Transform followTransform)
        {
            SoundInstance sound = (soundBank as EffectSoundBank).Play(followTransform);
            if (sound != null) return sound;

            sound = (soundBank as ImpactSoundBank).Play(followTransform, 0f);
            if (sound != null) return sound;

            sound = (soundBank as BlendSoundBank).Play(followTransform);
            if (sound != null) return sound;

            sound = (soundBank as AmbienceSoundBank).Play(followTransform);
            if (sound != null) return sound;

            sound = (soundBank as SequenceSoundBank).Play(followTransform);

            return sound;
        }
    }
}