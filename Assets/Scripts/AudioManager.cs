using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    // References to the AudioSource components
    public AudioSource sfxAudioSource; // For playing sound effects
    public AudioSource bgmAudioSource; // For playing background music

    public AudioClip[] clips;

    // Dictionary to store audio clips by name
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


    // Reference to the background music clip
    public AudioClip backgroundMusicClip;

    void Start()
    {
        // Add loaded audio clips to the dictionary with their names as keys
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        // Debug.Log(audioClips);

        PlayBackgroundMusic();
    }

    // Method to play a specific audio clip by name
    public void PlayAudioClip(string clipName)
    {
        // Check if the clip name exists in the dictionary
        if (audioClips.ContainsKey(clipName))
        {
            // Stop any currently playing audio
            sfxAudioSource.Stop();

            // Set the audio clip and play it
            sfxAudioSource.clip = audioClips[clipName];
            sfxAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip with name " + clipName + " does not exist!");
        }
    }

    // Method to play background music
    public void PlayBackgroundMusic()
    {
        // Check if the background music clip is valid
        if (backgroundMusicClip != null)
        {
            // Set the background music clip and play it on loop
            bgmAudioSource.clip = backgroundMusicClip;
            bgmAudioSource.loop = true;
            bgmAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background music clip is not assigned!");
        }
    }

    // Method to stop background music
    public void StopBackgroundMusic()
    {
        bgmAudioSource.Stop();
    }
}
