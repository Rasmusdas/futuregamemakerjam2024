using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BombBall : Ball
{
    public float blowUpTimer;
    public float aoe;
    public bool debug = true;

    [SerializeField]private ParticleSystem _blowUpParticleSystem;
    [SerializeField]private ParticleSystem _fuseParticleSystem;
    
    
    public override void Shoot(GameObject shooter, Vector3 dir, float chargeModifier)
    {
        StartCoroutine(BlowTimerUp());
        _fuseParticleSystem.Play();
        base.Shoot(shooter, dir, chargeModifier);
    }

    IEnumerator BlowTimerUp()
    {
        yield return new WaitForSeconds(blowUpTimer);
        BlowUp();
    }

    void BlowUp()
    {
        if (debug)
        {
            var gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gb.transform.position = transform.position;
            var tempVec = Vector3.one * aoe;
            tempVec.y = 0.1f;
            gb.transform.localScale = tempVec;
        }
        _blowUpParticleSystem.transform.parent = null;
        _blowUpParticleSystem.Play();

        var hits = Physics.SphereCastAll(transform.position, aoe / 2, Vector3.up, 0.02f);

        foreach (var hit in hits)
        {
            if (hit.transform.TryGetComponent(out PlayerController player))
            {
                player.HitPlayer();
            }
        }
//        soundManager.ExplosionSound();
        Destroy(_blowUpParticleSystem,2);
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!fired) return;
        if (other.gameObject.CompareTag("Floor")) return;
        
        
        BlowUp();
    }
}
