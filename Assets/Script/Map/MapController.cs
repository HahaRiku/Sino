using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour {
    private bool open = false;
    private Animator ani;

    public struct FloorObj {
        public Image floorNum;
        public MapGradient mapGradient;
        public Image[] roomImgs;
    }
    private FloorObj[] floors;

    public struct Stair {
        public Image LeftStair;
        public Image RightStair;
    }
    private Stair[] stairs;
    /*private MapGradient F3, F2, F1, B1, B2, B3;
    private GameObject F3_F2, F2_F1, F1_B1, B1_B2, B2_B3;*/

    public Sprite transparent;
    public Sprite quesMark;
    public Sprite leftStair;
    public Sprite rightStair;

	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        GameObject floorsObj = transform.GetChild(0).GetChild(0).gameObject;
        GameObject stairsObj = transform.GetChild(0).GetChild(1).gameObject;
        GameObject floorNumObj = transform.GetChild(0).GetChild(2).gameObject;
        floors = new FloorObj[6];
        stairs = new Stair[5];
        for(int i =0; i<MapSystem.data.floors.Length; i++) {
            GameObject thisFloor = floorsObj.transform.GetChild(i).gameObject;
            floors[i].floorNum = floorNumObj.transform.GetChild(i).gameObject.GetComponent<Image>();
            floors[i].floorNum.sprite = transparent;
            floors[i].mapGradient = thisFloor.GetComponent<MapGradient>();
            floors[i].roomImgs = new Image[(MapSystem.data.floors[i].樓層 == Floor.FloorType.F3) ? MapSystem.data.floors[i].rooms.Length - 1 : MapSystem.data.floors[i].rooms.Length - 2];
            for(int j =0; j < ((MapSystem.data.floors[i].樓層 == Floor.FloorType.F3) ? MapSystem.data.floors[i].rooms.Length - 1 : MapSystem.data.floors[i].rooms.Length - 2); j++) {
                floors[i].roomImgs[j] = thisFloor.transform.GetChild(j).GetComponent<Image>();
            }
            if (i != MapSystem.data.floors.Length - 1) {
                stairs[i].LeftStair = stairsObj.transform.GetChild(i).GetChild(0).gameObject.GetComponent<Image>();
                stairs[i].LeftStair.sprite = transparent;
                stairs[i].RightStair = stairsObj.transform.GetChild(i).GetChild(1).gameObject.GetComponent<Image>();
                stairs[i].RightStair.sprite = transparent;
            }
        }
        ClearMap();
	}
	
	// Update is called once per frame
	void Update () {
		if(!open) {
            //update map
            if(MapSystem.dirty) {
                MapSystem.dirty = false;
                bool[] floorWalked = new bool[6];
                for(int i = 0; i < 6; i++) {
                    floorWalked[i] = false;
                }
                bool[] floorLeftWalked = new bool[6];
                bool[] floorRightWalked = new bool[6];
                foreach (Floor f in MapSystem.data.floors) {
                    bool[] walked = new bool[(f.樓層 == Floor.FloorType.F3) ? f.rooms.Length - 1 : f.rooms.Length - 2];
                    
                    foreach(Room r in f.rooms) {    //換每間的icon 和 記錄走過 以算gradient
                        //判定走過: 動gradient
                        //只有走廊: 房間img放透明
                        //未進去過&&有房間: 房間img放問號, 進去過&&有房間: 房間img放icon
                        if(r.從左數來第幾間 > 0) {
                            walked[r.從左數來第幾間 - 1] = r.走過;

                            if (!r.走過 || r.房間類型 == Room.Type.只有走廊) {
                                floors[(int)f.樓層].roomImgs[r.從左數來第幾間 - 1].sprite = transparent;
                            }
                            else if (!r.進去過了) {
                                floors[(int)f.樓層].roomImgs[r.從左數來第幾間 - 1].sprite = quesMark;
                            }
                            else {
                                floors[(int)f.樓層].roomImgs[r.從左數來第幾間 - 1].sprite = r.icon;
                            }
                        }
                        else if(r.從左數來第幾間 == -1) {  //left stairs room
                            floorLeftWalked[(int)f.樓層] = r.走過;
                        }
                        else if(r.從左數來第幾間 == -2) {  //right stairs room
                            floorRightWalked[(int)f.樓層] = r.走過;
                        }
                        if(!floorWalked[(int)f.樓層] && r.走過) {
                            floorWalked[(int)f.樓層] = true;
                        }
                    }
                    int record = -1;
                    bool recordedWalked = false;
                    List<MapGradient.GradientInfo> gradients = new List<MapGradient.GradientInfo>();
                    for(int i =0; i<walked.Length; i++) {
                        if(walked[i]) {
                            if(record == -1) {
                                recordedWalked = true;
                                record = 1;
                                floors[(int)f.樓層].mapGradient.leftColor = new Color(1, 1, 1, 1);
                            }
                            else {
                                if(recordedWalked) {
                                    record++;
                                }
                                else {
                                    record = 0;
                                    recordedWalked = true;
                                    MapGradient.GradientInfo g;
                                    g.color = new Color(1, 1, 1, 1);
                                    g.position = f.rooms[0].房間左邊界Pos;
                                    foreach (Room r in f.rooms) {
                                        if(r.從左數來第幾間 - 1 == i) {
                                            g.position = r.房間左邊界Pos;
                                            break;
                                        }
                                    }
                                    gradients.Add(g);
                                }
                            }
                        }
                        else {
                            if(record == -1) {
                                recordedWalked = false;
                                record = 1;
                                floors[(int)f.樓層].mapGradient.leftColor = new Color(1, 1, 1, 0);
                            }
                            else {
                                if(!recordedWalked) {
                                    record++;
                                }
                                else {
                                    record = 0;
                                    recordedWalked = false;
                                    MapGradient.GradientInfo g;
                                    g.color = new Color(1, 1, 1, 0);
                                    g.position = f.rooms[0].房間左邊界Pos;
                                    foreach (Room r in f.rooms) {
                                        if (r.從左數來第幾間 - 1 == i) {
                                            g.position = r.房間左邊界Pos;
                                            break;
                                        }
                                    }
                                    gradients.Add(g);
                                }
                            }
                        }
                    }
                    floors[(int)f.樓層].mapGradient.gradients = new MapGradient.GradientInfo[gradients.Count];
                    int index = 0;
                    foreach(MapGradient.GradientInfo g in gradients) {
                        floors[(int)f.樓層].mapGradient.gradients[index] = g;
                        index++;
                    }

                    for(int i = 0; i < f.rooms.Length; i++) {
                        if (f.rooms[i].從左數來第幾間 == 1 && f.樓層 != Floor.FloorType.F3) {
                            if (floorLeftWalked[(int)f.樓層] && !f.rooms[i].走過) {
                                //加上Gradient
                                MapGradient.GradientInfo[] temp = new MapGradient.GradientInfo[floors[(int)f.樓層].mapGradient.gradients.Length + 1];
                                MapGradient.GradientInfo tempG;
                                tempG.color = new Color(1, 1, 1, 0);
                                tempG.position = f.rooms[i + 1].房間左邊界Pos / 3.0f;
                                temp[0] = tempG;
                                for(int j = 1; j < floors[(int)f.樓層].mapGradient.gradients.Length + 1; j++) {
                                    temp[j] = floors[(int)f.樓層].mapGradient.gradients[j - 1];
                                }
                                floors[(int)f.樓層].mapGradient.leftColor = new Color(1, 1, 1, 1);
                                floors[(int)f.樓層].mapGradient.gradients = temp;
                            }
                        }
                        else if (f.rooms[i].從左數來第幾間 == ((f.樓層 == Floor.FloorType.F3) ? f.rooms.Length - 1 : f.rooms.Length - 2)) {
                            if (floorRightWalked[(int)f.樓層] && !f.rooms[i].走過) {
                                //加上Gradient
                                MapGradient.GradientInfo[] temp = new MapGradient.GradientInfo[floors[(int)f.樓層].mapGradient.gradients.Length + 1];
                                MapGradient.GradientInfo tempG;
                                tempG.color = new Color(1, 1, 1, 1);
                                tempG.position = f.rooms[i].房間左邊界Pos + (1 - f.rooms[i].房間左邊界Pos) / 3.0f;
                                for(int j = 0; j < floors[(int)f.樓層].mapGradient.gradients.Length; j++) {
                                    temp[j] = floors[(int)f.樓層].mapGradient.gradients[j];
                                }
                                temp[floors[(int)f.樓層].mapGradient.gradients.Length] = tempG;
                                floors[(int)f.樓層].mapGradient.gradients = temp;
                            }
                        }
                    }
                }

                for(int i = 0; i < 6; i++) {
                    if(floorWalked[i]) {
                        floors[i].floorNum.sprite = MapSystem.data.floors[i].floorNum;
                    }
                    if (floorLeftWalked[i]) {
                        if (i == 1) {
                            stairs[i].LeftStair.sprite = leftStair;
                        }
                        else if (i == 5) {
                            stairs[i - 1].LeftStair.sprite = leftStair;
                        }
                        else if (i > 1 && i < 5) {
                            stairs[i].LeftStair.sprite = leftStair;
                            stairs[i - 1].LeftStair.sprite = leftStair;
                        }
                    }
                    if (floorRightWalked[i]) {
                        if (i == 0) {
                            stairs[i].RightStair.sprite = rightStair;
                        }
                        else if (i == 5) {
                            stairs[i - 1].RightStair.sprite = rightStair;
                        }
                        else {
                            stairs[i].RightStair.sprite = rightStair;
                            stairs[i - 1].RightStair.sprite = rightStair;
                        }
                    }
                }
            }
        }
	}

    public void OpenMap() {
        if(!open) {
            open = true;
            ani.SetTrigger("OpenMap");
        }
    }

    public void CloseMap() {
        if(open) {
            open = false;
            ani.SetTrigger("CloseMap");
        }
    }

    public void ClearMap() {
        for(int i = 0; i < 6; i++) {
            floors[i].floorNum.sprite = transparent;
            floors[i].mapGradient.leftColor = new Color(1, 1, 1, 0);
            floors[i].mapGradient.gradients = new MapGradient.GradientInfo[0];
            for(int j = 0; j < floors[i].roomImgs.Length; j++) {
                floors[i].roomImgs[j].sprite = transparent;
            }
        }
        for(int i = 0; i < 5; i++) {
            stairs[i].LeftStair.sprite = transparent;
            stairs[i].RightStair.sprite = transparent;
        }
    }
    
    public bool GetOpen() {
        return open;
    }
}
