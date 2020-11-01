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
            if(Input.GetButtonDown("Fire1"))
                playerMovement.Dash(Input.GetAxis("Horizontal"));

        else if (Input.GetAxis(horizInput) !=0)
            playerMovement.MoveX(Input.GetAxis(horizInput));
            if(Input.GetButtonDown(dashInput))
                playerMovement.Dash(Input.GetAxis(horizInput));

        if (Input.GetButtonDown(jumpInput) || Input.GetButtonDown("Jump"))
            playerMovement.Jump();
        if (Input.GetButtonDown(respawnInput) || Input.GetButtonDown("Fire2"))
            GameManager.RespawnPlayer();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Pause();
        }
    }

}
