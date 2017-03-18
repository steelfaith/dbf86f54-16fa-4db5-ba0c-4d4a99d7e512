using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Infrastructure;
using System.Collections;

namespace Assets.Scripts
{
    public class AnimationController : MonoBehaviour
    {
        private MonsterCave monsterCave;

        private void Start()
        {
            monsterCave = MonsterCave.Instance();
        }

        private static AnimationController animationController;
        public static AnimationController Instance()
        {
            if (!animationController)
            {
                animationController = FindObjectOfType(typeof(AnimationController)) as AnimationController;
                if (!animationController)
                    Debug.LogError("Could not find AnimationController");
            }
            return animationController;
        }

        public void PlayAnimation(GameObject monster, AnimationAction action)
        {
            var anim = monster.GetComponent<Animator>();
            if (anim == null)
            {
                //older assets might not have animator, just animation
                PlayAnimationLegacy(monster, action);
                return;
            }
            var info = monster.GetComponent<BaseCreature>();
            anim.Play(monsterCave.TryGetAnimationName(info.NameKey,action));
        }

        private void PlayAnimationLegacy(GameObject monster, AnimationAction action)
        {
            var anim = monster.GetComponent<Animation>();
            var info = monster.GetComponent<BaseCreature>();
            anim.Play(monsterCave.TryGetAnimationName(info.NameKey, action));
        }

        public void PlayAnimationWithWait(GameObject monster, AnimationAction action)
        {
            StartCoroutine(DoAnimation(monster, action));
        }

        private IEnumerator WaitForAnimation(Animation animation)
        {
            do
            {
                yield return null;
            } while (animation.isPlaying);
        }

        IEnumerator DoAnimation(GameObject monster, AnimationAction action)
        {
            var anim = monster.GetComponent<Animation>();
            if (anim == null) yield return null ;
            var info = monster.GetComponent<BaseCreature>();
            anim.CrossFade(monsterCave.TryGetAnimationName(info.NameKey, action));
            yield return new WaitForSeconds(2f); // wait for two seconds.
        }
    }
}
