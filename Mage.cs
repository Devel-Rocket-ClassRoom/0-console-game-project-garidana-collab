using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

// Mage.cs

class Mage : Player
{
    public Mage(string name)
    {
        Name = name;
        MaxHp = 100;
        Hp = MaxHp;
        MaxMp = 120;
        Mp = MaxMp;
        Attack = 20;
        CritChance = 0.05;
        EvadeChance = 0.1;

        skills.Add(new Skill
        {
            Name = "비전구",
            Description = "마법사의 기본 공격입니다",
            ManaCost = 10,
            CoolDown = 0,
            Effect = (player, monster, bs) => bs.PlayerDealDamage(player, monster, 1)
        });
        skills.Add(new Skill
        {
            Name = "마나 실드",
            Description = "마나를 몸에 둘러 어떤 공격이던 막아냅니다",
            ManaCost = 30,
            CoolDown = 4,
            Effect = (player,monster, bs) =>
            {
                player.statusEffects.Add(new StatusEffect
                {
                    Name = "마나 실드",
                    Duration = 1,
                    OnTakeDamage = (damage) =>
                    {
                        return damage *= 0;
                    }
                });
            }
        });
        skills.Add(new Skill
        {
            Name = "마나 작렬",
            Description = "마나를 끌어모아 강력한 데미지를 입힙니다",
            ManaCost = 0,
            CoolDown = 4,
            Effect = (player, monster, bs) =>
            {
                double cost = player.Mp * 0.5;
                bs.PlayerDealDamage(player, monster, 0, (cost * 1.5));
                player.Mp -= cost; // 마나 소모량 작업 필요
            }
        });
        skills.Add(new Skill
        {
            Name = "저주 : 대혼란",
            Description = "상대방의 정신을 교란해 사리분별도 못하게 합니다",
            ManaCost = 20,
            CoolDown = 6,
            Effect = (player,monster,bs) =>
            {
                monster.statusEffects.Add(new StatusEffect
                {
                    Name = "대혼란",
                    Duration = 2,
                    OnTurnStart = (character, bs) => bs.MonsterDealDamage(monster, monster, 1)
                });
               
            }
        });


    }
}
