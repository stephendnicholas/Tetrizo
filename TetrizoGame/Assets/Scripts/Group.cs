using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Group : MonoBehaviour {

    public Vector3 rotationPoint;

    private PlayAreaManager playAreaManager;
    private Spawner spawner;

    private bool relativeControls = false;

    // 0 = original, 1 = 1 rotation, 2 = two rotations, 3 = 3 rotations. Should loop back to 0 after this point
    private int orientation = 0;

    private float lastFallTime = 0.0f;
    private float perFallInterval = 0.8f;
    private bool debugEnabled;

    enum Direction {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    // Start is called before the first frame update
    void Start() {
        //Reset camera to same position and orientation as this block
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.rotation = Quaternion.identity;

        playAreaManager = (PlayAreaManager) FindObjectOfType<PlayAreaManager>();

        if(!playAreaManager.isValidGridPos(this)) {
            debug("Game over");
            Destroy(gameObject);
            print("test2");
        }
        
    }

    // Update is called once per frame
    void Update() {

        Direction directionToMove = Direction.NONE;

        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            debug("Left Pressed");

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
            debug("Right Pressed");

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
            debug("Down Pressed");

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
            debug("Up Pressed");
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
         debug("Direction to move: " + directionToMove);
        }

        Move(directionToMove);

        if (Time.time - lastFallTime > perFallInterval) {
            Move(Direction.DOWN);
            lastFallTime = Time.time;
        }
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

        if(playAreaManager.isValidGridPos(this)) {
            Camera.main.transform.position += new Vector3(-1, 0, 0);
        }
        else {
            transform.position -= new Vector3(-1, 0, 0);
        }
    }


    private void MoveRight() {
        transform.position += new Vector3(1, 0, 0);

        if (playAreaManager.isValidGridPos(this)) {
            Camera.main.transform.position += new Vector3(1, 0, 0);
        }
        else {
            transform.position -= new Vector3(1, 0, 0);
        }
    }


    private void MoveDown() {
        transform.position += new Vector3(0, -1, 0);

        if (playAreaManager.isValidGridPos(this)) {
            Camera.main.transform.position += new Vector3(0, -1, 0);
        }
        // We've hit our terminal down state
        else {
            // Move back to the last valid position
            transform.position -= new Vector3(0, -1, 0);

            playAreaManager.bottomedOut(this);            
            enabled = false;
        }
    }


    //AKA rotate
    private void MoveUp() {
        debug("Old orientation: " + orientation);
        orientation++;
        if (orientation == 4) {
            orientation = 0;
        }
        debug("New orientation: " + orientation);

        //TODO
        //transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

        transform.Rotate(0, 0, -90);

        if (playAreaManager.isValidGridPos(this)) {
            //Camera.main.transform.Rotate(0, 0, -90);
        }
        else {
            transform.Rotate(0, 0, 90);
        }
    }


    private void debug(String msg) {
        if(debugEnabled) {
            Debug.Log(msg);
        }
    }
}
