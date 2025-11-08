using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lose : Sounds
{
    public Animator deathAni;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.gameObject.layer == 7)
        {
            triggered = true;
            PlaySound(0, 0.4f);
            deathAni.Play("death");
        }
    }
}
