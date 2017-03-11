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
        bool attackInProgress;
        float nextSecond = 0;

        private void Update()
        {
            if (!attackInProgress && !onGlobalCooldown) return;
            if(rechargeEnd <= Time.time)
            {                
                EndCooldown();
                EndCastTime();
                attackInProgress = false;
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
            attackInfo = fatbic.GetAttackInformation(attackIndex);
        }

        public void StartButtonAction()
        {            
            if (onGlobalCooldown) return;
            attackInProgress = true;
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

        private void Start()
        {
            ProcessAttackInfo();
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
            if (cooldownImage == null || !attackInProgress && !onGlobalCooldown) return;
            cooldownImage.fillAmount = (rechargeEnd - Time.time) / rechargeTime;
            cooldownImage.color = Color.Lerp(startColor, endColor, 1 - cooldownImage.fillAmount);
            if(attackInfo.DamageStyle == DamageStyle.Tick && attackInProgress)
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
            if (castTimeImage == null || !attackInProgress && !onGlobalCooldown) return;
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
            if (castTimeImage == null || !attackInProgress) return;
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
            SetButtonColor();
            castGlowImage.color = button.image.color;
        }

        private void SetButtonColor()
        {
            var buttonImage = button.GetComponent<Image>();
            switch (attackInfo.MonsterType)
            {
                case MonsterType.Fae:
                    buttonImage.color = Color.cyan;
                    break;
                case MonsterType.Dragon:
                    buttonImage.color = Color.green;
                    break;
                case MonsterType.Light:
                    buttonImage.color = Color.white;
                    break;
                case MonsterType.Shadow:
                    buttonImage.color = Color.black;
                    break;
                case MonsterType.Demon:
                    
                    break;
                case MonsterType.Mechanical:
                    buttonImage.color = Color.gray;
                    break;
                case MonsterType.Wood:
                    break;
                case MonsterType.Wind:
                    buttonImage.color = Color.blue;
                    break;
                case MonsterType.Fire:
                    buttonImage.color = Color.red;
                    break;
                case MonsterType.Water:
                    break;
                default:
                    break;
            }
        }

        private void FireAttackAttempt()
        {
            var handler = AttackAttempt;
            if (handler != null)
                AttackAttempt(this, new DataEventArgs<AttackInfo> { Data = attackInfo });
        }
    }
}