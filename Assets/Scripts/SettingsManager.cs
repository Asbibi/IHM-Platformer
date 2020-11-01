using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Toggle AudioFeedBack = null;
    [SerializeField] private Toggle VisualFeedBack = null;
    //---------------------------------------------
    [SerializeField] private UIParameterManager ParameterManager = null;
    [SerializeField] private InputField SpeedXMax = null;
    [SerializeField] private InputField SpeedYMin = null;
    [SerializeField] private InputField SpeedJump = null;
    [SerializeField] private InputField SpeedWallJump = null;
    [SerializeField] private InputField SpeedDash = null;
    [SerializeField] private Slider NbJump = null;
    //---------------------------------------------
    [SerializeField] private InputField FrictionX = null;
    [SerializeField] private InputField Gravity = null;
    [SerializeField] private InputField WallFriction = null;
    [SerializeField] private InputField WallGravity = null;
    [SerializeField] private UIConvertSliderToInvert WallAirControl = null;
    [SerializeField] private Slider Inertia = null;
    //---------------------------------------------
    [SerializeField] private InputField ToleranceReplacement = null;
    
    [SerializeField] private bool defaultAudioFeedBack = false;
    [SerializeField] private bool defaultVisualFeedBack = true;
    //---------------------------------------------
    [SerializeField] private float defaultSpeedXMax = 7;
    [SerializeField] private float defaultSpeedYMin = 15;
    [SerializeField] private float defaultSpeedJump = 15;
    [SerializeField] private float defaultSpeedWallJump = 15;
    [SerializeField] private float defaultSpeedDash = 15;
    [SerializeField] private int defaultNbJump = 2;
    //---------------------------------------------
    [SerializeField] private float defaultFrictionX = 0.3f;
    [SerializeField] private float defaultGravity = 0.5f;
    [SerializeField] private float defaultWallFriction = 0.5f;
    [SerializeField] private float defaultWallGravity = 0.1f;
    [SerializeField] private float defaultWallAirControl = 1;
    [SerializeField] private float defaultInertia = 0;
    //---------------------------------------------
    [SerializeField] private float defaultToleranceReplacement = 0.01f;


    [Header("Input")]
    [SerializeField] private UIControllerManager ControllerManager = null;
    [SerializeField] private Dropdown MoveLatDropdown = null;
    [SerializeField] private Dropdown JumpDropdown = null;
    [SerializeField] private Dropdown DashDropdown = null;
    [SerializeField] private Dropdown RespawnDropdown = null;
    [SerializeField] private int defaultMoveLatInput = 0;
    [SerializeField] private int defaultJumpInput = 0;
    [SerializeField] private int defaultDashInput = 1;
    [SerializeField] private int defaultRespawnInput = 3;

    [Header("Settings")]
    [SerializeField] private UITerrainManager TerrainManager = null;
    [SerializeField] private InputField IceInertia = null;
    [SerializeField] private InputField SlimeSpeedMultiplier = null;
    [SerializeField] private InputField SlimeNullSpeed = null;
    [SerializeField] private InputField PicDetection = null;
    [SerializeField] private float defaultIceInertia = 0.97f;
    [SerializeField] private float defaultSlimeSpeedMultiplier = 2;
    [SerializeField] private float defaultSlimeNullSpeed = 10;
    [SerializeField] private float defaultPicDetection = 0.1f;


    private void Start()
    {
        if (PlayerPrefs.GetInt("PlayerPrefCreated") == 0)
        {
            RestoreDefaultInput();
            RestoreDefaultParameter();
            RestoreDefaultTerrain();
            SaveInput();
            SaveParameter();
            SaveTerrain();
            PlayerPrefs.SetInt("PlayerPrefCreated", 1);
        }
    }

    // ==========================================================
    #region ==================== Parameter Settings =========================

    public void RestoreDefaultParameter()
    {
        AudioFeedBack.isOn = defaultAudioFeedBack;
        ParameterManager.SetAudioFeedBack(defaultAudioFeedBack);
        VisualFeedBack.isOn = defaultVisualFeedBack;
        ParameterManager.SetVisualFeedBack(defaultVisualFeedBack);

        SpeedXMax.text = defaultSpeedXMax.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetMaxSpeedX(defaultSpeedXMax);
        SpeedYMin.text = defaultSpeedYMin.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetMinSpeedY(defaultSpeedYMin);
        SpeedJump.text = defaultSpeedJump.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetJumpSpeed(defaultSpeedJump);
        SpeedWallJump.text = defaultSpeedWallJump.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetJumpWallSpeedX(defaultSpeedWallJump);
        SpeedDash.text = defaultSpeedDash.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetDashSpeedX(defaultSpeedDash);
        NbJump.value = defaultNbJump;
        ParameterManager.SetJumpNumber(defaultNbJump);

        FrictionX.text = defaultFrictionX.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetFrictionX(defaultFrictionX);
        Gravity.text = defaultGravity.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetGravity(defaultGravity);
        WallFriction.text = defaultWallFriction.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetWallFriction(defaultWallFriction);
        WallGravity.text = defaultWallGravity.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetWallJumpAirFriction(defaultWallGravity);
        WallAirControl.SetConvertedValue(defaultWallAirControl);
        ParameterManager.SetWallAirControl(defaultWallAirControl);
        Inertia.value = defaultInertia;
        ParameterManager.SetInertia(defaultInertia);

        ToleranceReplacement.text = defaultToleranceReplacement.ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetDetectionTolerance(defaultToleranceReplacement);
    }
    public void LoadParameter()
    {
        AudioFeedBack.isOn = PlayerPrefs.GetInt("AudioFeedBack") != 0;
        ParameterManager.SetAudioFeedBack(PlayerPrefs.GetInt("AudioFeedBack") != 0);
        VisualFeedBack.isOn = PlayerPrefs.GetInt("VisualFeedBack") != 0; ;
        ParameterManager.SetVisualFeedBack(PlayerPrefs.GetInt("VisualFeedBack") != 0);

        SpeedXMax.text = PlayerPrefs.GetFloat("SpeedXMax").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetMaxSpeedX(PlayerPrefs.GetFloat("SpeedXMax"));
        SpeedYMin.text = PlayerPrefs.GetFloat("SpeedYMin").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetMinSpeedY(PlayerPrefs.GetFloat("SpeedYMin"));
        SpeedJump.text = PlayerPrefs.GetFloat("SpeedJump").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetJumpSpeed(PlayerPrefs.GetFloat("SpeedJump"));
        SpeedWallJump.text = PlayerPrefs.GetFloat("SpeedWallJump").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetJumpWallSpeedX(PlayerPrefs.GetFloat("SpeedWallJump"));
        SpeedDash.text = PlayerPrefs.GetFloat("SpeedDash").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetDashSpeedX(PlayerPrefs.GetFloat("SpeedDash"));
        NbJump.value = PlayerPrefs.GetInt("NbJump");
        ParameterManager.SetJumpNumber(PlayerPrefs.GetInt("NbJump"));

        FrictionX.text = PlayerPrefs.GetFloat("FrictionX").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetFrictionX(PlayerPrefs.GetFloat("FrictionX"));
        Gravity.text = PlayerPrefs.GetFloat("Gravity").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetGravity(PlayerPrefs.GetFloat("Gravity"));
        WallFriction.text = PlayerPrefs.GetFloat("WallFriction").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetWallFriction(PlayerPrefs.GetFloat("WallFriction"));
        WallGravity.text = PlayerPrefs.GetFloat("WallGravity").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetWallJumpAirFriction(PlayerPrefs.GetFloat("WallGravity"));
        WallAirControl.SetConvertedValue(PlayerPrefs.GetFloat("WallAirControl"));
        ParameterManager.SetWallAirControl(PlayerPrefs.GetFloat("WallAirControl"));
        Inertia.value = PlayerPrefs.GetFloat("Inertia");
        ParameterManager.SetInertia(PlayerPrefs.GetFloat("Inertia"));

        ToleranceReplacement.text = PlayerPrefs.GetFloat("ToleranceReplacement").ToString(CultureInfo.InvariantCulture.NumberFormat);
        ParameterManager.SetDetectionTolerance(PlayerPrefs.GetFloat("ToleranceReplacement"));
    }
    public void SaveParameter()
    {
        PlayerPrefs.SetInt("AudioFeedBack", AudioFeedBack.isOn ? 1 : 0);
        PlayerPrefs.SetInt("VisualFeedBack", VisualFeedBack.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("SpeedXMax", float.Parse(SpeedXMax.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("SpeedYMin", float.Parse(SpeedYMin.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("SpeedJump", float.Parse(SpeedJump.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("SpeedWallJump", float.Parse(SpeedWallJump.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("SpeedDash", float.Parse(SpeedDash.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetInt("NbJump", (int)NbJump.value);
        PlayerPrefs.SetFloat("FrictionX", float.Parse(FrictionX.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("Gravity", float.Parse(Gravity.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("WallFriction", float.Parse(WallFriction.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("WallGravity", float.Parse(WallGravity.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("WallAirControl", WallAirControl.GetConvertedValue());
        PlayerPrefs.SetFloat("Inertia", Inertia.value);
        PlayerPrefs.SetFloat("ToleranceReplacement", float.Parse(ToleranceReplacement.text, CultureInfo.InvariantCulture.NumberFormat));
    }
    #endregion


    // ==========================================================
    #region ==================== Input Settings =========================

    public void RestoreDefaultInput()
    {
        MoveLatDropdown.value = defaultMoveLatInput;
        ControllerManager.SetHorizInput(defaultMoveLatInput);
        JumpDropdown.value = defaultJumpInput;
        ControllerManager.SetJumpInput(defaultJumpInput);
        DashDropdown.value = defaultDashInput;
        ControllerManager.SetDashInput(defaultDashInput);
        RespawnDropdown.value = defaultRespawnInput;
        ControllerManager.SetRespawnInput(defaultRespawnInput);
    }
    public void LoadInput()
    {
        MoveLatDropdown.value = PlayerPrefs.GetInt("MoveLatInput");
        ControllerManager.SetHorizInput(PlayerPrefs.GetInt("MoveLatInput"));
        JumpDropdown.value = PlayerPrefs.GetInt("JumpInput");
        ControllerManager.SetJumpInput(PlayerPrefs.GetInt("JumpInput"));
        DashDropdown.value = PlayerPrefs.GetInt("DashInput");
        ControllerManager.SetDashInput(PlayerPrefs.GetInt("DashInput"));
        RespawnDropdown.value = PlayerPrefs.GetInt("RespawnInput");
        ControllerManager.SetRespawnInput(PlayerPrefs.GetInt("RespawnInput"));
    }
    public void SaveInput()
    {
        PlayerPrefs.SetInt("MoveLatInput", MoveLatDropdown.value);
        PlayerPrefs.SetInt("JumpInput", JumpDropdown.value);
        PlayerPrefs.SetInt("DashInput", DashDropdown.value);
        PlayerPrefs.SetInt("RespawnInput", RespawnDropdown.value);
    }
    #endregion


    // ==========================================================
    #region ==================== Terrain Settings =========================

    public void RestoreDefaultTerrain()
    {
        IceInertia.text = defaultIceInertia.ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetIceInertia(defaultIceInertia);
        SlimeSpeedMultiplier.text = defaultSlimeSpeedMultiplier.ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetBounceMultiplier(defaultSlimeSpeedMultiplier);
        SlimeNullSpeed.text = defaultSlimeNullSpeed.ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetBounceNullSpeed(defaultSlimeNullSpeed);
        PicDetection.text = defaultPicDetection.ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetKillerTolerance(defaultPicDetection);
    }
    public void LoadTerrain()
    {
        IceInertia.text = PlayerPrefs.GetFloat("IceInertia").ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetIceInertia(PlayerPrefs.GetFloat("IceInertia"));
        SlimeSpeedMultiplier.text = PlayerPrefs.GetFloat("SlimeSpeedMultiplier").ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetBounceMultiplier(PlayerPrefs.GetFloat("SlimeSpeedMultiplier"));
        SlimeNullSpeed.text = PlayerPrefs.GetFloat("SlimeNullSpeed").ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetBounceNullSpeed(PlayerPrefs.GetFloat("SlimeNullSpeed"));
        PicDetection.text = PlayerPrefs.GetFloat("PicDetection").ToString(CultureInfo.InvariantCulture.NumberFormat);
        TerrainManager.SetKillerTolerance(PlayerPrefs.GetFloat("PicDetection"));
    }
    public void SaveTerrain()
    {
        PlayerPrefs.SetFloat("IceInertia", float.Parse(IceInertia.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("SlimeSpeedMultiplier", float.Parse(SlimeSpeedMultiplier.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("SlimeNullSpeed", float.Parse(SlimeNullSpeed.text, CultureInfo.InvariantCulture.NumberFormat));
        PlayerPrefs.SetFloat("PicDetection", float.Parse(PicDetection.text, CultureInfo.InvariantCulture.NumberFormat));
    }
    #endregion

}
