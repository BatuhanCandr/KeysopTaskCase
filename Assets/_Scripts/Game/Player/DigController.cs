using System.Collections.Generic;
using ScriptBoy.DiggableTerrains2D;
using UnityEngine;
using UnityEngine.Serialization;

public class DigController : MonoBehaviour
{
    
    [Header("Weapons")]
    public GameObject sideScythe;
    public GameObject downScythe;
    
    
    [Header("Dig Settings")]
    [SerializeField] Shovel m_Shovel;
    [SerializeField] ParticleSystem m_ShovelParticleSystem;
    [SerializeField] Shovel m_Pickaxe;
    [SerializeField] ParticleSystem m_PickaxeParticleSystem;
    private bool isDemo;
    void DigByShovel()
    {
        DigAndEmitParticles(m_Shovel, m_ShovelParticleSystem);
    }
    void DigByPickaxe()
    {
        DigAndEmitParticles(m_Pickaxe, m_PickaxeParticleSystem);
    }
    
    void DigAndEmitParticles(Shovel shovel, ParticleSystem particleSystem)
    {
        if (isDemo)
        {
            List<TerrainParticle> particles = new List<TerrainParticle>();
            TerrainParticleUtility.GetParticles(shovel, particles, 200);

            if (shovel.Dig(out float diggedArea))
            {
                Vector3 velocity = particleSystem.transform.right * particleSystem.transform.lossyScale.x * 10;

                for (int i = 0; i < particles.Count; i++)
                {
                    ParticleSystem.EmitParams emit = new ParticleSystem.EmitParams();
                    emit.position = particles[i].position;
                    emit.startColor = particles[i].color;
                    emit.velocity = velocity;
                    particleSystem.Emit(emit, 1);
                }
                    
               // if (diggedArea > 2) PlayDigSound();
            }
        }
        else
        {
            if (shovel.Dig(out float diggedArea))
            {
                int particleCount = (int)(100f * Mathf.InverseLerp(2, 15, diggedArea));
                particleSystem.Emit(particleCount);

               // if (diggedArea > 2) PlayDigSound();
            }
        }
    }

    public void SetFalseScythe()
    {
        sideScythe.SetActive(false);
        downScythe.SetActive(false);
        GameManager.Instance.playerController.isDigging = false;
    }
    public void SetTrueScythe(int side)
    {
        GameManager.Instance.playerController.isDigging = true;
        if (side==0)
        {
            sideScythe.SetActive(true);
        }
        else if (side == 1)
        {
            downScythe.SetActive(true);
        }
        
    }
}
