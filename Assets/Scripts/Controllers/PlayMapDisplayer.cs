// Copyright (c) 2025 Frederick William Haslam born 1962 in the USA.

using AetheraModel.Builder;
using AetheraModel.Descriptors.Types;
using AetheraModel.Engine;
using AetheraModel.Graph;
using Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers {

    public class PlayMapDisplayer {

        static internal float FRAME_DEPTH = 1f;

        public PlayMapController Boss { get; set; }
        public PlayEngine Engine { get { return Boss.Engine;} }
        public List<Star> Stars { get { return Engine.Puzzle.Stars; } }

        internal int Width { get{return Boss.width; } }
        internal int Height { get{return Boss.height; } }

        public void NewPuzzleDraw() {
Debug.Log("=== DrawTooling()");
            //FixClickBox( Width, Height );
            //FixFrame( 0f, 0f, Width, Height );
            CreateStars();
            NewStateDraw();

//Debug.Log("=== Map Summary\n"+Boss.Engine.Puzzle.ToDisplayGrid());
        }

        public void NewStateDraw() {
Debug.Log("=== Redraw()");
            RedrawStars();
        }

        internal string StarName( int starId ) {  return "Star-"+starId; }
        internal string RangeName( int starId ) {  return "Star-Range-"+starId; }

        //internal void FixClickBox( int widt, int tall, string boxName = "ClickBoxGO" ) {

        //    // find / create frame
        //    var workTfm = Boss.transform.Find( boxName );


        //    if (workTfm==null) {

        //        workTfm = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        //        workTfm.name = boxName;
        //        workTfm.SetParent( Boss.transform );

        //        workTfm.gameObject.AddComponent<BoxCollider>();
        //    }

        //    // locate frame
        //    workTfm.localPosition = Vector3.zero;
        //    workTfm.localScale = new Vector2( Width, Height );

        //}

        //internal void FixFrame( float left, float top, float right, float bottom, string frameName = "FrameImageGO" ) {

        //    // find / create frame
        //    var workTfm = Boss.transform.Find( frameName );


        //    if (workTfm==null) {
        //        workTfm = PlayMapController.Instantiate( Boss.frameTemplate.transform );
        //        workTfm.name = frameName;
        //        workTfm.SetParent( Boss.transform );
        //    }

        //    // locate frame
        //    workTfm.localPosition = new Vector3( 0,0, FRAME_DEPTH );
        //    workTfm.localScale = new Vector2( Width, Height );
        //    //frameTransform.localPosition = new Vector3( (left+right)/2, (top+bottom)/2, +1f );
        //    //frameTransform.localScale = new Vector2( right, bottom );

        //}

        /// <summary>
        /// New Puzzle draw - create all objects, place locations.
        /// </summary>
        internal void CreateStars() {

Debug.Log("=== CreateStar (" + Stars.Count+ ")" );
            // all children, stars and ranges
            var names = new HashSet<string>();
            foreach (Transform child in Boss.transform) {
                var name = child.name;
                if (name.StartsWith("Star-")) names.Add(name);
            }

            // find or create
            for ( int starIx=0;starIx<Stars.Count;starIx++) {

                var starName = StarName( starIx );
                var rangeName = RangeName( starIx );

                Transform starTfm=null, rangeTfm=null;

                // create star + range
                if (!names.Contains(starName)) { 
                    starTfm = PlayMapController.Instantiate( Boss.starTemplate.transform, Boss.transform );
                    starTfm.name = starName;

                    rangeTfm = GameObject.Instantiate(Boss.circleTemplate.transform, Boss.transform );
                    rangeTfm.name = rangeName;

                    // scale + scripts
                    starTfm.localScale = Boss.starLocalScale * ( (starIx==0 || starIx==1) ? 12f : 8f );
                    rangeTfm.localScale = 2 * 100f * Vector2.one;
                }
                else {
                    starTfm = Boss.transform.Find(starName);
                    rangeTfm = Boss.transform.Find(rangeName);

                    starTfm.gameObject.GetComponent<StarController>().Cleanup();

                    names.Remove(starName);
                    names.Remove(rangeName);
                }

                // update locations, visibility
                var loc = Stars[starIx].Loc;
                starTfm.position = new Vector3( (loc.Horz-Width/2f), (loc.Vert-Height/2f),  -0.1f );
                rangeTfm.position = new Vector3( (loc.Horz-Width/2f), (loc.Vert-Height/2f), -0.1f );

                // record initial location
                starTfm.gameObject.GetComponent<StarController>().Initialize( starIx );

                // visibility handled by 'redraw'
                //rangeTfm.gameObject.SetActive( Engine.Active(starIx) );
            }

            // remove unmatched names
Debug.Log("UNMATCHED NAMES = "+ string.Join(", ",names) );
            foreach ( var name in names ) {
                var sphere = Boss.transform.Find(name).gameObject;
                PlayMapController.DestroyImmediate(sphere);
            }
        }

        
        /// <summary>
        /// New State draw - set visibility on range, change star image.
        /// </summary>
        internal void RedrawStars() {

Debug.Log("=== RedrawStars" );

            // update ranges
            for ( int starIx=0;starIx<Stars.Count;starIx++) {

                var starName = StarName( starIx );
                var rangeName = RangeName( starIx );

                Debug.Log("Finding Star = "+starIx );
                //var star = Boss.transform.Find(starName).gameObject;
                var range = Boss.transform.Find(rangeName).gameObject;

                range.SetActive( Engine.Puzzle.Stars[starIx].State==StarStateEnum.Active );
            }

        }

    }

}