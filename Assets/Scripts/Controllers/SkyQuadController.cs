using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyQuadController : MonoBehaviour {

    internal Vector2 lastScreenSize;

    // Start is called before the first frame update
    public void Start() {
        lastScreenSize = Vector2.zero;
        Resize();
    }


    internal void Resize() {


		var camBack = Camera.main.transform.localPosition.z;
		var goDepth = transform.localPosition.z;

		var distance = ( goDepth + camBack ) * 2.05f;
		var v3BottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		var v3TopRight = Camera.main.ViewportToWorldPoint(new Vector3(1,1,distance));

		transform.localScale = new Vector3( v3BottomLeft.x-v3TopRight.x, v3BottomLeft.y-v3TopRight.y, distance );

	}


    // Update is called once per frame
    public void Update() {

        Vector2 screenSize = new Vector2(Screen.width, Screen.height); 
        if ( lastScreenSize != screenSize ) {
            Resize();
            lastScreenSize = screenSize;
        }
    }
}
