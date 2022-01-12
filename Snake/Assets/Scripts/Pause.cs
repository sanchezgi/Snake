using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
  private Button ResumeButton; 
  private Button MenuButton; 

  private void Awake()
  {
    ResumeButton = transform.Find("Resume").GetComponent<Button>();
    MenuButton = transform.Find("Back").GetComponent<Button>();
  }

  // Start is called before the first frame update
  void Start()
    {
     ResumeButton.gameObject.SetActive(false);
     MenuButton.gameObject.SetActive(false);
    }

    public void Resume()
    {
       ResumeButton.gameObject.SetActive(false);
    MenuButton.gameObject.SetActive(false);
    Time.timeScale = 1.0f;
    }

  public void BackToMenu()
  {
    SceneManager.LoadScene("MainScene 1");
    Time.timeScale = 1.0f;
  }
}
