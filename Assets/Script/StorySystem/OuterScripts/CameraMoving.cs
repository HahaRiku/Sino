using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {
    [System.Serializable]
    public class Coordinate {
        public float x;
        public float y;
    }
    [System.Serializable]
    public struct MovingData {
        public Coordinate destinationPosition;
        public float movingTimeInSeconds;
    }
    public Coordinate[] setPositions;
    public MovingData[] movingData;
    public AnimationCurve[] x_aniCurve;
    public AnimationCurve[] y_aniCurve;

    private bool wiggleStart = false;
    private int wiggleIndex;
    private float wiggleStartTime = 0;

    void Update() {
        if(wiggleStart) {
            wiggleStartTime = Time.time;
        }
    }

    public void SetCameraPosition(int positionIndex) {
        transform.localPosition = new Vector3(setPositions[positionIndex].x, setPositions[positionIndex].y, transform.localPosition.z);
    }

    public void MoveToPosition(int movingDataIndex) {
        StartCoroutine(Moving(transform.localPosition.x, transform.localPosition.y, movingData[movingDataIndex].destinationPosition.x, 
            movingData[movingDataIndex].destinationPosition.y, movingData[movingDataIndex].movingTimeInSeconds * 60));
    }

    public void Wiggle(int wiggleCurveIndex) {
        wiggleStart = true;
        wiggleIndex = wiggleCurveIndex;
    }

    IEnumerator Moving(float start_x, float start_y, float d_x, float d_y, float frames) {
        if (start_x < d_x) {
            if (start_y < d_y) {
                for(float i = start_x, j = start_y; i < d_x || j < d_y; i += (d_x-start_x)/frames, j += (d_y-start_y)/frames) {
                    transform.localPosition = new Vector3(i, j, transform.localPosition.z);
                    yield return null;
                }
            }
            else if (start_y > d_y) {
                for (float i = start_x, j = start_y; i < d_x || j > d_y; i += (d_x - start_x) / frames, j += (d_y - start_y) / frames) {
                    transform.localPosition = new Vector3(i, j, transform.localPosition.z);
                    yield return null;
                }
            }
            else {
                for(float i = start_x; i< d_x; i += (d_x - start_x) / frames) {
                    transform.localPosition = new Vector3(i, transform.localPosition.y, transform.localPosition.z);
                    yield return null;
                }
            }
        }
        else if(start_x > d_x) {
            if (start_y < d_y) {
                for (float i = start_x, j = start_y; i > d_x || j < d_y; i += (d_x - start_x) / frames, j += (d_y - start_y) / frames) {
                    transform.localPosition = new Vector3(i, j, transform.localPosition.z);
                    yield return null;
                }
            }
            else if (start_y > d_y) {
                for (float i = start_x, j = start_y; i > d_x || j > d_y; i += (d_x - start_x) / frames, j += (d_y - start_y) / frames) {
                    transform.localPosition = new Vector3(i, j, transform.localPosition.z);
                    yield return null;
                }
            }
            else {
                for (float i = start_x; i > d_x; i += (d_x - start_x) / frames) {
                    transform.localPosition = new Vector3(i, transform.localPosition.y, transform.localPosition.z);
                    yield return null;
                }
            }
        }
        else {
            if (start_y < d_y) {
                for (float j = start_y; j < d_y; j += (d_y - start_y) / frames) {
                    transform.localPosition = new Vector3(transform.localPosition.x, j, transform.localPosition.z);
                    yield return null;
                }
            }
            else if (start_y > d_y) {
                for (float j = start_y; j > d_y; j += (d_y - start_y) / frames) {
                    transform.localPosition = new Vector3(transform.localPosition.x, j, transform.localPosition.z);
                    yield return null;
                }
            }
        }
    }

}
