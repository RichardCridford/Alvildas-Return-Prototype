using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StonesAnimation : MonoBehaviour
{

    [SerializeField] AlvildaController alvildaController;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource turningStones;
    private float delayInSeconds = 10f;

    


    void Awake()
    {

        animator = GetComponent<Animator>();
        turningStones = GetComponent<AudioSource>();

        alvildaController = FindObjectOfType<AlvildaController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        // animator condition for turning the stones
        animator.SetBool("PlayOnce", false);
    }

    
    // This method is triggered by an animation event
    public void TurningStonesSound()
    {
        turningStones.Play();
    }

    
    // This is the method that can be triggered by an Animation Event
    public void WillDoThings()
    {
        // stops the animation of the stones turning looping
        animator.speed = 0;
        
        StartCoroutine(WaitAndLoad());

        // Move Alvilda to a new position between the stones
        alvildaController.AlvildaEndLevel();

        // Trigger a light or particle effect


    }


    IEnumerator WaitAndLoad()
    {

        // Wait
        yield return new WaitForSeconds(delayInSeconds);

        // Load the next available scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }
}
