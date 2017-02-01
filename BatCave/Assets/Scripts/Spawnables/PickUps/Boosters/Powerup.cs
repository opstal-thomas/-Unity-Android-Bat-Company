using UnityEngine;

abstract public class Powerup : MonoBehaviour {

    public AudioClip[] sounds;
    private AudioSource audioSource;

    protected void BaseStart() {
        audioSource = GetComponent<AudioSource>();
    }

    protected void PlayRandomSound() {
        audioSource.clip = sounds[Random.Range(0, sounds.Length - 1)];
        audioSource.Play();
    }
}
