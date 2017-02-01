using UnityEngine;
using System.Collections;

public class GameAudioManager : MonoBehaviour {
    public AudioClip[] soundtracks;
    public AudioSource audioSourceOne;
    public AudioSource audioSourceTwo;

    public float fadeOutPercentage;
    public float fadeOutStep;

    private AudioClip currentSoundtrack;

    private bool FadeOutOne;
    private bool FadeOutTwo;

    private void Start() {
        FadeOutOne = true;
        FadeOutTwo = true;
        currentSoundtrack = soundtracks[Random.Range(0, soundtracks.Length)];
        audioSourceOne.clip = currentSoundtrack;
        audioSourceOne.Play();
    }

    private void FixedUpdate() {
        if (audioSourceOne.time > (currentSoundtrack.length/100)* fadeOutPercentage && FadeOutOne) {
            currentSoundtrack = soundtracks[Random.Range(0, soundtracks.Length)];
            FadeOutOne = false;
            StartCoroutine(SourceOneFadeOut());
            StartCoroutine(SourceTwoFadeIn());
        }

        if (audioSourceTwo.time > (currentSoundtrack.length / 100)* fadeOutPercentage && FadeOutTwo)
        {
            currentSoundtrack = soundtracks[Random.Range(0, soundtracks.Length)];
            FadeOutTwo = false;
            StartCoroutine(SourceTwoFadeOut());
            StartCoroutine(SourceOneFadeIn());
        }
    }

    private IEnumerator SourceOneFadeIn()
    {
        audioSourceOne.clip = currentSoundtrack;
        audioSourceOne.Play();

        for (float i = 0; audioSourceOne.volume < 0.75f; i += 0.1f)
        {
            yield return new WaitForSeconds(fadeOutStep);
            audioSourceOne.volume += 0.1f;
        }

        FadeOutOne = true;
        audioSourceOne.volume = 0.75f;
    }

    private IEnumerator SourceOneFadeOut() {
        for (float i = 1; audioSourceOne.volume > 0; i -= 0.1f)
        {
            yield return new WaitForSeconds(fadeOutStep);
            audioSourceOne.volume -= 0.1f;
        }

        audioSourceOne.volume = 0;
        audioSourceOne.Stop();
    }

    private IEnumerator SourceTwoFadeIn() {
        audioSourceTwo.clip = currentSoundtrack;
        audioSourceTwo.Play();

        for (float i = 0; audioSourceTwo.volume < 0.75f; i += 0.1f)
        {
            yield return new WaitForSeconds(fadeOutStep);
            audioSourceTwo.volume += 0.1f;
        }

        FadeOutTwo = true;
        audioSourceTwo.volume = 0.75f;
    }

    private IEnumerator SourceTwoFadeOut()
    {
        for (float i = 1; audioSourceTwo.volume > 0; i -= 0.1f)
        {
            yield return new WaitForSeconds(fadeOutStep);
            audioSourceTwo.volume -= 0.1f;
        }

        audioSourceTwo.volume = 0;
        audioSourceTwo.Stop();
    }
}
