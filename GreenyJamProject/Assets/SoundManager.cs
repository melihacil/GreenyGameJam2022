using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private GameObject[] soundObjects;


    public static SoundManager instance;




    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        //soundSources = new AudioSource[soundLength];
    }

    public void StopPlaying()
    {
        FindObjectOfType<AudioSource>().Stop();
    }

    public void PlaySource(int index)
    {
        GameObject newSoundObject = new GameObject("Sound");
        AudioSource audioSource = newSoundObject.AddComponent<AudioSource>();
        audioSource.volume = 0.2f;
        audioSource.clip = soundObjects[index].GetComponent<AudioSource>().clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySource(string name)
    {
        Array.Find(soundObjects, x => x.GetComponent<AudioSource>().name == name).GetComponent<AudioSource>().Play();
    }
}


