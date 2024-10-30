using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffectData", menuName = "Scriptable Objects/SoundEffectData")]
public class SoundEffectData : ScriptableObject
{
    [SerializeField] List<AudioClip> clips;

    public AudioClip GetAudioClip()
    {
        return clips[Random.Range(0, clips.Count)];
    }
}
