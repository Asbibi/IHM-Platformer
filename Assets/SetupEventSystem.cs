using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetupEventSystem : MonoBehaviour
{
    private StandaloneInputModule standaloneInputModule;
    private string[] controllerInputsHAxis = {"Cont_HorizSL",
                                                "Cont_HorizDPad",
                                                "Cont_HorizSR"};

    
    private string[] controllerInputsVAxis = {"Cont_VertSL",
                                                "Cont_VertDPad",
                                                "Cont_VertSR"};
    private string[] controllerInputsButton = {"Cont_A",
                                                "Cont_B",
                                                "Cont_X",
                                                "Cont_Y",
                                                "Cont_RB",
                                                "Cont_LB",
                                                "Cont_RT",
                                                "Cont_LT"};

    // Start is called before the first frame update
    void Start()
    {
        standaloneInputModule = GetComponent<StandaloneInputModule>();
        standaloneInputModule.horizontalAxis = controllerInputsHAxis[PlayerPrefs.GetInt("MoveLatInput")];
        standaloneInputModule.verticalAxis = controllerInputsVAxis[PlayerPrefs.GetInt("MoveLatInput")];
        standaloneInputModule.submitButton = controllerInputsButton[PlayerPrefs.GetInt("JumpInput")];       
    }
}
