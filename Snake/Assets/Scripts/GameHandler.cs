using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
  private static GameHandler Instance;
  private static int Score;

  [SerializeField] private Snake snake;

  private Board board;

  private void Awake()
  {
    Instance = this;
    InitStats();
  }

  // Start is called before the first frame update
  void Start()
    {
    InitStats();
    board = new Board(20,20);

    if (snake != null && board != null)
    {
      snake.Setup(board);
      board.Setup(snake);
    }
     
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  private static void InitStats()
  {
    Score = 0;
  }


  public static int GetScore()
  {
    return Score;
  }

  public static void AddScore()
  {
    Score += 100;
  }
}
