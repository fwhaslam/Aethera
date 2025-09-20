// Copyright (c) 2025 Frederick William Haslam born 1962 in the USA.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Controllers {

    public class ScoreController : MonoBehaviour {

        public TMP_Text text;

        // Start is called before the first frame update
        void Start() {
        
        }

        // Update is called once per frame
        void Update() {
        
//Debug.Log("INS="+PlayMapController.Instance);
//Debug.Log("ENG="+PlayMapController.Instance.Engine);
//Debug.Log("BSR="+PlayMapController.Instance.Engine.BestScore);
            if (PlayMapController.Instance!=null) {

                var best = PlayMapController.Instance.Engine.BestScore;
                var current = PlayMapController.Instance.Engine.CurrentScore;

                text.text = ""+best+"/"+current;
            }
        }

    }
}
