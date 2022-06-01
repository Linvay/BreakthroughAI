using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Engine;
using System.Threading;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;
    public PlayMode playMode;
    public int currentMode = 0;
    public Text turnText;

    // Positions and team for each chesspiece
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    // The remaining number of pieces for each player
    private int pieceNumBlack = 16;
    private int pieceNumWhite = 16;

    public string currentPlayer = "white";

    public bool isGameOn = false;
    private bool gameOver = false;

    // Game Engine
    internal GameEngine engine;

    private WaitCallback AIDoMove;
    private bool isMoveGenComplete;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    void Start()
    {
        playMode = PlayMode.BlackCPU;
        turnText.text = "White's turn";

        playerWhite = new GameObject[]
        {
            Create("white_pawn", 0, 0), Create("white_pawn", 1, 0), Create("white_pawn", 2, 0), Create("white_pawn", 3, 0),
            Create("white_pawn", 4, 0), Create("white_pawn", 5, 0), Create("white_pawn", 6, 0), Create("white_pawn", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1), Create("white_pawn", 3, 1),
            Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1), Create("white_pawn", 7, 1),
        };

        playerBlack = new GameObject[]
        {
            Create("black_pawn", 0, 7), Create("black_pawn", 1, 7), Create("black_pawn", 2, 7), Create("black_pawn", 3, 7),
            Create("black_pawn", 4, 7), Create("black_pawn", 5, 7), Create("black_pawn", 6, 7), Create("black_pawn", 7, 7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6),
            Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6), Create("black_pawn", 7, 6),
        };

        // Set all the piece positions on the position board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }

        engine = new GameEngine();
        // engine.DoRender = true;
        AIDoMove = new WaitCallback(EngineMovePiece);
        currentPlayer = "white";

        isMoveGenComplete = false;
        gameObject.GetComponent<MoveLog>().ClearLog();
    }

    private void Init()
    {
        turnText.text = "White's turn";

        foreach (GameObject obj in positions)
        {
            Destroy(obj);
        }

        playerWhite = new GameObject[]
        {
            Create("white_pawn", 0, 0), Create("white_pawn", 1, 0), Create("white_pawn", 2, 0), Create("white_pawn", 3, 0),
            Create("white_pawn", 4, 0), Create("white_pawn", 5, 0), Create("white_pawn", 6, 0), Create("white_pawn", 7, 0),
            Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1), Create("white_pawn", 3, 1),
            Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1), Create("white_pawn", 7, 1),
        };

        playerBlack = new GameObject[]
        {
            Create("black_pawn", 0, 7), Create("black_pawn", 1, 7), Create("black_pawn", 2, 7), Create("black_pawn", 3, 7),
            Create("black_pawn", 4, 7), Create("black_pawn", 5, 7), Create("black_pawn", 6, 7), Create("black_pawn", 7, 7),
            Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6),
            Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6), Create("black_pawn", 7, 6),
        };

        // Set all the piece positions on the position board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }

        pieceNumBlack = 16;
        pieceNumWhite = 16;

        engine = new GameEngine();
        AIDoMove = new WaitCallback(EngineMovePiece);
        currentPlayer = "white";

        isMoveGenComplete = false;
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();

        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 
            || y < 0 
            || x >= positions.GetLength(0) 
            || y >= positions.GetLength(1))
            return false;
        return true;
    }

    public int GetPieceNumBlack()
    {
        return pieceNumBlack;
    }

    public int GetPieceNumWhite()
    {
        return pieceNumWhite;
    }

    public void DecreasePieceNumBlack()
    {
        pieceNumBlack -= 1;
    }

    public void DecreasePieceNumWhite()
    {
        pieceNumWhite -= 1;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool isGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            turnText.text = "Black's turn";
        }
        else
        {
            currentPlayer = "white";
            turnText.text = "White's turn";
        }

        if ((playMode == PlayMode.WhiteCPU && currentPlayer == "white")
            || (playMode == PlayMode.BlackCPU && currentPlayer == "black")
            || playMode == PlayMode.CPUvsCPU)
        {
            AIDoMove = new WaitCallback(EngineMovePiece);
            ThreadPool.QueueUserWorkItem(AIDoMove);
        }
    }

    public void Update()
    {
        if (isGameOn)
        {
            turnText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            if (engine.BlackWins || engine.WhiteWins)
            {
                gameOver = true;
                if (engine.BlackWins)
                {
                    Debug.Log("Game Over, Black Wins!");
                    turnText.text = "Black wins!";
                    gameObject.GetComponent<MoveLog>().SendMessageToLog("Black wins");
                }
                else if (engine.WhiteWins)
                {
                    Debug.Log("Game Over, White Wins!");
                    turnText.text = "White wins!";
                    gameObject.GetComponent<MoveLog>().SendMessageToLog("White wins");
                }
            }

            if (gameOver == true)
            {
                isGameOn = false;
                Time.timeScale = 0f;
            } 

            if (isMoveGenComplete)
            {
                var LastMove = engine.MoveHistory.Peek();
                Debug.Log(LastMove);
                gameObject.GetComponent<MoveLog>().SendMessageToLog(LastMove.ToString());
                MovePieceInUI(LastMove.SourceColumn, LastMove.SourceRow, LastMove.DestinationColumn, LastMove.DestinationRow);
                isMoveGenComplete = false;
                if (gameOver == false)
                    NextTurn();
            }
        }
        else
        {
            turnText.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
        }
    }

    private void EngineMovePiece(object state)
    {
        engine.DoMove();
        isMoveGenComplete = true;
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        Debug.Log("The winner is " + playerWinner);
        gameObject.GetComponent<MoveLog>().SendMessageToLog(string.Format("The winner is {0}", playerWinner));
    }


    public void MovePieceInUI(int SourceColumn, int SourceRow, int DesitnationColumn, int DesitnationRow)
    {
        GameObject chesspiece = positions[SourceColumn, SourceRow];
        Chessman cm = positions[SourceColumn, SourceRow].GetComponent<Chessman>();

        if (positions[DesitnationColumn, DesitnationRow] != null)
        {
            GameObject cp = positions[DesitnationColumn, DesitnationRow];
            if (cp.name == "black_pawn")
            {
                DecreasePieceNumBlack();
            }
            else
            {
                DecreasePieceNumWhite();
            }
            Destroy(cp);
        }

        SetPositionEmpty(SourceColumn, SourceRow);

        cm.SetXBoard(DesitnationColumn);
        cm.SetYBoard(DesitnationRow);
        cm.SetCoords();
        SetPosition(chesspiece);
    }

    public void MovePieceInEngine(int SourceColumn, int SourceRow, int DesitnationColumn, int DesitnationRow)
    {
        byte sc = (byte)SourceColumn;
        byte sr = (byte)SourceRow;
        byte dc = (byte)DesitnationColumn;
        byte dr = (byte)DesitnationRow;
        engine.MovePiece(sc, sr, dc, dr);

        var LastMove = engine.MoveHistory.Peek();
        Debug.Log(LastMove);
        gameObject.GetComponent<MoveLog>().SendMessageToLog(LastMove.ToString());
    }

    public void ModeSelected(int val)
    {
        currentMode = val;
    }

    public void StartGame()
    {
        isGameOn = true;
        playMode = PlayMode.BlackCPU + currentMode;

        Time.timeScale = 1f;
        if (gameOver)
        {
            gameOver = false;
        }

        DestroyMovePlates();
        gameObject.GetComponent<MoveLog>().ClearLog();
        Init();

        if (playMode == PlayMode.CPUvsCPU || playMode == PlayMode.WhiteCPU)
        {
            ThreadPool.QueueUserWorkItem(AIDoMove);
        }
    }

    public enum PlayMode
    {
        BlackCPU,
        WhiteCPU,
        CPUvsCPU
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }
}
