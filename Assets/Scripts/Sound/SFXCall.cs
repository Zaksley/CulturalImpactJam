using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXCall : MonoBehaviour
{
    public AudioSource aus;
    public List<AudioClip> audios = new List<AudioClip>();
    public List<AudioClip> randomAudios = new List<AudioClip>();

    public void CallSFX(int index)
    {
        aus.clip = audios[index];
        aus.Play();
    }

    public void CallRandomSFX(int index)
    {
        index = Random.Range(0, 3);
        aus.clip = audios[index];
        aus.Play();
    }
    public void changeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void changeSceneInt(int i)
    {
        SceneManager.LoadScene(i);
    }
}
