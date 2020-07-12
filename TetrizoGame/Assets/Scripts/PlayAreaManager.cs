using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaManager : MonoBehaviour {

    private static int height = 20;
    private static int width = 10;

    private Spawner spawner;

    private Transform[,] grid = new Transform[width, height];
    private bool debugEnabled = false;

    private void Start() {
        spawner = (Spawner)FindObjectOfType<Spawner>();
    }


    public bool isValidGridPos(Group group) {
        foreach (Transform child in group.transform) {

            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            // Not inside play area?
            if (!insidePlayArea(roundedX, roundedY)) {
                return false;
            }

            Transform existingOccupant = grid[roundedX, roundedY];

            // Block in grid cell (and not part of same group)
            if (existingOccupant != null && existingOccupant.parent != group.transform) { 
                return false;
            }
        }

        return true;
    }


    // Same as isValidGridPos, but with the y element of each block reduced by one
    public bool isOneDownValidGridPos(Group group) {
        foreach (Transform child in group.transform) {

            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            //TODO
            roundedY--;

            // Not inside play area?
            if (!insidePlayArea(roundedX, roundedY)) {
                return false;
            }

            Transform existingOccupant = grid[roundedX, roundedY];

            // Block in grid cell (and not part of same group)
            if (existingOccupant != null && existingOccupant.parent != group.transform) {
                return false;
            }
        }

        return true;
    }


    bool insidePlayArea(int x, int y) {
        return x >= 0 && x < width
            && y >= 0 && y < height;
    }


    public void updateGridPosition(Group group) {
        foreach (Transform child in group.transform) {
            int roundedX = Mathf.RoundToInt(child.transform.position.x);
            int roundedY = Mathf.RoundToInt(child.transform.position.y);

            grid[roundedX, roundedY] = child;
        }
    }


    public void bottomedOut(Group group) {
        updateGridPosition(group);
        HandleFullLines();
        spawner.SpawnNext();
    }

    private void HandleFullLines() {

        int fullLineCount = 0;

        for (int y = height - 1; y >= 0; y--) {
            if (IsLineFull(y)) {
                debug("Full line y:" + y);
                fullLineCount++;

                DeleteLine(y);
                MoveLinesAboveDown(y);
            }
        }

        if (fullLineCount != 0) {
            GameManager.Instance.shakeScreen();
            playLinesClearedNoise();
            GameManager.Instance.LinesCleared(fullLineCount);
        }
    }

    private void MoveLinesAboveDown(int moveLinesAboveY) {
        for (int y = moveLinesAboveY; y < height; y++) {
            debug("Moving down line: " + y);
            for (int x=0; x < width; x++) {
                if(grid[x, y] != null) {
                    // Move existing element down one on the screen
                    grid[x, y].transform.position += Vector3.down;

                    //Move the grid content down one
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;


                }
            }
        }
    }


    private bool IsLineFull(int y) {
        //Loop through row, if any element is empty, then row not full
        for(int x=0; x < width; x++) {
            if(grid[x, y] == null) {
                return false;
            }
        }

        //If we've got this far, then row was full
        return true;
    }


    public void DeleteLine(int y) {
        debug("Deleting line: " + y);
        for (int x = 0; x < width; x++) {
            // Remove the blocks
            Destroy(grid[x, y].gameObject);

            // Purge the grid
            grid[x, y] = null;
        }
    }


    private void debug(String msg) {
        if (debugEnabled) {
            Debug.Log(msg);
        }
    }


    public void GameOver() {
        GameManager.Instance.GameOver();
    }


    //Set in Unity editor
    public AudioClip landedClip;
    public AudioClip lineClearedClip;


    public void playLandedNoise() {
        if (GameManager.Instance.isMusicPlaying()) {
            AudioSource levelAudio = GameObject.FindGameObjectWithTag("levelAudio").GetComponent<AudioSource>();
            levelAudio.PlayOneShot(landedClip, 1.0f);
        }
    }


    public void playLinesClearedNoise() {
        if (GameManager.Instance.isMusicPlaying()) {
            AudioSource levelAudio = GameObject.FindGameObjectWithTag("levelAudio").GetComponent<AudioSource>();
            levelAudio.PlayOneShot(lineClearedClip, 0.4f);
        }
    }

}
