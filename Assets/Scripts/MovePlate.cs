using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    // Board positions
    int matrixX;
    int matrixY;

    // false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            // change sprite to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);
            if (cp.name == "black_pawn")
            {
                controller.GetComponent<Game>().DecreasePieceNumBlack();
            }
            else
            {
                controller.GetComponent<Game>().DecreasePieceNumWhite();
            }
            Destroy(cp);
        }

        int sourceCol = reference.GetComponent<Chessman>().GetXBoard();
        int sourceRow = reference.GetComponent<Chessman>().GetYBoard();

        // Set the chesspiece's original location to empty
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),
                                                         reference.GetComponent<Chessman>().GetYBoard());
        // Move Piece in engine too
        controller.GetComponent<Game>().MovePieceInEngine(sourceCol, sourceRow, matrixX, matrixY);

        // Move reference chess piece to this position
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        // Update the matrix
        controller.GetComponent<Game>().SetPosition(reference);

        reference.GetComponent<Chessman>().DestroyMovePlates();

        // check if all the chesspieces have been captured
        if (controller.GetComponent<Game>().GetPieceNumBlack() == 0)
        {
            controller.GetComponent<Game>().Winner("white");
        }

        if (controller.GetComponent<Game>().GetPieceNumWhite() == 0)
        {
            controller.GetComponent<Game>().Winner("black");
        }

        // Check if chesspiece reaches the end of the row
        if (reference.GetComponent<Chessman>().name == "white_pawn" && matrixY == 7)
        {
            controller.GetComponent<Game>().Winner("white");
        }

        if (reference.GetComponent<Chessman>().name == "black_pawn" && matrixY == 0)
        {
            controller.GetComponent<Game>().Winner("black");
        }

        controller.GetComponent<Game>().NextTurn();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
