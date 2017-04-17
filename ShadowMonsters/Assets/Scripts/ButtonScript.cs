using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Infrastructure;
using System;

namespace Assets.Scripts
{
    public class ButtonScript : MonoBehaviour
    {

        public Button button;
        public Image cooldownImage;
        public Color startColor;
        public Color endColor;
        public Image castTimeImage;
        public Image castGlowImage;
        public Color castTimeStartColor;
        public Color castTimeEndColor;
        public bool IsCasting;
        public int attackIndex;
        public Button powerUpButton;
        public Button powerDownButton;
        public PowerUpLevels attackPower;
        private float rechargeEnd;
        private float rechargeTime;
        public AttackInfo attackInfo;
        private FatbicDisplayController fatbic;
        private bool onGlobalCooldown;
        public event EventHandler<DataEventArgs<AttackInfo>> AttackAttempt;
        float nextSecond = 0;

        private void Update()
        {

            if (!IsCasting && !onGlobalCooldown) return;
            if(rechargeEnd <= Time.time)
            {                
                EndCooldown();
                EndCastTime();
                IsCasting = false;
                fatbic.IsBusy = false; 
                return;
            }
            if(onGlobalCooldown || attackInfo.DamageStyle == DamageStyle.Instant || attackInfo.DamageStyle == DamageStyle.Tick)
            {
                CoolDownTick();
            }
            else
            {
                CastTimeTick();
            }
            
        }

        private void Awake()
        {
            fatbic = FatbicDisplayController.Instance();
        }

        public void InitializeButton()
        {
            //TODO: only the first 5 hold attacks.  figure out if bond, run, and stop attack should be attacks or something different
            if (attackIndex > 4) return;
            fatbic.RegisterAttackButton(this, attackIndex);
            attackInfo = fatbic.GetAttackInformation(attackIndex);
            button.image.sprite = attackInfo.Icon;
            ProcessAttackInfo();            
        }

        public void StartButtonAction(float timeout)
        {            
            if (onGlobalCooldown) return;

            castGlowImage.enabled = true;
            IsCasting = true;
            if (attackInfo.DamageStyle == DamageStyle.Instant || attackInfo.DamageStyle == DamageStyle.Tick)
            {
                StartCooldown(timeout);                
            }
            else
            {
                StartCastTime(timeout);                
            }
        }

        public void StartCooldown(float recharge)
        {
            if (!button.enabled && recharge <= (rechargeEnd -Time.time)) return;
            
            button.enabled = false;
            rechargeTime = recharge;
            rechargeEnd = Time.time + rechargeTime;
            cooldownImage.fillAmount = 1.0f;
        }

        public void StartGlobalCooldown(float recharge)
        {
            onGlobalCooldown = true;
            StartCooldown(recharge);
        }


        private void StartCastTime(float time)
        {
            if (button.enabled = false && time <= (rechargeEnd - Time.time)) return;
            button.enabled = false;
            rechargeTime = time;
            rechargeEnd = Time.time + rechargeTime;
            castTimeImage.fillAmount = 1.0f;
            
        }

        public void CoolDownTick()
        {
            if (cooldownImage == null || !IsCasting && !onGlobalCooldown) return;
            cooldownImage.fillAmount = (rechargeEnd - Time.time) / rechargeTime;
            cooldownImage.color = Color.Lerp(startColor, endColor, 1 - cooldownImage.fillAmount);
            if(attackInfo.DamageStyle == DamageStyle.Tick && IsCasting)
            {
                if (Time.time >= nextSecond)
                {
                    nextSecond = Mathf.FloorToInt(Time.time) + 1;
                }
            }
        }

        public void CastTimeTick()
        {
            if (castTimeImage == null || !IsCasting && !onGlobalCooldown) return;
            castTimeImage.fillAmount = (rechargeEnd - Time.time) / rechargeTime; ;
            castTimeImage.color = Color.Lerp(castTimeStartColor, castTimeEndColor, 1 - castTimeImage.fillAmount);

        }

        public void EndCooldown()
        {
            if (cooldownImage == null) return;
            cooldownImage.fillAmount = 0.0f;
            button.enabled = true;
            castGlowImage.enabled = false;
            onGlobalCooldown = false;
        }

        public void EndCastTime()
        {
            if (castTimeImage == null || !IsCasting) return;
            castTimeImage.fillAmount = 0.0f;
            castGlowImage.enabled = false;
            button.enabled = true;
        }

        private void ProcessAttackInfo()
        {
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = attackInfo.Name;
            SetButtonColors();
            castGlowImage.color = button.image.color;
        }

        private void SetButtonColors()
        {
            var buttonImage = button.GetComponent<Image>();
            var backgroundColor = attackInfo.Affinity.GetColorFromMonsterAffinity();
            buttonImage.color = backgroundColor;
            var text = button.GetComponentInChildren<Text>();
        }

    }
}
