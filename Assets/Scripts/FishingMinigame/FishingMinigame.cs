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

    [SerializeField] private GameObject AimingSectionContainer;
    [SerializeField] private GameObject CastSectionContainer;
    [SerializeField] private GameObject ReelingSectionContainer;

    private float aimPercentage = 0f;
    private float castPercentage = 0f;
    private float reelingPercentage = 0f;
    private float successChange = 0f;
    void Start()
    {
        FishingState = FishingState.Aiming;
        battleHUD = GetComponentInParent<BattleHUD>();

        ChangeState(FishingState.Aiming);
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
		AimingSectionContainer.SetActive(false);
		CastSectionContainer.SetActive(false);
		//ReelingSectionContainer.SetActive(false);
		switch (FishingState)
        {
            case FishingState.Aiming:
				AimingSectionContainer.SetActive(true);
				break;
            case FishingState.Cast:
                CastSectionContainer.SetActive(true);
                break;
            case FishingState.Reeling:
                ReelingSectionContainer.SetActive(true);
                break;
            case FishingState.End:
                break;
        }
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
        ChangeState(FishingState.Cast);
    }

}
