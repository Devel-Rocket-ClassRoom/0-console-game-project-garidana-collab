using System;
using System.Collections.Generic;
using System.Text;

// Monster.cs

public abstract class Monster : Character
{
    protected List<Skill> mskills = new List<Skill>();

    protected ConsoleColor MColor;

    public abstract void ExecuteSkill(Character target, BattleSystem bs);
}