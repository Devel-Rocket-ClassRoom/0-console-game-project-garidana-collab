using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

class SulfuricSlime : Monster
{
    Random random = new();
    public SulfuricSlime()
    {
        Name = "유황 슬라임";
        MaxHp = 150;
        Hp = MaxHp;
        Attack = 15;
        CritChance = 0.05;
        EvadeChance = 0.01;

        mskills.Add(new Skill
        {
            Name = "유황 분출",
            Description = "유황 가스가 주변을 감쌉니다"
        });
        
        // ㅇ
    }
    public override void ExecuteSkill(Character target, BattleSystem bs)
    {
        
        if (random.NextDouble() <= 0.3)
        {
            Console.WriteLine($"{Name}이(가) {mskills[0].Name}을 시전했습니다.\n{mskills[0].Description}");
            if (!target.statusEffects.Any(se => se.Name == "가스 중독"))
            {
                target.statusEffects.Add(new StatusEffect
                {
                    Name = "가스 중독",
                    Duration = 3,
                    OnTurnStart = (target, bs) =>
                    {
                        target.Hp -= 3;
                        Console.WriteLine($"3의 가스 중독 피해를 입었습니다.");
                        Console.WriteLine();
                    }
                });
            }
            
        }
        else
        {
            bs.MonsterDealDamage(this, target, 1.5);
            if (random.NextDouble() <= 0.5)
            {
                Console.WriteLine($"{Name}의 공격에 유황이 유독 많이 함유 되어있었습니다.\n{mskills[0].Description}");
                if (!target.statusEffects.Any(se => se.Name == "가스 중독"))
                {
                    target.statusEffects.Add(new StatusEffect
                    {
                        Name = "가스 중독",
                        Duration = 3,
                        OnTurnStart = (target, bs) =>
                        {
                            target.Hp -= 3;
                            Console.WriteLine($"3의 가스 중독 피해를 입었습니다.");
                            Console.WriteLine();
                        }
                    });
                }
                
            }
        }

    }
}

