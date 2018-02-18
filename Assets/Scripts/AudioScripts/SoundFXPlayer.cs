using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SoundFXPlayer : MonoBehaviour {
    #region main variables
    [Header("Volume")]
    public float volume = 1;
    public float minVolumeVariance = 0;
    public float maxVolumeVariance = 0;

    [Header("Pitch")]
    public float pitch = 1;
    public float minPitchVariance = 0;
    public float maxPitchVariance = 0;

    [Header("Other Properties")]
    public float delayBeforePlaySound = 0;

    public AudioClipStruct[] audioClipList = new AudioClipStruct[1];

    private AudioSource aSource;
    #endregion main variables



    #region monobehaviour methods
    private void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
        if (!aSource)
        {
            aSource = GetComponent<AudioSource>();
        }

        aSource.volume = this.volume;
        aSource.pitch = this.pitch;
    }
    #endregion monobehaviour methods

    #region sound methods
    /// <summary>
    /// Plays a new sound. If there is currently a sound occurring when this method
    /// is called, the previous sound will be stopped and a new one will be started
    /// </summary>
    public void PlaySound()
    {
        StopSound();
    }

    /// <summary>
    /// Pauses a currently playing sound. If there was no sound being
    /// played when this method was called, this method will return and do
    /// nothing to the audio source
    /// </summary>
    public void PauseSound()
    {
        aSource.Pause();
    }

    /// <summary>
    /// If we want to continue a sound if it was paused (for instance, by the player),
    /// we may want to continue the paused sound instead of starting the sound over
    /// with PlaySound()
    /// </summary>
    public void UnPauseSound()
    {
        aSource.UnPause();
    }

    /// <summary>
    /// Stops playing a sound entirely
    /// </summary>
    public void StopSound()
    {
        aSource.Stop();
    }
    #endregion sound methods

    #region helper methods
    protected virtual AudioClip GetNextPlayableClip()
    {
        //To-Do
        return null;
    }
    #endregion helper methods

    #region structs
    public struct AudioClipStruct
    {
        [Tooltip("The audio clip that will be played")]
        public AudioClip audioClip;
        [Range(0f, 1f)]
        [Tooltip("How likely it is for this sound to be played")]
        public float weight;
    }
    #endregion structs
}
