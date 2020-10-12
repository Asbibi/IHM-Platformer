using UnityEngine;

public class UIControllerManager : UIMenuOpenable
{
    [SerializeField] PlayerController player = null;
    private string[] controllerInputsAxis = {"Cont_HorizSL",
                                                "Cont_HorizDPad",
                                                "Cont_HorizSR"};
    private string[] controllerInputsButton = {"Cont_A",
                                                "Cont_B",
                                                "Cont_X",
                                                "Cont_Y",
                                                "Cont_RB",
                                                "Cont_LB",
                                                "Cont_RT",
                                                "Cont_LT"};

    public void SetHorizInput(int _idInput)
    {
        player.horizInput = controllerInputsAxis[_idInput];
    }
    public void SetJumpInput(int _idInput)
    {
        player.jumpInput = controllerInputsButton[_idInput];
    }
    public void SetDashInput(int _idInput)
    {
        player.dashInput = controllerInputsButton[_idInput];
    }
}
