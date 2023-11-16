using UnityEngine;
public enum ParticleEffects { Pick , Connect , ConnectReject }
public class ParticleManager : Singleton<ParticleManager>
{
    [Header("References")]
    [SerializeField] private GameObject objectPickParticle;
    [SerializeField] private GameObject connectRejectParticle;
    [SerializeField] private GameObject connectParticle;

    public void PlayParticleEffect(ParticleEffects effectType, Vector3 targetPos)
    {
        GameObject pObject = effectType switch
        {
            ParticleEffects.Pick => objectPickParticle,
            ParticleEffects.Connect => connectParticle,
            ParticleEffects.ConnectReject => connectRejectParticle,
            _ => throw new System.NotImplementedException(),
        };
        pObject.gameObject.SetActive(true);
        pObject.transform.position = targetPos;
        pObject.GetComponent<ParticleSystem>().Play();
    }
}
