using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	[Header("Sound")]

	[SerializeField] float delayForMusic = 5f;
	[SerializeField] float delayInSeconds = 2f;
	[SerializeField] AudioSource menu_music;
	
	
	//Setup Sound Effects in the opening scene 
	[SerializeField] AudioSource lappingStream;
	[SerializeField] AudioSource birdsAndWind;

	//[SerializeField] AudioClip correct;
	[SerializeField] AudioSource correctSFX;

	
	[Header ("Controls")]
	[SerializeField] bool startGame = false;
	[SerializeField] bool onlyOnce = false;

	
	
	
	void Start()
	{
		

		SetupSFX(); 
		PlayMusic();
	}

	
	private void SetupSFX()
    {
		lappingStream.loop = true;
		lappingStream.Play();

		birdsAndWind.loop = false;
		birdsAndWind.Play();
    }






	public void PlayMusic()
	{
		StartCoroutine(WaitThenPlay());
	}

	IEnumerator WaitThenPlay()
	{
		yield return new WaitForSeconds(delayForMusic);
		menu_music.loop = true;
		//menu_music.Play();

		StartCoroutine(fadeSound());
	}

	IEnumerator fadeSound ()
    {
		
		//Music fades up, 
		while (menu_music.volume < 0.3f)
        {
			menu_music.volume += Time.deltaTime / 10.0f;
			yield return null;
        }


    }

	
	

	// Controls and loading
	private void CheckInput()
	{
		startGame = Input.GetButtonDown("Submit");
		
		if (Input.GetButtonDown("Submit") && (!onlyOnce))
        {
			StopFXAndMusic();
		}
	}


	private void StopFXAndMusic()
    {
		correctSFX.Play();
		lappingStream.Stop();
		birdsAndWind.Stop();
		menu_music.Stop();

	}



	private void StartTheGame()
	{
		if (startGame)
		{

			
			
			//Debug.Log("Do this once");
			onlyOnce = true;
			


		}

		if (onlyOnce)
		{
			

			LoadLevel();

		}

	}










	public void LoadLevel()
	{
		StartCoroutine(WaitAndLoad());
	
	
	}

	IEnumerator WaitAndLoad()
	{
		

		yield return new WaitForSeconds(delayInSeconds);
		SceneManager.LoadScene(1);

	}
	
	
	
	
	
	
	
	// Update is called once per frame
	void Update () 
	{
		CheckInput();
		StartTheGame();
	}
}
