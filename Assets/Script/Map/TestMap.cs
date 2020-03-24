using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour {
    public bool openMap;
    public bool closeMap;
    public bool 走過F1左二;
    public bool 走進F1左二房間;

    private MapController mapController;

	// Use this for initialization
	void Start () {
        mapController = GetComponent<MapController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(openMap) {
            openMap = false;
            mapController.OpenMap();
        }
        else if(closeMap) {
            closeMap = false;
            mapController.CloseMap();
        }
        else if(走過F1左二) {
            走過F1左二 = false;
            MapSystem.SetPassedBy("1F走廊5", Floor.FloorType.F1);
        }
        else if(走進F1左二房間) {
            走進F1左二房間 = false;
            MapSystem.SetEnteredTheRoom("1F_5_花園&墓園", Floor.FloorType.F1);
        }
	}
}
