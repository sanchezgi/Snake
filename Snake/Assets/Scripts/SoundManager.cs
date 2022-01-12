using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  static public AudioSource source;

  // Start is called before the first frame update
    void Start()
    {   
      source = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public static void PlaySound(string clip)
  {
    switch (clip)
    {
      case "ButtonClick":
        source.PlayOneShot(GameAssets.Instance.ButtonClick);
        break;
      case "AppleSound":
        source.PlayOneShot(GameAssets.Instance.AppleSound);
        break;
    }
  }
}
