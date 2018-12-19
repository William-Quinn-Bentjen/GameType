using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaPlayer : Teams.TeamMember {
    public MeshRenderer meshRenderer;
    public Vector3 spawnOffset;
    public Color personalColor;
    public enum InputType
    {
        keyboard,
        controller1,
        controller2,
        controller3,
        controller4,
        none
    }
    public InputType input;
    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null)
        {
            meshRenderer.material.color = personalColor;
        }
    }
    public void SetColor(Color color)
    {
        if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && meshRenderer.material != null) meshRenderer.material.color = color;
    }
}
