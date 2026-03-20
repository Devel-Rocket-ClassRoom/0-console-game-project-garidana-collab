using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Text;

class FireSpirit : Monster
{

    protected int roundCount { get; set; } = 1; // 3라운드당 강력한 데미지용
    public int heatStack = 0;  // 열기 데미지의 
    bool IsLavaAbsorb = false;

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
        mskills.Add(new Skill
        {
            Name = "용암 흡수",
            Description = "불의 정령이 용암을 흡수해 체력을 회복하기 시작합니다"
        });
        
    }

    public override void ExecuteSkill (Character target, BattleSystem bs)
    {
        roundCount++;
        if (bs.Round % 3 == 0)
        {
            Console.WriteLine();
            Console.WriteLine($"{Name}이 {mskills[1].Name}을 시전했습니다.\n{mskills[1].Description}");
            bs.MonsterDealDamage(this, target, 3);
        }
        // 체력이 50% 미만으로 내려갈 시 용암흡수 실행
        if (bs.Round % 2 == 0 && Hp < MaxHp * 0.5 && !IsLavaAbsorb)
        {
            IsLavaAbsorb = true;
        }
        // 용암흡수 체력 회복
        if (bs.Round % 2 == 0 && IsLavaAbsorb)
        {
            Console.WriteLine($"{Name}이 {mskills[2].Name}을 시전했습니다.\n{mskills[2].Description}");
            Hp += 8;
        }
        else
        {
            if (heatStack == 5)
            {
                heatStack = 0;
            }
            bs.MonsterDealDamage(this, target, 1);
            Console.WriteLine($"{Name}의 {mskills[0].Name}으로 인해 열기피해가 증가했습니다.");
            heatStack++;
            HeatDamage += heatStack;
        }
        
    }
}
