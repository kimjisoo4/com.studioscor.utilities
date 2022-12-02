using UnityEngine;


namespace StudioScor.GameplayQueSystem
{
    [CreateAssetMenu(menuName = "GameplayQue/QueFX/new VFX", fileName = "QFX_")]
    public class QueVFX : QueFX
    {
        [SerializeField] private ParticleSystem _Effect;

        public override void PlayQue(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            ParticleSystem effect = Instantiate(_Effect, position, rotation);

            effect.transform.localScale = scale;

            effect.Play();
        }
    }
}
 