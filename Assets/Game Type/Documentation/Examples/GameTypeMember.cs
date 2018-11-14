using System.Collections;
using System.Collections.Generic;
using Teams.Base;
using UnityEngine;

public class GameTypeMember : Teams.Base.BaseTeamMember {
    public MeshRenderer meshRenderer;
    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void Awake()
    {
        base.Awake();
    }
    // Use this for initialization
    protected override bool Join(BaseTeam teamToJoin = null)
    {
        if (base.Join(teamToJoin))
        {
            if (meshRenderer != null && meshRenderer.material != null && teamToJoin != null && teamToJoin.data != null)
            {
                meshRenderer.material.color = teamToJoin.data.TeamColor;
            }
            //gets here on build
            //TestGameManager.instance.text.text = teamToJoin.data.TeamName;
            TestGameManager.instance.text.text = "Build gets here";
            GameManager.Instance.GameType.Points.AddPoints(teamToJoin, 500);
            //cant pass here on build
            TestGameManager.instance.text.text = "Build can't get here";
            //gets here on play
            TestGameManager.instance.text.text = GameManager.Instance.GameType.Points.CheckPoints(teamToJoin).ToString() + " Points";
            return true;
        }
        return false;
    }
}
