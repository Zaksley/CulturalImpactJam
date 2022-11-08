using UnityEngine;
using UnityEditor;

namespace Maarti {
    [CustomEditor(typeof(WallBuilder))]
    [System.Serializable]
    public class WallBuilderEditor : Editor {

        private static Vector3 pointSnap = Vector3.one * 0.5f;

        public override void OnInspectorGUI() {
            WallBuilder wallGenerator = (WallBuilder)target;

            // Style
            GUIStyle style = new GUIStyle("BoldLabel");
            style.normal.textColor = Color.blue;

            // Add crossing point
            if (GUILayout.Button("Add crossing point")) {
                wallGenerator.AddCrossingPoint();
            }

            // Remove crossing point
            if (GUILayout.Button("Remove last crossing point")) {
                wallGenerator.RemoveLastCrossingPoint();
            }

            // GUILayout.Space(20);
            DrawDefaultInspector();
        }

        public void OnSceneGUI() {
            WallBuilder wallGenerator = target as WallBuilder;
            float handleSize = Mathf.Max(wallGenerator.blockSize / 2f * wallGenerator.handleSize, 0.05f);
            foreach (Transform child in wallGenerator.transform) {
                if (child.name.StartsWith("WallPoint")) {
                    Vector3 oldPoint = child.position;
                    Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, handleSize, pointSnap, Handles.SphereHandleCap);
                    if (oldPoint != newPoint) {
                        Undo.RecordObject(child, "Move");
                        if (wallGenerator.lockYAxis) {
                            // We lock the Y position when moving handles
                            Plane plane = new Plane(Vector3.up, oldPoint);
                            Vector3 sceneCameraPosition = SceneView.currentDrawingSceneView.camera.transform.position;
                            Ray ray = new Ray(sceneCameraPosition, newPoint - sceneCameraPosition);
                            if (plane.Raycast(ray, out float distance)) {
                                newPoint = ray.GetPoint(distance);
                            }
                            newPoint.y = oldPoint.y;
                        }
                        child.position = newPoint;
                        wallGenerator.Update();
                    }
                }
            }
        }

    }
}

