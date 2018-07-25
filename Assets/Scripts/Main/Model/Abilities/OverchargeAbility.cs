using UnityEngine;

[CreateAssetMenu(fileName = "OverchargeAbility", menuName = "Game data/Abilities/Overcharge")]
public class OverchargeAbility : TriggeredAbility {

    [SerializeField] Effect effect;

    public override Instance CreateInstance (Ship caster) {
        return new OverchargeAbilityInstance(caster, this);
    }

    private class OverchargeAbilityInstance : TriggeredAbilityInstance {


        public override string name { get { return "Overcharge"; } }

        public OverchargeAbilityInstance (Ship caster, OverchargeAbility model) : base(caster, model) {
        }

        protected override void Trigger () {
            caster.status.AddEffect(Instantiate((model as OverchargeAbility).effect));
        }
    }
}