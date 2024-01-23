using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
    public AudioMixer audioMixer;
    private const string PLAYER_PREFS_MUTE_SETTING = "muteFlag";
    private bool muteFlag = false;

    private void Awake(){
        Instance = this;
    }

    public void EnsureSFXDestruction(AudioSource source)
    {
        StartCoroutine("DelayedSFXDestruction", source);
    }

    private IEnumerator DelayedSFXDestruction(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }

        GameObject.Destroy(source.gameObject);
    }

    public void SetAllAudioSources()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_MUTE_SETTING)){
            int muteVal = PlayerPrefs.GetInt(PLAYER_PREFS_MUTE_SETTING, 0);
            if (muteVal > 0){muteFlag = true;}
            else{muteFlag = false;}
        }

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.mute = muteFlag;
        }

        AudioListener.pause = muteFlag;
    }
}
