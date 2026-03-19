using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class Mage : Player
{
    public Mage(string name)
    {
        Name = name;
        MaxHp = 100;
        Hp = MaxHp;
        MaxMp = 120;
        Mp = MaxMp;
        Attack = 25;
        CritChance = 0.1;
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
            Description = "마나를 몸에 둘러 다음"
        })

    }
}
