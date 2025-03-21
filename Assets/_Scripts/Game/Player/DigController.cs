using System.Collections.Generic;
using ScriptBoy.DiggableTerrains2D;
using UnityEngine;

public class DigController : MonoBehaviour
{
    [Header("Weapons")]
    public GameObject sideScythe;
    public GameObject downScythe;

    [Header("Dig Settings")]
    [SerializeField] private Shovel m_Shovel;
    [SerializeField] private ParticleSystem m_ShovelParticleSystem;
    [SerializeField] private Shovel m_Pickaxe;
    [SerializeField] private ParticleSystem m_PickaxeParticleSystem;

    private bool isDemo;

    // Initiates digging with the shovel and emits particles
    private void DigByShovel()
    {
        DigAndEmitParticles(m_Shovel, m_ShovelParticleSystem);
    }

    // Initiates digging with the pickaxe and emits particles
    private void DigByPickaxe()
    {
        DigAndEmitParticles(m_Pickaxe, m_PickaxeParticleSystem);
    }

    // Handles the digging and particle emission logic for both shovel and pickaxe
    private void DigAndEmitParticles(Shovel shovel, ParticleSystem particleSystem)
    {
        if (isDemo)
        {
            EmitParticlesInDemo(shovel, particleSystem);
        }
        else
        {
            EmitParticlesInNormalMode(shovel, particleSystem);
        }
    }

    // Handles particle emission in demo mode
    private void EmitParticlesInDemo(Shovel shovel, ParticleSystem particleSystem)
    {
        List<TerrainParticle> particles = new List<TerrainParticle>();
        TerrainParticleUtility.GetParticles(shovel, particles, 200);

        if (shovel.Dig(out float diggedArea))
        {
            Vector3 velocity = particleSystem.transform.right * particleSystem.transform.lossyScale.x * 10;

            foreach (var particle in particles)
            {
                ParticleSystem.EmitParams emit = new ParticleSystem.EmitParams
                {
                    position = particle.position,
                    startColor = particle.color,
                    velocity = velocity
                };
                particleSystem.Emit(emit, 1);
            }

            if (diggedArea > 2) GameManager.Instance.playerSound.DigSound();
        }
    }

    // Handles particle emission in normal mode
    private void EmitParticlesInNormalMode(Shovel shovel, ParticleSystem particleSystem)
    {
        if (shovel.Dig(out float diggedArea))
        {
            int particleCount = (int)(100f * Mathf.InverseLerp(2, 15, diggedArea));
            particleSystem.Emit(particleCount);

            if (diggedArea > 2) GameManager.Instance.playerSound.DigSound();
        }
    }

    // Deactivates both scythes and sets the player digging state to false
    public void SetFalseScythe()
    {
        sideScythe.SetActive(false);
        downScythe.SetActive(false);
        GameManager.Instance.playerController.isDigging = false;
    }

    // Activates the appropriate scythe based on the side parameter
    public void SetTrueScythe(int side)
    {
        GameManager.Instance.playerController.isDigging = true;
        
        if (side == 0)
        {
            sideScythe.SetActive(true);
        }
        else if (side == 1)
        {
            downScythe.SetActive(true);
        }
    }
}
