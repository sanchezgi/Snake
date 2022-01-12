using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{

  public static GameAssets Instance;

  public Sprite SnakeHeadSprite;
  public Sprite FoodSprite;
  public Sprite BodySprite;
  public Sprite BodyBottomLeft;
  public Sprite BodyBottomRight;
  public Sprite BodyTopLeft;
  public Sprite BodyTopRight;

  public AudioClip ButtonClick;
  public AudioClip AppleSound;

  private void Awake()
  {
    Instance = this;
  }
}
