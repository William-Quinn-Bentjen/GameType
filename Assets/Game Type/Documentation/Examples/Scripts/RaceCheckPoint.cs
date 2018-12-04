using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class RaceCheckPoint : GameTypeSpecificObject {
    [Range(0, 999999)]
    public int checkPointNumber = 0;
    public delegate void ColliderTouched(Collision collision);
    public ColliderTouched onTouched;
    private void OnCollisionEnter(Collision collision)
    {
        onTouched?.Invoke(collision);
    }
    private void Reset()
    {
        checkPointNumber = FindObjectsOfType<RaceCheckPoint>().Length-1;
        gameTypeTag = GameTypeTag.Race;
    }
    private void OnDrawGizmos()
    {
        drawString("Check Point # " + checkPointNumber, transform.position);
    }
    //taken from https://answers.unity.com/questions/44848/how-to-draw-debug-text-into-scene.html
    static public void drawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();

        var restoreColor = GUI.color;

        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        if (view != null)
        {
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }

            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
        }

    }
}

