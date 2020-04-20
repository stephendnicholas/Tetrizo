using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Group : MonoBehaviour {

    private bool relativeControls = true;

    // 0 = original, 1 = 1 rotation, 2 = two rotations, 3 = 3 rotations. Should loop back to 0 after this point
    private int orientation = 0;

    enum Direction {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in transform) {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            renderer.color = new Color(255, 0, 0);
        }

        //TODO - reset camera to same position as this block
    }

    // Update is called once per frame
    void Update() {

        Direction directionToMove = Direction.NONE;

        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Debug.Log("Left Pressed");

            if(relativeControls) {
                switch (orientation) {
                    case 0:
                        directionToMove = Direction.LEFT;
                        break;
                    case 1:
                        directionToMove = Direction.UP;
                        break;
                    case 2:
                        directionToMove = Direction.RIGHT;
                        break;
                    case 3:
                        directionToMove = Direction.DOWN;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
            else {
                directionToMove = Direction.LEFT;
            }
        }
        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Debug.Log("Right Pressed");

            if (relativeControls) {
                switch (orientation) {
                    case 0:
                        directionToMove = Direction.RIGHT;
                        break;
                    case 1:
                        directionToMove = Direction.DOWN;
                        break;
                    case 2:
                        directionToMove = Direction.LEFT;
                        break;
                    case 3:
                        directionToMove = Direction.UP;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
            else {
                directionToMove = Direction.RIGHT;
            }
        }
        // Move Down
        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Debug.Log("Down Pressed");

            if (relativeControls) {
                switch (orientation) {
                    case 0:
                        directionToMove = Direction.DOWN;
                        break;
                    case 1:
                        directionToMove = Direction.LEFT;
                        break;
                    case 2:
                        directionToMove = Direction.UP;
                        break;
                    case 3:
                        directionToMove = Direction.RIGHT;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
            else {
                directionToMove = Direction.DOWN;
            }
        }
        //Up = Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Debug.Log("Up Pressed");
            if (relativeControls) {
                switch (orientation) {
                    case 0:
                        directionToMove = Direction.UP;
                        break;
                    case 1:
                        directionToMove = Direction.RIGHT;
                        break;
                    case 2:
                        directionToMove = Direction.DOWN;
                        break;
                    case 3:
                        directionToMove = Direction.LEFT;
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException();
                }
            }
            else {
                directionToMove = Direction.UP;
            }
        }

        if (directionToMove != Direction.NONE) { 
         Debug.Log("Direction to move: " + directionToMove);
        }

        Move(directionToMove);
    }


    private void Move(Direction direction) {
        switch(direction) {
            case Direction.NONE:
                break;
            case Direction.UP:
                MoveUp();
                break;
            case Direction.DOWN:
                MoveDown();
                break;
            case Direction.LEFT:
                MoveLeft();
                break;
            case Direction.RIGHT:
                MoveRight();
                break;
        }
    }

    private void MoveLeft() {
        transform.position += new Vector3(-1, 0, 0);
        Camera.main.transform.position += new Vector3(-1, 0, 0);
    }


    private void MoveRight() {
        transform.position += new Vector3(1, 0, 0);
        Camera.main.transform.position += new Vector3(1, 0, 0);
    }


    //AKA rotate
    private void MoveUp() {
        Debug.Log("Old orientation: " + orientation);
        orientation++;
        if (orientation == 4) {
            orientation = 0;
        }
        Debug.Log("New orientation: " + orientation);

        transform.Rotate(0, 0, -90);

        Camera.main.transform.Rotate(0, 0, -90);
    }


    private void MoveDown() {
        transform.position += new Vector3(0, -1, 0);
        Camera.main.transform.position += new Vector3(0, -1, 0);
    }

}
