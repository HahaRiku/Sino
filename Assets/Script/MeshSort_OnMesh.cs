using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSort_OnMesh : MonoBehaviour {

	public string sortingLayerName;
	public int sortingOrder;
	private MeshRenderer renderer;
	
	void Start () {
		renderer = GetComponent<MeshRenderer>();
		renderer.sortingLayerName = sortingLayerName;
		renderer.sortingOrder = sortingOrder;
	}
	
}
