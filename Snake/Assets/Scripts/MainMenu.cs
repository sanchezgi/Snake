using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

  private Button playButton;
  private Button quitButton;

  private void Awake()
  {
    playButton = transform.Find("PlayButton").GetComponent<Button>();
    quitButton = transform.Find("QuitButton").GetComponent<Button>();
  }

  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void Play()
  {
    SceneManager.LoadScene("MainScene"); 
  }

  public void Quit()
  {
    Application.Quit();
  }
}
