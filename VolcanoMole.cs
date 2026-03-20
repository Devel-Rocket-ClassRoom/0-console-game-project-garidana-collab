using System;
using System.Collections.Generic;
using System.Text;


class VolcanoMole : Monster
{
    public bool IsBurrow = false;

    Random rand = new Random();
    public VolcanoMole()
    {
        Name = "화산 두더지";
        MaxHp = 150;
        Hp = MaxHp;
        Attack = 15;
        CritChance = 0.0;
        EvadeChance = 0.0;
        MColor = ConsoleColor.DarkBlue;

        mskills.Add(new Skill
        {
            Name = "땅굴 잠행",
            Description = "화산 두더지가 굴을 파고 들어갑니다...",
            // Value = 0;
            //Effect = (player, monster, bs) => bs.MonsterDealDamage(monster, player, 2.5)
        });
    }

    public override void ExecuteSkill(Character target, BattleSystem bs)
    {
        if (!IsBurrow)
        {
            
            if (rand.NextDouble() <= 0.20)  // 땅굴 잠행 발동 확률
            {
                IsBurrow = true;
                Console.WriteLine($"{Name}이(가) \"{mskills[0].Name}\"을 시전했습니다.\n{mskills[0].Description}");
            }
            else
            {
                bs.MonsterDealDamage(this, target, 1);
            }
        }
        else if (IsBurrow)
        {
            bs.MonsterDealDamage(this, target, 4);
            IsBurrow = false;
        }
    }

}