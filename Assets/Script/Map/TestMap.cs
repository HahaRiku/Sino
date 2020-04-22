using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour {
    public bool openMap;
    public bool closeMap;
    public bool 走過B2右一;
    public bool 走進B2右一房間;
    public bool 走過F2左一;
    public bool 走進F2左一房間;
    public bool 走過F2左一樓梯;
    public bool test;

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
        else if(走過B2右一) {
            走過B2右一 = false;
            MapSystem.SetPassedBy("B2牢房走廊1", Floor.FloorType.B2);
        }
        else if(走進B2右一房間) {
            走進B2右一房間 = false;
            MapSystem.SetEnteredTheRoom("B2_1_文青牢房", Floor.FloorType.B2);
        }
        else if (走過F2左一) {
            走過F2左一 = false;
            MapSystem.SetPassedBy("2F走廊6", Floor.FloorType.F2);
        }
        else if (走進F2左一房間) {
            走進F2左一房間 = false;
            MapSystem.SetEnteredTheRoom("2F_6_僕人房", Floor.FloorType.F2);
        }
        else if(走過F2左一樓梯) {
            走過F2左一樓梯 = false;
            MapSystem.SetPassedBy("2F左樓梯", Floor.FloorType.F2);
        }
    }
}
