using UnityEngine;
using System.Collections.Generic;

public class SoundEffectPlayer : MonoBehaviour
{
    public int poolSize = 10;
    private List<AudioSource> audioSources;
    private int currentSourceIndex = 0;

private void Start()
{
    audioSources = new List<AudioSource>();
    for (int i = 0; i < poolSize; i++)
    {
        // Create a new gameobject for each audiosource
        GameObject audioSourceObj = new GameObject($"AudioSource_{i}");
        AudioSource audioSource = audioSourceObj.AddComponent<AudioSource>();
        // Add audiosource to list
        audioSources.Add(audioSource);
    }
}

public void PlaySound(AudioClip sound,  float volume,bool is3D = false,Vector3 position = new Vector3())
{
    audioSources[currentSourceIndex].clip = sound;
    audioSources[currentSourceIndex].volume = volume;
    if (is3D)
    {
        audioSources[currentSourceIndex].spatialBlend = 1;
        audioSources[currentSourceIndex].transform.position = position;
    }
    else
    {
        audioSources[currentSourceIndex].spatialBlend = 0;
    }
    audioSources[currentSourceIndex].Play();
    currentSourceIndex = (currentSourceIndex + 1) % audioSources.Count;
}
}