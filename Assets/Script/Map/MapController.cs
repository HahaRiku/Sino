using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour {
    private bool open = false;
    private Animator ani;

    public struct FloorObj {
        public MapGradient mapGradient;
        public Image[] roomImgs;
    }
    private FloorObj[] floors;

    public struct Stair {
        public GameObject LeftStair;
        public GameObject RightStair;
    }
    private Stair[] stairs;
    /*private MapGradient F3, F2, F1, B1, B2, B3;
    private GameObject F3_F2, F2_F1, F1_B1, B1_B2, B2_B3;*/

    public Sprite transparent;
    public Sprite quesMark;

	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        GameObject floorsObj = transform.GetChild(0).GetChild(0).gameObject;
        GameObject stairsObj = transform.GetChild(0).GetChild(1).gameObject;
        floors = new FloorObj[6];
        stairs = new Stair[5];
        for(int i =0; i<MapSystem.data.floors.Length; i++) {
            GameObject thisFloor = floorsObj.transform.GetChild(i).gameObject;
            floors[i].mapGradient = thisFloor.GetComponent<MapGradient>();
            floors[i].roomImgs = new Image[MapSystem.data.floors[i].rooms.Length];
            for(int j =0; j<MapSystem.data.floors[i].rooms.Length; j++) {
                floors[i].roomImgs[j] = thisFloor.transform.GetChild(j).GetComponent<Image>();
            }
            if (i != MapSystem.data.floors.Length - 1) {
                stairs[i].LeftStair = stairsObj.transform.GetChild(i).GetChild(0).gameObject;
                stairs[i].RightStair = stairsObj.transform.GetChild(i).GetChild(1).gameObject;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(!open) {
            //update map
            if(MapSystem.dirty) {
                MapSystem.dirty = false;
                foreach(Floor f in MapSystem.data.floors) {
                    int floorIndex = 0;
                    if (f.樓層 == Floor.FloorType.F3) floorIndex = 0;
                    else if (f.樓層 == Floor.FloorType.F2) floorIndex = 1;
                    else if (f.樓層 == Floor.FloorType.F1) floorIndex = 2;
                    else if (f.樓層 == Floor.FloorType.B1) floorIndex = 3;
                    else if (f.樓層 == Floor.FloorType.B2) floorIndex = 4;
                    else if (f.樓層 == Floor.FloorType.B3) floorIndex = 5;
                    bool[] walked = new bool[f.rooms.Length];
                    foreach(Room r in f.rooms) {
                        //判定走過: 動gradient
                        //只有走廊: 房間img放透明
                        //未進去過&&有房間: 房間img放問號, 進去過&&有房間: 房間img放icon
                        walked[r.從左數來第幾間 - 1] = r.走過;
                        
                        if(!r.走過 || r.房間類型 == Room.Type.只有走廊) {
                            floors[floorIndex].roomImgs[r.從左數來第幾間 - 1].sprite = transparent;
                        }
                        else if (!r.進去過了) {
                            floors[floorIndex].roomImgs[r.從左數來第幾間 - 1].sprite = quesMark;
                        }
                        else {
                            floors[floorIndex].roomImgs[r.從左數來第幾間 - 1].sprite = r.icon;
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
                                floors[floorIndex].mapGradient.leftColor = new Color(1, 1, 1, 1);
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
                                floors[floorIndex].mapGradient.leftColor = new Color(1, 1, 1, 0);
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
                    floors[floorIndex].mapGradient.gradients = new MapGradient.GradientInfo[gradients.Count];
                    int index = 0;
                    foreach(MapGradient.GradientInfo g in gradients) {
                        floors[floorIndex].mapGradient.gradients[index] = g;
                        index++;
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
    
}
