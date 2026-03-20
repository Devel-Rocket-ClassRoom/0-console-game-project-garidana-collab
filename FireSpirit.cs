using System;
using System.Collections.Generic;
using System.Text;

class FireSpirit : Monster
{
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
            Description = "불의 정령이 묵직하게 때립니다."
        });
        mskills.Add(new Skill
        {
            Name = "화염 강타",
            Description = "거센 불길이 치솟습니다."
        });
        mskills.Add(new Skill
        {
            Name = "용암 흡수",
            Description = "불의 정령이 용암을 흡수해 체력을 회복하기 시작합니다."
        });
        
    }

    public override void ExecuteSkill (Character target, BattleSystem bs)
    {
        // 체력이 50% 미만으로 내려갈 시 용암흡수 실행
        if (bs.Round % 2 == 0 && Hp < MaxHp * 0.5 && !IsLavaAbsorb)
        {
            IsLavaAbsorb = true;
        }
        // 용암흡수 체력 회복
        if (bs.Round % 2 == 0 && IsLavaAbsorb)
        {
            Console.WriteLine($"{Name}이 \"{mskills[2].Name}\"를 시전했습니다.\n{mskills[2].Description}");
            Hp += 8;
        }

        // 시작 부터 매 3턴 마다"화염 강타"
        if (bs.Round % 3 == 0)
        {
            Console.WriteLine();
            Console.WriteLine($"{Name}이 \"{mskills[1].Name}\"을 시전했습니다.\n{mskills[1].Description}");
            bs.MonsterDealDamage(this, target, 3);
            return;
        }   

        // 기본공격 녹아내린 주먹
        if (heatStack < 5)
        {
            HeatDamage++;
            heatStack++;
        }
        bs.MonsterDealDamage(this, target, 1);
        Console.WriteLine($"{Name}의 \"{mskills[0].Name}\"으로 인해 열기피해가 증가했습니다 ({heatStack} 중첩).");
        
    }
}
