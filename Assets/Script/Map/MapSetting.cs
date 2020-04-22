using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetting : MonoBehaviour {
    public bool setPassedByInBeginning;
    public bool setEnteredInBeginning;

    private void Start() {
        if(setPassedByInBeginning) {
            CorridorSetPassedBy(SystemVariables.Scene);
        }
        if(setEnteredInBeginning) {
            RoomSetEntered(SystemVariables.Scene);
        }
    }

    public void CorridorSetPassedBy(string sceneName) {
        Floor.FloorType floorType;
        string floorTypeStr = sceneName.Substring(0, 2);
        if (floorTypeStr == "3F") {
            floorType = Floor.FloorType.F3;
        }
        else if (floorTypeStr == "2F") {
            floorType = Floor.FloorType.F2;
        }
        else if (floorTypeStr == "1F") {
            floorType = Floor.FloorType.F1;
        }
        else if (floorTypeStr == "B1") {
            floorType = Floor.FloorType.B1;
        }
        else if(floorTypeStr == "B2") {
            floorType = Floor.FloorType.B2;
        }
        else {
            floorType = Floor.FloorType.B3;
        }
        if(!MapSystem.HavePassedBy(sceneName, floorType)) {
            MapSystem.SetPassedBy(sceneName, floorType);
        }
    }

    public void RoomSetEntered(string sceneName) {
        Floor.FloorType floorType;
        string floorTypeStr = sceneName.Substring(0, 2);
        if (floorTypeStr == "3F") {
            floorType = Floor.FloorType.F3;
        }
        else if (floorTypeStr == "2F") {
            floorType = Floor.FloorType.F2;
        }
        else if (floorTypeStr == "1F") {
            floorType = Floor.FloorType.F1;
        }
        else if (floorTypeStr == "B1") {
            floorType = Floor.FloorType.B1;
        }
        else if (floorTypeStr == "B2") {
            floorType = Floor.FloorType.B2;
        }
        else {
            floorType = Floor.FloorType.B3;
        }
        if (!MapSystem.HaveEnteredTheRoom(sceneName, floorType)) {
            MapSystem.SetEnteredTheRoom(sceneName, floorType);
        }
    }
}
