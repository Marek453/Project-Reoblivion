using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Announcement
{
    public class AnnouncementSystem : MonoBehaviour
    {
        [Header("Settings")]
        public float baseDelay = 0.1f;

        [Header("Library")]
        public string resourcesPath = "VOX";

        private AudioSource audioSource;
        private Dictionary<string, AudioClip> wordLibrary = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            LoadAllWords();
        }

        private void Start()
        {
            Speak("mst unit november 7 has entered the facility mstannounce norpcs");
        }

        private void LoadAllWords()
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>(resourcesPath);
            foreach (AudioClip clip in clips)
            {
                if (!wordLibrary.ContainsKey(clip.name.ToLower()))
                {
                    wordLibrary.Add(clip.name.ToLower(), clip);
                }
            }
            Debug.Log($"VOX System loaded {wordLibrary.Count} words.");
        }

        public void Speak(string sentence)
        {
            StartCoroutine(ProcessSentence(sentence));
        }

        private IEnumerator ProcessSentence(string sentence)
        {
            string[] rawWords = sentence.Split(' ');

            foreach (string token in rawWords)
            {
                string cleanToken = token.ToLower().Trim();
                float currentPitch = 1.0f;

                if (cleanToken.StartsWith("pitch:"))
                {
                    string pitchVal = cleanToken.Split(':')[1];
                    if (float.TryParse(pitchVal, out float p))
                    {
                        audioSource.pitch = p;
                    }
                    continue; 
                }

                if (cleanToken == "wait" || cleanToken == "...")
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                if (wordLibrary.ContainsKey(cleanToken))
                {
                    AudioClip clip = wordLibrary[cleanToken];

                    audioSource.PlayOneShot(clip);

                    yield return new WaitForSeconds((clip.length / audioSource.pitch) + baseDelay);
                }
                else
                {
                    Debug.LogWarning($"VOX Word not found: {cleanToken}");
                }
            }

            audioSource.pitch = 1.0f;
        }
    }
}
