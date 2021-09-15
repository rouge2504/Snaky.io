using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float audioSpeed;

    [SerializeField] private List<AudioItem> audioItems;
    private AudioSource audioSource;


    [Header("Volumens")]
    [SerializeField] private float soundVolume;
    [SerializeField] private float musicVolume;
    [SerializeField] private OptionsMusic settingMusic;
    [SerializeField] private OptionsMusic settingSounds;

    private void Start()
    {
        instance = this;
        audioSource = this.GetComponent<AudioSource>();
        musicVolume = PlayerPrefs.GetFloat(GameUtils.PREFS_SOUND_VOLUME,1);
        soundVolume = PlayerPrefs.GetFloat(GameUtils.PREFS_SOUND_TOGGLE,1);
        settingMusic.SetSetting(musicVolume);
        settingSounds.SetSetting(soundVolume);

    }

    public void SetVolumeMusic(Slider setting)
    {
        audioSource.volume = setting.value;
        settingMusic.SetSetting(setting.value);
        PlayerPrefs.SetFloat(GameUtils.PREFS_SOUND_VOLUME, audioSource.volume);
    }

    public void SetVolumeSound(Slider setting)
    {
        soundVolume = setting.value;
        settingSounds.SetSetting(setting.value);
        PlayerPrefs.SetFloat(GameUtils.PREFS_SOUND_TOGGLE, soundVolume);
    }

    public void PlayAudio(string name)
    {
        int index = audioItems.FindIndex(
            delegate (AudioItem item)
            {
                return item.name.Equals(name, StringComparison.Ordinal);
            });

        audioSource.PlayOneShot(audioItems[index].clip, soundVolume);

    }

    public void PlayAudio(string name, float volume)
    {
        int index = audioItems.FindIndex(
            delegate (AudioItem item)
            {
                return item.name.Equals(name, StringComparison.Ordinal);
            });

        audioSource.PlayOneShot(audioItems[index].clip, volume);

    }

    public void PlayAudio(string name, bool useVolume)
    {
        int index = audioItems.FindIndex(
            delegate (AudioItem item)
            {
                return item.name.Equals(name, StringComparison.Ordinal);
            });

        audioSource.PlayOneShot(audioItems[index].clip, useVolume ? audioItems[index].volume : soundVolume);

    }

    public void PlayAudioCourutine(string name)
    {
        int index = audioItems.FindIndex(
    delegate (AudioItem item)
    {
        return item.name.Equals(name, StringComparison.Ordinal);
    });
        StartCoroutine(AudioLoop(index));
    }

    public void StopCourtine(string name)
    {
        int index = audioItems.FindIndex(
    delegate (AudioItem item)
    {
        return item.name.Equals(name, StringComparison.Ordinal);
    });
        StopCoroutine(AudioLoop(index));
    }

    IEnumerator AudioLoop(int index)
    {
        while (true)
        {
            audioSource.PlayOneShot(audioItems[index].clip, soundVolume);
            yield return new WaitForSeconds(audioItems[index].clip.length);
        }
    }

    public void PlayBackground(string name, bool normalSpeed = true)
    {
        int index = audioItems.FindIndex(
            delegate (AudioItem item)
            {
                return item.name.Equals(name, StringComparison.Ordinal);
            });

        audioSource.volume = audioItems[index].volume;
        audioSource.clip = audioItems[index].clip;
        audioSource.Play();

        if (normalSpeed)
        {
            NormalSpeed();
        }
    }


    public void Accelerate()
    {
        audioSource.pitch = audioSpeed;
    }

    public void NormalSpeed()
    {
        audioSource.pitch = 1;
    }

    public void StopAudio()
    {
        audioSource.Stop();
        NormalSpeed();
    }
}

[Serializable]

public class AudioItem
{
    public string name;
    public AudioClip clip;
    public float volume = 1;

}