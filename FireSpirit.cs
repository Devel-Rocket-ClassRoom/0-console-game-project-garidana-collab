using System;
using System.Collections.Generic;
using System.Text;

class FireSpirit : Monster
{

    protected int roundCount { get; set; } = 1; // 3라운드당 강력한 데미지용
    public int heatStack = 0;  // 열기 데미지의 

    Random random = new();

    public FireSpirit ()
    {
        
        

        Name = "불의 정령";
        MaxHp = 150;
        Hp = MaxHp;
        Attack = 20;
        CritChance = 0.05;
        EvadeChance = 0.05;
        HeatDamage = 3;


        mskills.Add(new Skill
        {
            Name = "녹아내린 주먹",
            Description = "불의 정령이 묵직하게 때립니다"
        });
        mskills.Add(new Skill
        {
            Name = "화염 강타",
            Description = "화염으로 가득 찬 공격을 합니다"
        });
        
    }

    public override void ExecuteSkill (Character target, BattleSystem bs)
    {
        roundCount++;
        if (bs.Round % 3 == 0)
        {
            Console.WriteLine();
            Console.WriteLine($"{Name}이 {mskills[1].Name}을 시전했습니다.");
            bs.MonsterDealDamage(this, target, 3);
        }
        else
        {
            bs.MonsterDealDamage(this, target, 1);
            heatStack++;
        }
    }
}
