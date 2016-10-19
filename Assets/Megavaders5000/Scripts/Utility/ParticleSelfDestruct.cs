using UnityEngine;

/* Destroy a particle system at the end of its expected lifetime.
 *  
 */ 
public class ParticleSelfDestruct : MonoBehaviour 
{
    void Start()
    {
		ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
		if ( particleSystem != null )
        {
			GameObject.Destroy(gameObject, particleSystem.duration + particleSystem.startLifetime);
        }
    }
}
