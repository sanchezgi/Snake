using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Snake : MonoBehaviour
{
  private enum EDirection
  {
    Left,
    Right,
    Up,
    Down
  }
  
  private Vector2Int GridPosition;
  private EDirection Direction;
  private Vector2 Fwd;
  private Quaternion Rotation;
  private float GridMoveRate;
  public float GridMoveRateMax;
  public float RaycastLength;
  private Board board;
  private int SnakeSize;
  private bool Alive;
  private List<SnakeMovePosition> SnakeList;
  private List<BodyPart> SnakeBodyList;
  private bool FoodEaten;
  public Button ResumeButton;
  public Button BackButton;
  public ParticleSystem Explosion;


  public void Setup(Board board)
  {
    this.board = board;
  }

  private void Awake()
  {
    GridPosition = new Vector2Int(2, 10);
    GridMoveRate = GridMoveRateMax;
    Direction = EDirection.Right; // Righ direction as default
    Fwd = Vector2.right;
    Rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
    SnakeSize = 0;
    SnakeList = new List<SnakeMovePosition>();
    SnakeBodyList= new List<BodyPart>();
    FoodEaten = false;
    Alive = true;
  }

  private void Update()
  {
    if (Alive)
    {
      HandleInput();
      CheckCollision();
      Move();
    }
  }

  private void HandleInput()
  {
    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
    {
      if (Direction != EDirection.Down) // Prevent snake going on opposite direction
      {
        Direction = EDirection.Up;
        Fwd = Vector2.up;
        Rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      }
    }

    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
    {
      if (Direction != EDirection.Up)
      {
        Direction = EDirection.Down;  
        Fwd = Vector2.down;
        Rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
      }
    }

    if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
    {
      if (Direction != EDirection.Left)
      {
        Direction = EDirection.Right;
        Fwd = Vector2.right;
        Rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
      }
    }

    if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
    {
      if (Direction != EDirection.Right)
      {
        Direction = EDirection.Left;
        Fwd = Vector2.left;
        Rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
      }
    }

    if (Input.GetKeyDown(KeyCode.P))
    {
      ResumeButton.gameObject.SetActive(true);
      BackButton.gameObject.SetActive(true);
      Time.timeScale = 0.0f;
    }
  }

  private void CheckCollision()
  {
    RaycastHit2D Hit = Physics2D.Raycast(transform.position, Fwd * RaycastLength, RaycastLength);

    if (Hit.collider)
    {
      if (Hit.transform.gameObject.CompareTag("Body"))
      {     
        Alive = false;
        SceneManager.LoadScene("GameOver");
      }

      if (Hit.transform.gameObject.CompareTag("Food"))
      {
        if (Alive)
        {
          board.FoodEaten();
          GameHandler.AddScore();
          SoundManager.PlaySound("AppleSound");
          StartExplosion();
          SnakeSize++;
          FoodEaten = true;
        }     
      }
    }
  }

  private void Move()
  {
    GridMoveRate += Time.deltaTime;

    if (GridMoveRate >= GridMoveRateMax)
    {
      SnakeMovePosition prevSnakePos = null;
      if (SnakeList.Count > 0)
      {
        prevSnakePos = SnakeList[0];
      }
      SnakeMovePosition SnakeMovePos = new SnakeMovePosition(prevSnakePos,GridPosition, Direction);
      SnakeList.Insert(0, SnakeMovePos);

      Vector2Int DirVector = new Vector2Int();

      switch (Direction)
      {
        case EDirection.Left:
          DirVector = new Vector2Int(-1, 0);
          break;
        case EDirection.Right:
          DirVector = new Vector2Int(1, 0);
          break;
        case EDirection.Up:
          DirVector = new Vector2Int(0, 1);
          break;
        case EDirection.Down:
          DirVector = new Vector2Int(0, -1);
          break;
        default:
          break;
      }

      GridPosition += DirVector;

      GridPosition = board.CheckPosition(GridPosition);

      if (FoodEaten)
      {
        CreateBody();
      }

      if (SnakeList.Count >= SnakeSize + 1)
      {
        SnakeList.RemoveAt(SnakeList.Count - 1);
      }
    
      GridMoveRate -= GridMoveRateMax;

      transform.position = new Vector3(GridPosition.x, GridPosition.y, 0.0f);
      transform.rotation = Rotation;

      UpdateBody();
    }
  }

  private void CreateBody()
  {
    SnakeBodyList.Add(new BodyPart());
    FoodEaten = false;
  }

  private void UpdateBody()
  {
    for (int i = 0; i < SnakeBodyList.Count; i++)
    {
      SnakeBodyList[i].SetPositionAndDirection(SnakeList[i]);
    }
  }
  public Vector2Int GetGridPosition()
  {
    return GridPosition;
  }

  public List<Vector2Int> GetListpositions()
  {
    List<Vector2Int> FullSnake = new List<Vector2Int>() { GridPosition }; // Head

    foreach (SnakeMovePosition item in SnakeList)
    {
      FullSnake.Add(item.GetPosition()); // Head + Body
    }

    return FullSnake;
  }


  private class BodyPart
  {
    private Transform transform;

    public BodyPart()
    {
      GameObject BodyGameObject = new GameObject("Body", typeof(SpriteRenderer), typeof(BoxCollider2D));
      BodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.BodySprite;
      BodyGameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.0f,1.0f);
      BodyGameObject.GetComponent<BoxCollider2D>().isTrigger = true;
      BodyGameObject.gameObject.tag = "Body";
      transform = BodyGameObject.transform;
    }

    public void SetPositionAndDirection(SnakeMovePosition move)
    {
      transform.position = new Vector3(move.GetPosition().x, move.GetPosition().y, 0.0f);

      SpriteRenderer SP = transform.gameObject.GetComponent<SpriteRenderer>();

      float angle = 0.0f;
      switch (move.GetDirection())
      {
        case EDirection.Left:
          switch (move.GetPreviousDirection())
          {
            case EDirection.Down:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyTopLeft;
              break;
            case EDirection.Up:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyBottomLeft;
              break;
            default:
              angle = -90.0f;
              SP.sprite = GameAssets.Instance.BodySprite;
              break;
          }
          break;
        case EDirection.Right:
          switch (move.GetPreviousDirection())
          {
            case EDirection.Down:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyTopRight;
              break;
            case EDirection.Up:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyBottomRight;
              break;
            default:
              angle = 90.0f;
              SP.sprite = GameAssets.Instance.BodySprite;
              break;
          }
          break;
        case EDirection.Up:
          switch (move.GetPreviousDirection())
          {
            case EDirection.Left:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyTopRight;
              break;
            case EDirection.Right:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyTopLeft;
              break;
            default:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodySprite;
              break;
          }
          break;
        case EDirection.Down:
          switch (move.GetPreviousDirection())
          {
            case EDirection.Left:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyBottomRight;
              break;
            case EDirection.Right:
              angle = 0.0f;
              SP.sprite = GameAssets.Instance.BodyBottomLeft;
              break;
            default:
              angle = 180.0f;
              SP.sprite = GameAssets.Instance.BodySprite;
              break;
          }
          break;
        default:
          break;
      }
      transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
    }
  }

  private class SnakeMovePosition
  {
    private SnakeMovePosition previousPosition;
    private Vector2Int Position;
    private EDirection Direction;

    public SnakeMovePosition(SnakeMovePosition prevPos,Vector2Int pos, EDirection dir)
    {
      Position = pos;
      Direction = dir;
      previousPosition = prevPos;
    }

    public Vector2Int GetPosition()
    {
      return Position;
    }

    public EDirection GetDirection()
    {
      return Direction;
    }

    public EDirection GetPreviousDirection()
    {
      if (previousPosition == null)
      {
        return EDirection.Right;
      }
      else
      {
        return previousPosition.Direction;
      }   
    }
  }

  public void StartExplosion()
  {
    Explosion.Play();
  }
}
