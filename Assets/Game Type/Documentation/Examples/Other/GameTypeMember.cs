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
    protected override bool Join(BaseTeam teamToJoin = null)
    {
        if (base.Join(teamToJoin))
        {
            if (meshRenderer != null && meshRenderer.material != null && teamToJoin != null && teamToJoin.data != null)
            {
                meshRenderer.material.color = teamToJoin.data.TeamColor;
            }
            TestGameManager.instance.text.text = teamToJoin.data.TeamName;
            //GameManager.Instance.GameType.Score.scoreKeeper.Add(teamToJoin, 0); // AddPoints(teamToJoin, 0);
            return true;
        }
        return false;
    }
}
