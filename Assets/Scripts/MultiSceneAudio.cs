using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MultiSceneAudio : MonoBehaviour
    {
        private AudioSource _source;
        private static AudioSource _currentlyPlaying;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            _source = GetComponent<AudioSource>();

            if (_currentlyPlaying == null)
            {
                _source.Play();
                _currentlyPlaying = _source;
            }
            else if (_currentlyPlaying.clip.name != _source.clip.name)
            {
                _currentlyPlaying.Stop();
                _source.Play();
                _currentlyPlaying = _source;
            }
        }
    }
}
