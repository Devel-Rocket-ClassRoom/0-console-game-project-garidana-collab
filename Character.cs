using System;
using System.Collections.Generic;

using System.Text;

// Character.cs

public abstract class Character
{
    public string Name { get; set; }
    public int MaxHp { get; set; }
    public double Hp { get; set; }
    public double Attack { get; set; }
    public double CritChance { get; set; }
    public double EvadeChance { get; set; }

    public bool IsAlive => Hp > 0;

    public bool IsCrit = false;
    public bool IsEvade = false;

    public List<StatusEffect> statusEffects = new();

    // 이름 색상
    public ConsoleColor Color { get; set; }
}


