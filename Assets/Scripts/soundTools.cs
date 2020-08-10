using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundTools
{
    public class soundTools : MonoBehaviour
    {
        public static soundTools i;
        [SerializeField]
        GameObject soundInstance;
        private void Start()
        {
            i = this;
        }
        public AudioSource SpawnNewSoundInstance(AudioClip s, SoundSettings set)
        {
            GameObject soundObj = Instantiate(soundInstance);
            AudioSource au = soundObj.GetComponent<AudioSource>();
            au.clip = s;
            au.Play();
            au.volume = set.volume;
            Destroy(soundObj, s.length);
            return au;
        }
    }
    public static class soundMethods
    {

    }
    public class SoundSettings
    {
        public float volume;
        public SoundSettings(float _volume = 1)
        {
            volume = _volume;
        }
    }
}