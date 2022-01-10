using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter
{
   [SerializeField] public FighterBase _base;
   [SerializeField] public int level;

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public FighterBase Base
    {
        get
        {
            return _base;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
    }

    public Fighter(FighterBase fBase, int flevel)
    {
     
        Init();

    }
    
    public bool HpChanged { get; set; }
    public int Exp { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }

    //Todas las formulas se han cogido de pokemon
    void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5);
        Stats.Add(Stat.MagickAttack, Mathf.FloorToInt((Base.MagickAttack * Level) / 100f) + 5);
        Stats.Add(Stat.MagickDefense, Mathf.FloorToInt((Base.MagickDefense * Level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5);

        MaxHp = Mathf.FloorToInt((Base.MaxHp * Level) / 100f) + 10 + Level;
    }

    public void Init()
    {

        // Generar habilidades
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }

        Exp = Base.GetExpForLevel(Level);

        CalculateStats();
       
        HP = MaxHp;
       
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];
        return statVal;
    }

    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }
        return false;
    }
    public int Attack
    {
        get { return GetStat(Stat.Attack); }
    }

    public int Defense
    {
        get { return GetStat(Stat.Defense); }
    }
        

    public int MagickAttack
    {
        get { return GetStat(Stat.MagickAttack); }
    }

    public int MagickDefense
    {
        get { return GetStat(Stat.MagickDefense); }
    }

    public int Speed
    {
        get { return GetStat(Stat.Speed); }
    }

    public int MaxHp
    {
          get; private set; 
    }

    public DamageDetails TakeDamage(Move  move, Fighter attacker) 
    {
        float critical = 1f;
        if (Random.value * 100f < 6.25f) // critico
            critical = 2f;
        
        float typeEffectiveness = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = typeEffectiveness,
            Critical = critical,
            Fainted = false

        };

        float attack = (move.Base.IsMagick) ? attacker.MagickAttack : attacker.Attack;
        float defense = (move.Base.IsMagick) ? MagickDefense : Defense;
       
        float modifiers = Random.Range(0.85f, 1f) * typeEffectiveness * critical;
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attack / defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        UpdateHP(damage);

        if (HP <= 0)
        {
            HP = 0;
            damageDetails.Fainted = true;
        }
        return damageDetails;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHp);
        HpChanged = true;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }

}

public class DamageDetails
{
    public bool Fainted { get; set; }

    public float Critical { get; set; }

    public float TypeEffectiveness { get; set; }

}
