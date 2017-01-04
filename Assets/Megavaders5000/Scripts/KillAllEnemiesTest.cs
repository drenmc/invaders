using UnityEngine;
using System;
using System.Collections;

public class KillAllEnemiesTest : MonoBehaviour 
{
	[SerializeField]
	bool EnableAction = false;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if( EnableAction && Input.GetKeyDown( KeyCode.F1 ) )
		{
			
			SceneGamePlayManager sgpm = GetComponent<SceneGamePlayManager>();
			Transform t = sgpm.EnemyContainer;
			for(int cnt = 0; cnt <  t.childCount; cnt ++ )
			{
				Transform childT = t.GetChild( cnt );
				EnemyController ec = childT.GetComponent<EnemyController>();
				if( ec != null )
				{
					sgpm.EnemyHit( ec );
				}



			}
			EnableAction = false;
		}
			
	}
}
