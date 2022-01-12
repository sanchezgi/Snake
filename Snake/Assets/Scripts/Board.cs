using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board 
{
  private Vector2Int FoodPosition;
  private GameObject FoodGameObject;
  private Snake snake;
  public int Width;
  public int Height;

  public Board(int width, int height)
  {
    Width = width;
    Height = height;
  }

  public void Setup(Snake snake)
  {
    this.snake = snake;
     SpawnFood();
  }


  private void SpawnFood()
  {
    do
    {
      FoodPosition = new Vector2Int(Random.Range(0, Width), Random.Range(0, Height));
    } while (snake.GetListpositions().IndexOf(FoodPosition) != -1);
   

    FoodGameObject = new GameObject("Food", typeof(SpriteRenderer),typeof(BoxCollider2D));
    FoodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.FoodSprite;
    FoodGameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.0f, 1.0f);
    FoodGameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    FoodGameObject.gameObject.tag = "Food";

    FoodGameObject.transform.position = new Vector3(FoodPosition.x, FoodPosition.y, 0.0f);
  }

  public void FoodEaten()
  {
    Object.Destroy(FoodGameObject);
    SpawnFood();
  }

  public Vector2Int CheckPosition(Vector2Int Pos)
  {
    if (Pos.x < 0)
    {
      Pos.x = Width - 1;
    }

    if (Pos.x > Width - 1)
    {
      Pos.x = 0;
    }

    if (Pos.y < 0)
    {
      Pos.y = Height - 1;
    }

    if (Pos.y > Height - 1)
    {
      Pos.y = 0;
    }

    return Pos;
  }
}
