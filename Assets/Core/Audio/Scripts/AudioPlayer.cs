using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;
    [SerializeField] AudioSource audioSource;
    private void Awake()
    {
        Instance = this;    
    }

    public void PlayAudio(SoundEffectData data)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(data.GetAudioClip());
    }
}
