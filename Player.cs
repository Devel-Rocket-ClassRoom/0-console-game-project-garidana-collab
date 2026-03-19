using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
// Player.cs

public abstract class Player : Character
{
    public int MaxMp { get; set; }
    public int Mp { get; set; }
    public List<Skill> skills = new();

    public void PrintSkills()
    {

        for (int i = 0; i < skills.Count; i++)
        {
            Write($"[{i}] {skills[i].Name} ({skills[i].ManaCost})");
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
        Write("스킬을 선택하세요.");
    }
}