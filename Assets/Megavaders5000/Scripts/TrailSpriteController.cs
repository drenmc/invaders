using UnityEngine;
using System.Collections;

public class TrailSpriteController : MonoBehaviour 
{

    SpriteRenderer sprite;
	// Use this for initialization
	void Start () 
    {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Vector3 scale = transform.localScale;

        scale.x -= (0.2f * Time.deltaTime);
        scale.y -= (0.2f * Time.deltaTime);

        transform.localScale = scale;

        Color a = sprite.color;
        a.a -= 2.5f * Time.deltaTime;
        sprite.color = a;

        if (a.a < 0.01f)
            Destroy(gameObject);


	}
}
