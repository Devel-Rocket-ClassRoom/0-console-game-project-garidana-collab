using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;
// Player.cs

public abstract class Player : Character
{
    public bool IsHeatImmune = false;
    // 플레이어 상점 골드
    public int gold = 100; // 원래 100 골드
    // 플레이어 인벤토리
    public Dictionary<string, int>  Inv = new();
    // 플레이어 스킬
    public List<Skill> skills = new();
    // 아티팩트 슬롯
    public List<Artifacts> artifacts = new();

    public void PrintSkills()
    {
        WriteLine("-------------------------------------");
        WriteLine("스킬을 선택하세요.");
        WriteLine();
        WriteLine("0. 인벤토리");
        for (int i = 0; i < skills.Count; i++)
        {
            Write($"{i + 1}. {skills[i].Name} ({skills[i].ManaCost})");
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
        WriteLine("-------------------------------------");

        
    }

    public void PrintStatusEffects()
    {
        if (statusEffects.Count > 0)
        {
            WriteLine("@ 버프/디버프 현황 @");
        foreach (var se in statusEffects)
        {
     
            WriteLine($"- [{se.Name}] {se.Duration}턴 남음 ");
        }
        }
        
        //for (int i = 0; i < statusEffects.Count; i++)
        //{
        //    WriteLine($"[{statusEffects[i].Name} - {statusEffects[i].Duration} 남음]");
        //}
    }
}