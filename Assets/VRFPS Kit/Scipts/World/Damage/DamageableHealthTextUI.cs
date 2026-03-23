using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRFPSKit
{
    public class DamageableHealthTextUI : MonoBehaviour
    {
        public TMP_Text text;
        private Damageable _damageable;

        private bool isDeath;
        [SerializeField] private GameObject txtBG;

        // Update is called once per frame
        void Update()
        {
            if (isDeath)
                return;

            int health = (int)_damageable.health;
            
            if(health > 0)
                text.text = $"HP: {health}\u2665";
            else
            {
                text.text = "Dead";
                isDeath = true;
                Invoke(nameof(DisableAfterDeath), 3);
            }
        }

        private void DisableAfterDeath()
        {
            txtBG.SetActive(false);
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _damageable = GetComponentInParent<Damageable>();
        }
    }
}