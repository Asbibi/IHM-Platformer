using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    public string horizInput = "Cont_HorizSL";
    public string jumpInput = "Cont_A";
    public string dashInput = "Cont_B";
    public string respawnInput = "Cont_Y";



    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
            playerMovement.MoveX(Input.GetAxis("Horizontal"));
            if(Input.GetButtonDown("DashKey"))
                playerMovement.Dash(Input.GetAxis("Horizontal"));

        else if (Input.GetAxis(horizInput) !=0)
            playerMovement.MoveX(Input.GetAxis(horizInput));
            if(Input.GetButtonDown(dashInput))
                playerMovement.Dash(Input.GetAxis(horizInput));

        if (Input.GetButtonDown(jumpInput) || Input.GetButtonDown("Jump"))
            playerMovement.Jump();
        if (Input.GetButtonDown(respawnInput) || Input.GetButtonDown("RespawnKey"))
            GameManager.RespawnPlayer();

        if (Input.GetButtonDown("Pause"))
        {
            GameManager.Pause();
        }

        
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.SetGravityMultiplier(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerMovement.SetGravityMultiplier(-1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.SetSlowMotion(true);
            Debug.Log("Slow");
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            GameManager.SetSlowMotion(false);
            Debug.Log("Normal");
        }
    }

}
