﻿using System.Collections;

using UnityEngine;

public abstract class HeldAbility : Ability {

    public abstract class HeldAbilityInstance : Instance {

        public override sealed CastType castType { get { return CastType.Held; } }
        public override bool isAvailable { get { return base.isAvailable && coroutine == null; } }

        private Coroutine coroutine;

        public HeldAbilityInstance (Ship caster) : base(caster) {
            // Nothing to do
        }

        public sealed override void Use () {
            if (isAvailable)
                coroutine = caster.StartCoroutine(AbililtyCoroutine());
        }

        public sealed override void Release () {
            if (coroutine != null) {
                caster.StopCoroutine(coroutine);
                coroutine = null;
                OnHoldStop();
            } else
                Debug.Log("Released unused held ability, ignoring");
        }

        private IEnumerator AbililtyCoroutine () {
            OnHoldStart();
            while (true) {
                OnHoldStay();
                yield return null;
            }
        }

        protected abstract void OnHoldStart ();
        protected abstract void OnHoldStay ();
        protected abstract void OnHoldStop ();

    }

}
