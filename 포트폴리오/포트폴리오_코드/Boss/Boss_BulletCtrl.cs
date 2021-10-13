using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_BulletCtrl : MonoBehaviour
{
    public GameObject[] EffectsOnCollision;
    public float DestroyTimeDelay = 3;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private int damage;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        damage = Random.Range(6, 8);
    }
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            foreach (var effect in EffectsOnCollision)
            {
                var instance = Pool.Instance.Spawn("Boss_BulletHit", collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;
                if (!UseWorldSpacePosition) instance.transform.parent = transform;
                if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
                else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else
                {
                    instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                    instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                }
                Pool.Instance.Despawn(instance, DestroyTimeDelay);
            }
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Playerctrl>().TakePlayerDamage(damage);
            if (other.GetComponent<Playerctrl>().currentplayerHP <= 0)
            {
                other.GetComponent<Playerctrl>().OnDie();
            }
        }
    }
}
