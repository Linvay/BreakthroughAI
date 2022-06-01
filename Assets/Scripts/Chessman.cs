using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // References to objects in the Unity Scene
    public GameObject controller;
    public GameObject movePlate;

    // Positions for this chess piece on the board
    private int xBoard = -1;
    private int yBoard = -1;
    // The position bias to match the board on scene
    private float xBias = -1.5f;
    private float yBias = -3.5f;

    // Variable to keep track of black/white player
    private string player;

    // References for sprites
    public Sprite black_pawn, white_pawn;

    public void Activate()
    {
        // Get the game controller
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Take the instantiated loaction and adjust the transform
        SetCoords();

        // Choose the correct sprite for the piece
        switch (this.name)
        {
            case "black_pawn": 
                this.GetComponent<SpriteRenderer>().sprite = black_pawn; 
                player = "black";
                break;
            case "white_pawn": 
                this.GetComponent<SpriteRenderer>().sprite = white_pawn; 
                player = "white";
                break;
        }
    }

    // Generate a piece on scene according to the value of xBoard/yBoard
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x += xBias;
        y += yBias;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    // Generate the move plates
    private void OnMouseUp()
    {
        if (controller.GetComponent<Game>().isGameOn)
        {
            if (controller.GetComponent<Game>().GetCurrentPlayer() == "white" && controller.GetComponent<Game>().playMode != Game.PlayMode.BlackCPU)
                return;
            else if (controller.GetComponent<Game>().GetCurrentPlayer() == "black" && controller.GetComponent<Game>().playMode != Game.PlayMode.WhiteCPU)
                return;
            else if (controller.GetComponent<Game>().playMode == Game.PlayMode.CPUvsCPU)
                return;

            if (!controller.GetComponent<Game>().isGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == this.player)
            {
                DestroyMovePlates();
                InitiateMovePlates();
            }
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_pawn":
                PawnMovePlate(xBoard + 1, yBoard -1);
                PawnMovePlate(xBoard, yBoard - 1);
                PawnMovePlate(xBoard - 1, yBoard - 1);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard + 1, yBoard + 1);
                PawnMovePlate(xBoard, yBoard + 1);
                PawnMovePlate(xBoard - 1, yBoard + 1);
                break;
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x, y) 
                && sc.GetPosition(x, y) != null 
                && sc.GetPosition(x, y).GetComponent<Chessman>().player != player 
                && xBoard != x)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        // Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        x += xBias;
        y += yBias;

        // Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        // Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        x += xBias;
        y += yBias;

        // Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
