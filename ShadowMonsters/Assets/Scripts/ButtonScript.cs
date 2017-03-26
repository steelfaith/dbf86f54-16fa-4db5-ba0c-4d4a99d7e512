﻿using System.Collections;
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
        public int attackIndex;
        private float rechargeEnd;
        private float rechargeTime;
        public AttackInfo attackInfo;
        private FatbicController fatbic;
        private bool onGlobalCooldown;
        public event EventHandler<DataEventArgs<AttackInfo>> AttackAttempt;
        float nextSecond = 0;

        private void Update()
        {
            if (!attackInfo.IsCasting && !onGlobalCooldown) return;
            if(rechargeEnd <= Time.time)
            {                
                EndCooldown();
                EndCastTime();
                attackInfo.IsCasting = false;
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
            fatbic = FatbicController.Instance();
        }

        public void InitializeButton()
        {
            attackInfo = fatbic.GetAttackInformation(attackIndex);
            button.image.sprite = attackInfo.Icon;
            ProcessAttackInfo();
        }

        public void StartButtonAction()
        {            
            if (onGlobalCooldown) return;
            attackInfo.IsCasting = true;
            fatbic.IsBusy = true;
            castGlowImage.enabled = true;
            if (attackInfo.DamageStyle == DamageStyle.Instant || attackInfo.DamageStyle == DamageStyle.Tick)
            {
                if (attackInfo.DamageStyle == DamageStyle.Instant)
                    FireAttackAttempt();
                StartCooldown(attackInfo.Cooldown);
                fatbic.StartGlobalRecharge(attackInfo.Cooldown, attackIndex);
            }
            else
            {
                StartCastTime(attackInfo.CastTime);
                fatbic.StartGlobalRecharge(attackInfo.CastTime,attackIndex);
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
            if (cooldownImage == null || !attackInfo.IsCasting && !onGlobalCooldown) return;
            cooldownImage.fillAmount = (rechargeEnd - Time.time) / rechargeTime;
            cooldownImage.color = Color.Lerp(startColor, endColor, 1 - cooldownImage.fillAmount);
            if(attackInfo.DamageStyle == DamageStyle.Tick && attackInfo.IsCasting)
            {
                if (Time.time >= nextSecond)
                {
                    nextSecond = Mathf.FloorToInt(Time.time) + 1;
                    FireAttackAttempt();

                }
            }
        }

        public void CastTimeTick()
        {
            if (castTimeImage == null || !attackInfo.IsCasting && !onGlobalCooldown) return;
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
            if (castTimeImage == null || !attackInfo.IsCasting) return;
            castTimeImage.fillAmount = 0.0f;
            castGlowImage.enabled = false;
            button.enabled = true;
            if(attackInfo.DamageStyle == DamageStyle.Delayed)
                FireAttackAttempt();
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

        private void FireAttackAttempt()
        {
            var handler = AttackAttempt;
            if (handler != null)
                AttackAttempt(this, new DataEventArgs<AttackInfo> { Data = attackInfo });
        }

        private void SetFontColor()
        {

        }

        Color32 ContrastColor(Color32 color)
        {
            byte d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double a = 1 - (0.299 * color.r + 0.587 * color.g + 0.114 * color.b) / 255;

            if (a < 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            return new Color32(d, d, d, color.a);
        }
    }
}
