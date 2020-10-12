using UnityEngine;

public class UIParameterManager : MonoBehaviour
{
    private bool menuOpen = false;
    [SerializeField] Animator animator = null;

    [SerializeField] PlayerMovement player = null;
    [SerializeField] Wind wind = null;

    
    // Open/Close Menu
    public void ToggleOpenMenu()
    {
        menuOpen = !menuOpen;
        animator.SetBool("Open", menuOpen);
    }

    // On wind Slider Change
    public void SetWindX(float _windForce)
    {
        wind.SetWindX(_windForce/5, player);
    }
    public void SetWindY(float _windForce)
    {
        wind.SetWindY(_windForce/5, player);
    }

}
