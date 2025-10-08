// Copyright (c) 2025 Frederick William Haslam born 1962 in the USA.

using AetheraModel.Builder;
using AetheraModel.Graph;
using Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Drawing;
using Controllers;
using UnityEngine.Events;
using System;
using AetheraModel.Engine;
using AetheraModel.Descriptors.Types;

namespace Controllers {

    public class PlayMapController : SingletonPersistentMonoBehaviour<PlayMapController> {

        static public readonly UnityEvent<int> starClickEvent = new UnityEvent<int>();

        public int width = 200;
        public int height = 200;

        public GameObject circleTemplate;
        public GameObject starTemplate;
        //public GameObject frameTemplate;


        static internal float FRAME_DEPTH = 1f;

        [HideInInspector]
        public Vector3 starLocalScale;

        public PlayEngine Engine { get; set; }

        public PlayMapDisplayer Display { get; set; }

        override protected void Initialize() {

            // save for repeated star creation
            PlayMapController.Instance.starLocalScale = starTemplate.transform.localScale;

            Display = new PlayMapDisplayer() { Boss=this };
            Engine = new PlayEngine();

            var puzzlesPath = Application.dataPath + "/_puzzles/";
            Engine.ReadPuzzleFolder( puzzlesPath );

            NewPuzzle();

            // wire up controls
            starClickEvent.AddListener(StarClicked);

        }

        public void NewPuzzle() {
            Engine.NextPuzzle();
            Display.NewPuzzleDraw();
        }

        internal void StarClicked( int starIndex ) {
            Debug.Log("Star Clicked = "+starIndex );

            DoMove( starIndex );
        }

        public void DoMove( int starId ) {

            var success = Engine.NextMove( starId );

            if (success) {
                Display.NewStateDraw();
            }
        }

        public void ResetPuzzle() {

            if (Engine.Reset()) {
                Display.NewStateDraw();
            }
        }

        public void UndoPuzzle() {

            var starId = Engine.UndoMove();

            if (starId>=0) {
                Display.NewStateDraw();
            }
        }

    }

}