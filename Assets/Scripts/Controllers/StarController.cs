// Copyright (c) 2025 Frederick William Haslam born 1962 in the USA.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers {

    public class StarController : MonoBehaviour {

        internal enum StarState {
            Waiting,
            Bouncing,
            Throbbing,
            Spinning
        }

        public float MinWait = 5f;
        public float MaxWait = 20f;
        public float StateTime = 4f;

        internal int starIndex;
        internal float nextStateTime,stateStartTime;
        internal StarState currentState;
        internal Vector3 startLoc,startScale;
        internal Quaternion startRot;

        // Start is called before the first frame update
        public void Initialize( int index ) {

            starIndex = index;

            startLoc = transform.localPosition;
            startRot = transform.localRotation;
            startScale = transform.localScale;

            SetWaitingTime();
        }

        // Called between end of round and start of next round
        public void Cleanup() {

            transform.localPosition = startLoc;
            transform.localRotation = startRot;
            transform.localScale = startScale;

            SetWaitingTime();
        }


        void OnMouseDown() {

            //Debug.Log("Star Mouse Down");

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Debug.Log("star Raycast? = " + Physics.Raycast(ray, out hit));

            if (Physics.Raycast(ray, out hit)) {

                Debug.Log("star RayCast name = " + hit.transform.name);
                //Debug.Log("star RayCast loc = " + hit.point);
                //var localV3 = transform.InverseTransformPoint(hit.point);
                //var relative = localV3 + new Vector3( 0.5f, 0.5f, 0f );
                //var scaled = Vector3.Scale( localV3 + new Vector3( 0.5f, 0.5f, 0f ), new Vector3( width, height, 1f ) );
                //Debug.Log("star RayCast Local = " + localV3);

                // Check if hit.transform is door, 
                //if(...) GetComponent<Animator>().SetTrigger("Tor");

                PlayMapController.starClickEvent.Invoke( starIndex );
            }
        }

        /// <summary>
        /// When waiting, set next state.
        /// </summary>
        void SetWaitingTime() {
            SetStateCompletionTime( StarState.Waiting, MinWait + UnityEngine.Random.value * (MaxWait-MinWait) );
        }

        /// <summary>
        /// Set next state, record current loc and rot.
        /// </summary>
        /// <param name="secs"></param>
        void SetStateCompletionTime( StarState newState, float secs ) {
//Debug.Log("Next State = "+secs+"s");
            currentState = newState;
            RestoreStartPosition();
            stateStartTime = Time.time;
            nextStateTime += secs;
        }

        void RestoreStartPosition() {
            transform.localPosition = startLoc;
            transform.localRotation = startRot;
            transform.localScale = startScale;
        }

        // Update is called once per frame
        void Update() {

            switch (currentState) {

                case StarState.Waiting: DoWaiting(); break;

                case StarState.Bouncing: DoBouncing(); break;
                case StarState.Throbbing: DoThrobbing(); break;
                case StarState.Spinning: DoSpinning(); break;
            }
        
        }

        internal void DoWaiting() {
           if (Time.time > nextStateTime ) {
                int newState = 1 + (int)( Random.value * 3 );
                SetStateCompletionTime( (StarState)newState, StateTime );
           }
        }

        static readonly float RADIANS = 2 * Mathf.PI;

        internal void DoBouncing() {

            if (Time.time > nextStateTime ) {
                SetWaitingTime();
                return;
            }

            var delta = Time.time - stateStartTime;
            var jump = ( 1f - Mathf.Cos( delta * RADIANS ) ) * 3f;
            if (jump<0) jump = 0f;

            transform.localPosition = startLoc + Vector3.up * jump;

        }

        internal void DoThrobbing() {

            if (Time.time > nextStateTime ) {
                SetWaitingTime();
                return;
            }

            var delta = Time.time - stateStartTime;
            var jump = ( 1f - Mathf.Cos( delta * RADIANS ) );
            if (jump<0) jump = 0f;

            transform.localScale = startScale * (1+jump);

        }
        internal void DoSpinning() {

           if (Time.time > nextStateTime ) {
                SetWaitingTime();
                return;
           }

            var delta = Time.time - stateStartTime;
            var jump = ( 1f - Mathf.Cos( delta * RADIANS ) );
            if (jump<0) jump = 0f;

            var spin = Quaternion.Euler( 0f, 0f, jump * 180f );

            transform.localRotation = startRot * spin;

        }

    }
    
}
