using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Movement")]
    public AudioClip footstepClip;
    [Tooltip("Minimum time between footsteps")]
    public float footstepCooldown = 0.25f;

    [Header("Jump & Land")]
    public AudioClip jumpClip;
    public AudioClip doubleJumpClip;
    public AudioClip landClip;

    [Header("Combat")]
    public AudioClip attackClip;
    public AudioClip hurtClip;

    [Header("Other")]
    public AudioClip rollClip;
    public AudioClip deathClip;

    private float nextFootstepTime = 0f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlayFootstep()
    {
        if (footstepClip == null) return;
        if (Time.time < nextFootstepTime) return;

        PlayOneShot(footstepClip);
        nextFootstepTime = Time.time + footstepCooldown;
    }

    public void PlayJump()
    {
        PlayOneShot(jumpClip);
    }

    public void PlayDoubleJump()
    {
        if (doubleJumpClip != null)
            PlayOneShot(doubleJumpClip);
        else
            PlayOneShot(jumpClip);
    }

    public void PlayLand()
    {
        PlayOneShot(landClip);
    }

    public void PlayRoll()
    {
        PlayOneShot(rollClip);
    }

    public void PlayAttack()
    {
        PlayOneShot(attackClip);
    }

    public void PlayHurt()
    {
        PlayOneShot(hurtClip);
    }

    public void PlayDeath()
    {
        PlayOneShot(deathClip);
    }


    private void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clip);
        }
        else
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
