using UnityEngine;

public class TestAudio : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        var a = gameObject.AddComponent<AudioSource>();
        a.clip = clip;
        a.Play();
    }
}
