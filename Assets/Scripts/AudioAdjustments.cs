// this is where we'd put music overrides if we wanted
using UnityEngine;

public class AudioAdjustments : MonoBehaviour
{
    [SerializeField] bool MuteSound = false;

    GameObject AudioObject;
 
    void Start()
    {
        AudioObject = FindObjectOfType<AudioManager>().gameObject;
        if (MuteSound) AudioObject.GetComponent<AudioSource>().mute = true;
    }

    private void OnDestroy()
    {
        if (AudioObject == null) Debug.LogWarning("AudioAdjustments: AudioObject is null!");
        else AudioObject.GetComponent<AudioSource>().mute = false;
    }
}
