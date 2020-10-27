using UnityEngine;

public class UIMenuOpenable : MonoBehaviour
{
    private bool menuOpen = false;
    Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }



    // =============== Open/Close Menu =====================
    public void ToggleOpenMenu()
    {
        menuOpen = !menuOpen;
        animator.SetBool("Open", menuOpen);
    }

}
