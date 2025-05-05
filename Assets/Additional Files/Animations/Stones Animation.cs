using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StonesAnimation : MonoBehaviour
{

    [SerializeField] AlvildaController alvildaController;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource turningStones;
    [SerializeField] GameObject spinningTrail;

    [SerializeField] private float delayInSecondsToMove = 2.0f;
    [SerializeField] private float delayInSecondsToLoad = 0.1f;
    

    


    void Awake()
    {

        animator = GetComponent<Animator>();
        turningStones = GetComponent<AudioSource>();

        alvildaController = FindObjectOfType<AlvildaController>();
        spinningTrail.SetActive(false);
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
        
        // Trigger a light or particle effect
        spinningTrail.SetActive(true);

        StartCoroutine(WaitAndLoad());

    }


    IEnumerator WaitAndLoad()
    {
        // Wait so particle effect can finish
        yield return new WaitForSeconds(delayInSecondsToMove);

        // Move Alvilda to a new position between the stones
        alvildaController.AlvildaEndLevel();


        // Wait
        yield return new WaitForSeconds(delayInSecondsToLoad);

        // Load the next available scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }
}
