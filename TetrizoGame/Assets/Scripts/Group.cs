using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class Group : MonoBehaviour {

    public Vector3 rotationPoint;

    private PlayAreaManager playAreaManager;
    private Spawner spawner;

    private Text upLabel;
    private Text rightLabel;
    private Text downLabel;
    private Text leftLabel;

    private String[] directonLabels = new string[] {
        "Rotate\nBlock",  // With rotationOffset 0 - this is the up arrow and direction
        "Block\nRight",   // With rotationOffset 0 - this is the right arrow and direction
        "Block\nDown",    // With rotationOffset 0 - this is the down arrow and direction
        "Block\nLeft",    // With rotationOffset 0 - this is the left arrow and direction
    };

    private int rotationOffset = 0; // 0 = standard, 1 = 90 degress, 2 = 180, 3 = 270

    // Based on selected difficulty
    private bool relativeControls = false;
    

    // 0 = original, 1 = 1 rotation, 2 = two rotations, 3 = 3 rotations. Should loop back to 0 after this point
    private int orientation = 0;

    private static float intitialPerFallInternal = 0.8f;
    private float currentPerFallInterval = intitialPerFallInternal;
    private float lastFallTime = 0.0f;

    // For debug
    private bool debugEnabled;
    private bool rotationEnabled = true; //TODO - use this

    enum Direction {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    // Start is called before the first frame update
    void Start() {
        relativeControls = GameManager.Instance.getCurrentDifficulty() == GameManager.Difficulty.HARD;


        //Reset camera to same position and orientation as this block
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.rotation = Quaternion.identity;

        playAreaManager = (PlayAreaManager) FindObjectOfType<PlayAreaManager>();

        // If not valid when spawned, then Game Over
        if(!playAreaManager.isValidGridPos(this)) {
            Destroy(gameObject);
            playAreaManager.GameOver();
        }

        // Find the direction labels on screen
        upLabel = GameObject.FindGameObjectWithTag("upLabel").GetComponent<Text>();
        rightLabel = GameObject.FindGameObjectWithTag("rightLabel").GetComponent<Text>();
        downLabel = GameObject.FindGameObjectWithTag("downLabel").GetComponent<Text>();
        leftLabel = GameObject.FindGameObjectWithTag("leftLabel").GetComponent<Text>();

        updateDirectionLabels();
    }

    // Update is called once per frame
    void Update() {

        // Ignore all if currently paused
        if(GameManager.Instance.isPaused()) {
            return;
        }

        // Re-calculate currentPerFallInterval in case the level has increased TODO - confirm the calculation to use
        currentPerFallInterval = intitialPerFallInternal - (GameManager.Instance.getCurrentLevel() * 0.05f);

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

        if (Time.time - lastFallTime > currentPerFallInterval) {
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

        //Update the direction labels if relative controls
        if(relativeControls) {
            increaseRotationOffset();
            updateDirectionLabels();
        }

        transform.Rotate(0, 0, -90);

        if (playAreaManager.isValidGridPos(this)) {
            if (rotationEnabled) { 
                Camera.main.transform.Rotate(0, 0, -90);
            }
        }
        else {
            transform.Rotate(0, 0, 90);
        }
    }


    // Handles incremeting the rotation offset, and the cycling around 0, 1, 2, 3
    private void increaseRotationOffset() {
        rotationOffset++;

        if(rotationOffset == 4) {
            rotationOffset = 0;
        }
    }


    private void updateDirectionLabels() {
        upLabel.text = directonLabels[rotationOffset % 4];       // if R0, then 0, if R1, then 1, if R2, then 2, if R3, then 3
        rightLabel.text = directonLabels[(rotationOffset + 1) % 4]; // if R0, then 1, if R1, then 2, if R2, then 3, if R3, then 0
        downLabel.text = directonLabels[(rotationOffset + 2) % 4]; // if R0, then 2, if R1, then 3, if R2, then 0, if R3, then 1
        leftLabel.text = directonLabels[(rotationOffset + 3) % 4]; // if R0, then 3, if R1, then 0, if R2, then 1, if R3, then 2
    }


    private void debug(String msg) {
        if(debugEnabled) {
            Debug.Log(msg);
        }
    }
}
