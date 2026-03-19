using System;
using System.Collections.Generic;
using System.Text;

// Warrior.cs
class Warrior : Player
{
    public Warrior(string name)
    {
        Name = name;

        MaxHp = 120;

        Hp = MaxHp;

        MaxMp = 80;

        Mp = MaxMp;

        Attack = 18;

        CritChance = 0.10;

        EvadeChance = 0.10;

        skills.Add(new Skill
        {
            // 스킬 0 기본 공격
            Name = "베기",
            Description = "전사의 기본공격입니다",
            ManaCost = 0,
            CoolDown = 0,
            Effect = (player, monster, bs) => bs.PlayerDealDamage(player, monster, 1)
        });
        skills.Add(new Skill
        {
            // 스킬 1
            Name = "방어 태세",
            Description = "방패를 들어올려 다음턴에 받는 데미지를 50% 감소 시킵니다",
            ManaCost = 20,
            CoolDown = 3,
            EffectSpan = 1,
            Effect = (player, monster, bs) =>
            {
                player.statusEffects.Add(new StatusEffect
                {
                    Name = "방어태세",
                    Duration = 1,
                    Value = 0.5,
                    OnTakeDamage = (damage) =>
                    {
                        return damage *= 0.5;
                    }
                });
            }
        });
        skills.Add(new Skill
        {
            // 스킬 2
            Name = "전투 함성",
            Description = "최대 체력을 20 증가 시키고, 20의 체력을 회복합니다",
            ManaCost = 40,
            CoolDown = 4,
            Effect = (player, monster, bs) =>
            {
                player.MaxHp += 20;
                player.Hp += 20;
            }
        });
        skills.Add(new Skill
        {
            // 스킬 3
            Name = "분노 강타",
            Description = "받은 만큼 더 세게 돌려줍니다....",
            ManaCost = 50,
            CoolDown = 5,
            Effect = (player, monster, bs) =>
            {
                bs.PlayerDealDamage(player, monster, 2, ((player.MaxHp - player.Hp) * 0.5));
            }
        });
    }
}
