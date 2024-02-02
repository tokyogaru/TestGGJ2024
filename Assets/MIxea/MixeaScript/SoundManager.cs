using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource musicObject, sfxObject;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    public void PlaySound(AudioClip clip, Transform spawn, float volume, float maxDist)
    {
        //Spawn gamobject
        AudioSource audioSrc = Instantiate(sfxObject, spawn.position, Quaternion.identity);

        //Assign audio clip
        audioSrc.clip = clip;
        audioSrc.volume = volume;
        //If its 3D
        if(maxDist != 0)
        {
            audioSrc.spatialBlend = 1;
            audioSrc.maxDistance = maxDist;
        }


        audioSrc.Play();

        //End after lenght of clip
        float clipLength = audioSrc.clip.length;
        Destroy(audioSrc.gameObject, clipLength);
    }
}
