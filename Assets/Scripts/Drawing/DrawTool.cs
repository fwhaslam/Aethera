// Copyright (c) 2023 Frederick William Haslam born 1962 in the USA.

using UnityEngine;
using UnityEngine.Analytics;

namespace Drawing {

    public class DrawTool {

        static public void BuildRectangle(
                GameObject gameObject, float mult,
                int left, int top, int right, int bottom ) {

            // Initialize LineRenderer
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 5; // 4 corners + closing point
            lineRenderer.loop = true;

            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.widthMultiplier = mult;
            var m2 = mult / 2f;

            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;

            // Define rectangle corners
            Vector3[] corners = new Vector3[] {
                new Vector3(left-m2, bottom+m2, 0),
                new Vector3(right+m2, bottom+m2, 0),
                new Vector3(right+m2, top-m2, 0),
                new Vector3(left-m2, top-m2, 0),
                new Vector3(left-m2, bottom+m2, 0) // Close the rectangle
            };

            // Set positions in LineRenderer
            lineRenderer.SetPositions(corners);
        }


        static public void BuildCircle(
            GameObject gameObject, float mult,
            Vector3 center, float radius, int steps) {
                        
            var lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.loop = true;         // Ensure the circle is closed
            lineRenderer.useWorldSpace = false; // Use local space for easy positioning

            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.widthMultiplier = mult;
            
            lineRenderer.positionCount = steps;
            float angleStep = 360f / steps;

            for (int i = 0; i < steps; i++) {
                float angle = Mathf.Deg2Rad * angleStep * i;
                Vector3 position = new Vector3(
                    center.x + Mathf.Cos(angle) * radius,
                    center.y + Mathf.Sin(angle) * radius,
                    0f
                );
                lineRenderer.SetPosition(i, position);
            }
        }
    }
    
}
