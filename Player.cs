using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
// Player.cs

public abstract class Player : Character
{
    
    public List<Skill> skills = new();

    public void PrintSkills()
    {
        WriteLine("-------------------------------");

        for (int i = 0; i < skills.Count; i++)
        {
            Write($"{i}. {skills[i].Name} ({skills[i].ManaCost})");
            if (skills[i].CurrentCD > 0)
            {
                Write($" [남은 쿨타임: {skills[i].CurrentCD}]");
            }
            if (skills[i].ManaCost > Mp)
            {
                Write("[마나 부족]");
            }
            WriteLine();
        }
        WriteLine("-------------------------------");

        Write("스킬을 선택하세요.");
    }

    public void PrintStatusEffects()
    {
        WriteLine("@ 버프/디버프 현황 @");
        foreach (var se in statusEffects)
        {
     
            WriteLine($"- [{se.Name}] {se.Duration}턴 남음 ");
        }
        WriteLine();
        //for (int i = 0; i < statusEffects.Count; i++)
        //{
        //    WriteLine($"[{statusEffects[i].Name} - {statusEffects[i].Duration} 남음]");
        //}
    }
}