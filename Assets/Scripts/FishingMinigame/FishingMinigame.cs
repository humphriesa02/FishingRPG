using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FishingState
{
	Aiming,
	Cast,
    Reeling,
    End
}

public class FishingMinigame : MonoBehaviour
{
    public FishingState FishingState;

    private BattleHUD battleHUD;

    private float aimPercentage = 0f;
    private float castPercentage = 0f;
    private float reelingPercentage = 0f;
    private float successChange = 0f;
    void Start()
    {
        FishingState = FishingState.Aiming;
        battleHUD = GetComponentInParent<BattleHUD>();
    }

    void Update()
    {
        switch (FishingState)
        {
            case FishingState.Aiming:
                if (Input.GetButtonDown("Submit"))
                {
                    ConfirmAim();
                }
                break;
            case FishingState.Cast:
                break;
            case FishingState.Reeling:
                break;
            case FishingState.End:
                battleHUD.SwapToActionMenu();
                break;
        }
    }
    
    void ChangeState(FishingState state)
    {
        FishingState = state;
    }

    public void setAimPercentage(float percent)
    {
        if (FishingState == FishingState.Aiming)
        {
            aimPercentage = percent;
        }
    }

    void ConfirmAim()
    {
        print(aimPercentage);
    }

}
