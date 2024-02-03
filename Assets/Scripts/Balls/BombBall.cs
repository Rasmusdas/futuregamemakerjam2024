using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : Ball
{
    public float blowUpTimer;
    public float aoe;
    public bool debug = true;

    [SerializeField]private ParticleSystem _particleSystem;
    public override void Shoot(GameObject shooter, Vector3 dir, float chargeModifier)
    {
        StartCoroutine(BlowUp());
        base.Shoot(shooter, dir, chargeModifier);
    }

    IEnumerator BlowUp()
    {
        yield return new WaitForSeconds(blowUpTimer);
        
        // Used to check AOE radius to see if it somewhat lines up with the particle system
        if (debug)
        {
            var gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gb.transform.position = transform.position;
            var tempVec = Vector3.one * aoe;
            tempVec.y = 0.1f;
            gb.transform.localScale = tempVec;
        }
        _particleSystem.transform.parent = null;
        _particleSystem.Play();

        var hits = Physics.SphereCastAll(transform.position, aoe / 2, Vector3.up, 0.02f);

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out PlayerController player))
            {
                player.HitPlayer();
            }
        }
        Destroy(_particleSystem,2);
        Destroy(gameObject);
    }
}
