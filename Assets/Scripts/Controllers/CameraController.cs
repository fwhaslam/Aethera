// Copyright (c) 2023 Frederick William Haslam born 1962 in the USA.

namespace Controllers {

    using UnityEngine;
    using UnityEngine.Analytics;

    public class CameraController2D : MonoBehaviour {

        public float distance = -400;

        public float width = 200;
        public float height = 200;

        public float zoomSpeed = 25f;
        public float panSpeed = 5f;

        private Camera cam;
        private Vector3 dragOrigin;
        private float minOrthographicSize;
        private float maxOrthographicSize;

        void Start() {
            cam = Camera.main;
            SetZoomLimits();
            CenterCamera();
        }

        void Update() {
            HandleZoom();
            HandlePan();
        }

        private void SetZoomLimits() {

            // Calculate orthographic sizes for zoom limits
            maxOrthographicSize = Mathf.Max(height, width / cam.aspect) * 0.55f;
            minOrthographicSize = maxOrthographicSize / 2;

            cam.orthographicSize = maxOrthographicSize;
        }

        /// <summary>
        /// Set camera pivot, then shift so center of map is at center of camera view.
        /// </summary>
        private void CenterCamera() {  

            var angleX = -10;
            cam.transform.rotation = Quaternion.Euler( angleX, 0, 0 );

            var radians = Mathf.PI * 2f * angleX / 360;
            var dy = distance * Mathf.Sin( radians );

            cam.transform.position = new Vector3( 0, -dy, distance );
        }

        private void HandleZoom() {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetKey(KeyCode.PageUp)) scroll += 0.1f;
            if (Input.GetKey(KeyCode.PageDown)) scroll -= 0.1f;

            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minOrthographicSize, maxOrthographicSize);
        }

        private void HandlePan() {
            Vector3 move = new Vector3();

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) move.y += panSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) move.y -= panSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) move.x -= panSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) move.x += panSpeed * Time.deltaTime;

            if (Input.GetMouseButtonDown(0))
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0)) {
                Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position += difference;
            }

            cam.transform.position += move;
            //ClampCameraPosition();
        }

        private void ClampCameraPosition() {

            float left = -100;
            float right = 100;
            float top = 100;
            float bottom = -100;

            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            //float centerX = 0;
            //float centerY = 0;

            float clampedX = Mathf.Clamp(cam.transform.position.x, left + halfWidth, right - halfWidth);
            float clampedY = Mathf.Clamp(cam.transform.position.y, bottom + halfHeight, top - halfHeight);

            cam.transform.position = new Vector3(clampedX, clampedY, -10f);

        }
    }

}