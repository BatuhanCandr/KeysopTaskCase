using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> sounds = new List<AudioClip>();
    

    public void PlaySound(int index)
    {
        if (index >= 0 && index < sounds.Count)
        {
            audioSource.PlayOneShot(sounds[index]);
        }
        else
        {
            Debug.LogWarning("Invalid Sound : " + index);
        }
    }
}