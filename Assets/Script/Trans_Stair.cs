using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trans_Stair : MonoBehaviour
{
    [System.Serializable]
    public struct Portal
    {
        public Object targetScene;
        public GameStateManager.SpawnPoint nextScenePos;    //0=null , 1=L , 2=R , 3=door , 4=stair
    };
    [SerializeField]
    private Portal[] portalList;

    public enum Stair
    {
        UpAndDown,
        Up,
        Down
    };
    public Stair StairType;

    void OnTriggerEnter2D(Collider2D col_item)
    {
        StartCoroutine(TransUI());
    }

    private IEnumerator TransUI()
    {
        CharWithItem.actEnable = false;     //limit moving

        switch ((int)StairType)
        {
            case 0:
                this.transform.GetChild(0).gameObject.SetActive(true);
                this.transform.GetChild(1).gameObject.SetActive(true);

                //UI+choose
                bool done = false;
                while (!done)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        done = true;
                        GameStateManager.Instance.黑幕轉場(portalList[0].targetScene.name, portalList[0].nextScenePos);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        done = true;
                        GameStateManager.Instance.黑幕轉場(portalList[1].targetScene.name, portalList[1].nextScenePos);
                    }
                    /*else if(Input.GetKeyDown(KeyCode.X)){	//cancel
						
					}*/
                    yield return 0;
                }
                break;
            case 1:
                this.transform.GetChild(0).gameObject.SetActive(true);
                done = false;
                while (!done)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        done = true;
                        GameStateManager.Instance.黑幕轉場(portalList[0].targetScene.name, portalList[0].nextScenePos);
                    }
                    yield return 0;
                }
                break;
            case 2:
                this.transform.GetChild(1).gameObject.SetActive(true);
                done = false;
                while (!done)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        done = true;
                        GameStateManager.Instance.黑幕轉場(portalList[0].targetScene.name, portalList[0].nextScenePos);
                    }
                    yield return 0;
                }
                break;
            default: break;
        }
    }
}
