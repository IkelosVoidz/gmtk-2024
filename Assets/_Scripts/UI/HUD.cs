using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text healthText, missionText;

    [SerializeField] Image bloodSplatter, bloodHit;

    [Header("Ammo")]
    [SerializeField] TextMeshProUGUI magazineAmmoUI;
    [SerializeField] TextMeshProUGUI totalAmmoUI;
    [SerializeField] Image ammoTypeUI;

    [Header("Weapon")]
    [SerializeField] Image activeWeaponUI;

    [Header("Throwables")]
    [SerializeField] Image lethalUI;
    [SerializeField] TextMeshProUGUI lethalAmountUI;

    [SerializeField] Image tacticalUI;
    [SerializeField] TextMeshProUGUI tacticalAmountUI;

    private bool _damageTaken = false;
    private int _health = 100;

    private void OnEnable()
    {
        //AttackConfigScriptable.OnShoot += UpdateAmmoDisplay;
        //ReloadConfigScriptable.OnAmmoChanged += UpdateAmmoDisplay;
        //PlayerWeaponManager.OnWeaponSwap += UpdateAmmoDisplay;
    }

    private void OnDisable()
    {
        //AttackConfigScriptable.OnShoot -= UpdateAmmoDisplay;
        //ReloadConfigScriptable.OnAmmoChanged -= UpdateAmmoDisplay;
        //PlayerWeaponManager.OnWeaponSwap -= UpdateAmmoDisplay;
    }

    private void Awake()
    {
        missionText.text = "";

        OnHealthChanged(100, Vector3.one);
    }


    public void OnHealthChanged(int newHealth, Vector3 attackDirection)
    {
        if (newHealth < _health)
        {
            _damageTaken = true;
            bloodHit.color = new Color(bloodHit.color.r, bloodHit.color.g, bloodHit.color.b, 0.5f);
        }

        _health = Mathf.Clamp(newHealth, 0, 100);

        //healthText.text = $"Health: {_health}";
        bloodSplatter.color = new Color(bloodSplatter.color.r, bloodSplatter.color.g, bloodSplatter.color.b, -(_health / 100f) + 1f);
    }

    public void InitializeMissions(Dictionary<string, SortedList<int, MissionObjective>> missions)
    {
        ChangeMissionDescription(missions["Inicial"][0]);
    }

    public void ChangeMissionDescription(MissionObjective obj)
    {
        missionText.text = $"{obj._description}";
    }

    private void Update()
    {
        if (_damageTaken)
        {
            if (bloodHit.color.a == 0f) _damageTaken = false;
            else
            {
                float newAlphaValue = bloodHit.color.a - Time.deltaTime;
                bloodHit.color = new Color(bloodHit.color.r, bloodHit.color.g, bloodHit.color.b, Math.Clamp(newAlphaValue, 0f, 1f));
            }
        }
    }
}

