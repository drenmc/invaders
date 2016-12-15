using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour 
{

    public float yvel = 0.5f; // Player missiles fire upwards, invader missiles fire down - default to a player missile
    
    public GameObject Tracer;


//    private float flipit = 0.0f;

    GameObject firedBy;

    float dropTime = 0.0f;

    

	// Use this for initialization
	void Start () 
    {
    //    gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 pos = transform.position;
        pos.y += yvel;
        transform.position = pos;

        if(dropTime <= 0.0f && Tracer != null)
        {
            GameObject trace = Instantiate(Tracer) as GameObject;
            trace.transform.position = pos;
            dropTime = 0.05f;
        }
        dropTime -= Time.deltaTime;


        /*
        flipit += 0.1f;
        if(flipit > 1.0f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
            flipit = 0.0f;

        }
        */
        if (pos.y > 5.2f || pos.y < -5.2f)
        {
            killMissile();
        }
	}

    public void playerFired(GameObject playerObject)
    {
       

        firedBy = playerObject;
    }

    public void enemyFired(GameObject enemy)
    {
		firedBy = enemy;
        // Set up the y velocity..
        yvel = -0.05f;

//        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
			Vector3 pos = enemy.transform.localPosition;

            pos.y -= 0.1f;
            transform.localPosition = pos;

        }

    }

    
    // Using OnTriggerEnter2D - missile rigid body set to kinematic; we're not using the phyics engine collision
    // mechanics.  We simply want to destroy something (enemy, player, shield)
    void OnTriggerEnter2D(Collider2D collidedWith)
    {

        // When Y VEl is > 0, the player fired the one and only allowable player missle.
        // We do not care if an enemy's missile touches another enemy..
        if(collidedWith.tag == "Enemy" && yvel > 0)
        {
            // Destroy enemy, inactivate player missile            
			EnemyController ic = collidedWith.GetComponent<EnemyController>();
            ic.missileHit();
            killMissile();
        }
        // Boom! Player hit. Ouch. 
        else if(collidedWith.tag == "Player" && yvel < 0)
        {
            PlayerController pc = collidedWith.GetComponent<PlayerController>();
            pc.missileHit();
            killMissile();
        }
        else if (collidedWith.tag == "Shield")
        {
        
            collidedWith.gameObject.SetActive(false);
            killMissile();
        }

    }

    private void killMissile()
    {
        if(firedBy != null)
        {
            if(firedBy.tag != "Enemy")
                firedBy.SendMessage("firedMissileDead", SendMessageOptions.DontRequireReceiver     );

            firedBy = null;
            

        }
        Destroy(this.gameObject);

    }
}
