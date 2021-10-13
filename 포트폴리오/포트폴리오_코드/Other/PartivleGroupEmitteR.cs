using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartivleGroupEmitteR : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private RendererFadE[] FadeRenderers;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private int countMultiplier = 1;
    [SerializeField] private bool emissionEnabled = false;

    public float SimulationSpeed
    {
        get
        {
            return simulationSpeed;
        }
        set
        {
            simulationSpeed = value;
            foreach (var ps in particleSystems)
            {
                var main = ps.main;
                main.simulationSpeed = simulationSpeed;
            }
        }
    }

    private float simulationSpeed = 1f;

    private void OnEnable()
    {
        EnableEmission(emissionEnabled);
    }

    public void Emit(int count)
    {
        foreach (var ps in particleSystems)
        {
            if (ps.main.startDelay.constant == 0)
                Emit(ps, count * countMultiplier);
            else
                StartCoroutine(PlayDelayed(ps, count));
        }
    }

    public void EnableEmission(bool enabled)
    {
        foreach (var ps in particleSystems)
        {
            var emission = ps.emission;
            emission.enabled = enabled;
        }
    }

    public void Show()
    {
        if (FadeRenderers != null)
        {
            foreach (var fade in FadeRenderers)
            {
                if (fade)
                    fade.SHOW(_fadeDuration);
            }
        }
    }

    public void Fade()
    {
        if (FadeRenderers != null)
        {
            foreach (var fade in FadeRenderers)
            {
                if (fade)
                    fade.FADE(_fadeDuration);
            }
        }
    }

    public void ClearParticles()
    {
        foreach (var ps in particleSystems)
        {
            ps.Clear();
        }
    }

    private IEnumerator PlayDelayed(ParticleSystem particleSystem, int count)
    {
        yield return new WaitForSeconds(particleSystem.main.startDelay.constant);
        Emit(particleSystem, count);
    }

    private void Emit(ParticleSystem particleSystem, int count)
    {
        if (particleSystem.emission.burstCount == 0)
        {
            particleSystem.Emit(count * countMultiplier);
        }
        else
        {
            var burst = particleSystem.emission.GetBurst(0);
            switch (burst.count.mode)
            {
                case ParticleSystemCurveMode.Constant:
                    particleSystem.Emit((int)burst.count.constant * countMultiplier * count);
                    break;
                case ParticleSystemCurveMode.TwoConstants:
                    particleSystem.Emit((int)Random.Range(burst.count.constantMin, burst.count.constantMax) * countMultiplier * count);
                    break;
            }
        }
    }
}