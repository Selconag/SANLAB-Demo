using UnityEngine;
public enum SoundEffects { Success, Failure, Connect, WrongConnect, LevelComplete }
public class AudioManager : Singleton<AudioManager>
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip failureSound;
    [SerializeField] private AudioClip connectSound;
    [SerializeField] private AudioClip wrongConnectSound;
    [SerializeField] private AudioClip levelCompleteSound;

    public void PlaySoundEffect(SoundEffects soundType)
    {
        audioSource.clip = soundType switch
        {
            SoundEffects.Success => successSound,
            SoundEffects.Failure => failureSound,
            SoundEffects.Connect => connectSound,
            SoundEffects.WrongConnect => wrongConnectSound,
            SoundEffects.LevelComplete => levelCompleteSound,
            _ => throw new System.NotImplementedException(),
        };

        audioSource.Play();
    }
}
