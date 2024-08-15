using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    /// <summary>
    /// Param 1 : current Health
    /// Param2 : Position of the attacker
    /// </summary>
    public UnityEvent<int, Vector3> OnHealthChange;
    /// <summary>
    /// Param1 : Position of death
    /// Param 2 : Position of attacker
    /// </summary>
    public UnityEvent<Vector3, Vector3> OnObjectDeath;

    [Tooltip("Health at the start of the object's life")]
    [SerializeField] private int _baseHealth = 100;
    [Tooltip("Max health that the obect can have (if it can be healed)")]
    [SerializeField] private int _maxHealth = 100;
    [Tooltip("Current Health of the object, READ ONLY!!!")]
    [SerializeField] private int _health = 0;
    [Tooltip("Wether or not this health component will apply the damage taken or not")]
    [SerializeField] public bool _disabled;

    private void Start()
    {
        _health = _baseHealth;
    }

    /// <summary>
    /// The object's health will decrease by the amount defined in "dmg" and will call an OnHealthChange event.
    /// <br></br>
    /// If health reaches 0 it will call a OnObjectDeath event
    /// </summary>
    /// <param name="dmg">Damage taken</param>
    /// <param name="attackerPos">Position of the actor that inflicts the damage</param>
    public void TakeDamage(int dmg, Vector3 attackDirection) //muy provisional esto 
    {
        if (_disabled) return;


        _health -= dmg;
        OnHealthChange?.Invoke(_health, attackDirection);
        if (_health <= 0)
        {
            OnObjectDeath?.Invoke(transform.position, attackDirection);
        }
    }

    /// <summary>
    /// This object's health will increase by the amount defined in "healAmount" and will call an OnHealthChange event.
    /// </summary>
    /// <param name="healAmount">The amount that this object will heal</param>
    public void Heal(int healAmount)
    {
        if (_disabled) return;


        _health += healAmount;
        if (_health > _maxHealth) _health = _maxHealth;
        //OnHealthChange?.Invoke(_health, Vector3.zero); //attack pos unused
    }



    // A partir de aqui todo esto son pruebas para ver que el HUD se updatea. PUES VAYA MIERDA DE PRUEBAS POL

    //private float targetTime = 5.0f;

    //private void Update()
    //{

    //    targetTime -= Time.deltaTime;
    //    //Debug.Log(targetTime);

    //    if (targetTime <= 0.0f)
    //    {
    //        timerEnded();
    //        targetTime = 5.0f;
    //    }
    //}

    //private void timerEnded()
    //{
    //    _health -= 10;
    //    OnHealthChange?.Invoke(_health, Vector3.zero);
    //}
}
