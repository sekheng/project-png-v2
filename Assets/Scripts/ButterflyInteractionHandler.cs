using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ButterflyInteractionHandler : AnimalInteractionHandler
{
    public SkinnedMeshRenderer[] meshes;
    private ITweenMagic tween;

    public ParticleSystem particle;

    private void Awake()
    {
        tween = GetComponent<ITweenMagic>();

        clickEvent.AddListener(OnButterflyClicked);
    }

    private void OnButterflyClicked()
    {
        particle.Play();
        //if (TimeManager.Instance.IsDay)
        if (SunHandler.Instance.isDay)
        {
            AudioManager.Instance.PlaySound("Day");
            if (transform.localScale.x == 1)
                tween.PlayForwardScale();
            else
                tween.PlayReverseScale();
        }
        else
        {
            AudioManager.Instance.PlaySound("Night");
            foreach (var mesh in meshes)
            {
                mesh.GetComponent<Animator>().SetBool("Emit", !mesh.GetComponent<Animator>().GetBool("Emit"));
            }
        }
    }

    public override void SetTexture(Texture2D texture)
    {
        foreach (var mesh in meshes)
        {
            Debug.LogWarning("Setting texture: "+mesh.gameObject.name);
            mesh.material.mainTexture = texture;
            mesh.material.SetFloat("EmissionStrength", 1+ PrefsHandler.butterflyIlluminationStrength / 10.0f);
        }
    }

    public void OnButterflyOutComplete()
    {
        var sc = gameObject.AddComponent<SchoolChild>();
        sc._spawner = SchoolController.Instance;
    }
}