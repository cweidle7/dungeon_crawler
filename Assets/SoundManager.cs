using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private AudioSource source;

    void Awake()
    {
        instance = this;
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.4f;
    }

    public void PlayHit()     => PlayTone(880f, 0.05f, 0.3f);
    public void PlayLevelUp() => StartCoroutine(PlayChime());
    public void PlayDeath()   => PlayTone(150f, 0.8f, 0.1f);
    public void PlayBoss()    => PlayTone(120f, 0.5f, 0.15f);
    public void PlayPickup()  => PlayTone(1200f, 0.03f, 0.5f);

    void PlayTone(float frequency, float duration, float volume)
    {
        int sampleRate = 44100;
        int samples = Mathf.RoundToInt(sampleRate * duration);
        AudioClip clip = AudioClip.Create("tone", samples, 1, sampleRate, false);
        float[] data = new float[samples];
        for (int i = 0; i < samples; i++)
        {
            float t = (float)i / sampleRate;
            float envelope = 1f - (t / duration);
            data[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * volume;
        }
        clip.SetData(data, 0);
        source.PlayOneShot(clip);
    }

    System.Collections.IEnumerator PlayChime()
    {
        float[] notes = { 523f, 659f, 784f, 1047f };
        foreach (float note in notes)
        {
            PlayTone(note, 0.15f, 0.4f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
