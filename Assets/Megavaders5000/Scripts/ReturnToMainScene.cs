using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class ReturnToMainScene : MonoBehaviour 
{
	[SerializeField]
	float RestartDelay = 1.0f;	// Set this value either here or in the inspector window


	// Use this for initialization
	void Start () 
	{
		// kill the game logic component..
		GameObject go = GameObject.Find("_GameLogic");
		if( go )
			Destroy( go );
		

		// Return Player to START SCREEN in RESTARTDELAY seconds
		Invoke( "ReturnToMain", RestartDelay );

	}


	public void ReturnToMain()
	{

		SceneManager.LoadScene("GameStart");	
	}

	
	// Update is called once per frame
	void Update () 
	{
	
	}

}
