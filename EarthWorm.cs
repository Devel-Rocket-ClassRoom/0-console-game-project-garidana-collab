using System;
using System.Collections.Generic;
using System.Text;

class EarthWorm : Monster
{
    Random random = new();
    public EarthWorm()
    {
        IsFinalBoss = true;

        Name = "지구 지렁이";
        MaxHp = 1000;
        Hp = MaxHp;
        Attack = 80;
        CritChance = 0.1;
        EvadeChance = 0.1;
        HeatDamage = 80;

        mskills.Add(new Skill
        {
            Name = "파멸의 비명",
            Description = "비명소리에 귀가 들리지 않게 됩니다.."
        });
        mskills.Add(new Skill
        {
            Name = "대지진",
            Description = "땅이 갈라지고 동굴 전체가 흔들립니다. 회피가 불가능 할 것 같습니다..."
        });
        mskills.Add(new Skill
        {
            Name = "화산의 지배자",
            Description = "지구 지렁이가 용암에 들어갑니다..."
        });
        mskills.Add(new Skill
        {
            Name = "지배자의 응시",
            Description = "아직 내 더듬이도 못 건드린다"
        });
    }
    public override void ExecuteSkill(Character target, BattleSystem bs)
    {

        bs.MonsterDealDamage(this, target, 10);
        //if (random.NextDouble() < 0.1)
        //{
        //    bs.MonsterDealDamage(this, target, 1);

        //}
        //else
        //{
        //    if (random.NextDouble() < 0.5)
        //    {
        //        Console.WriteLine($"{Name}가 {mskills[0].Name}을 시전합니다.\n{mskills[0].Description}");
        //        bs.MonsterDealDamage(this, target, 0.5);
        //        target.statusEffects.Add(new StatusEffect
        //        {
        //            Name = "이명",
        //            Duration = 5,
        //            OnTurnStart = (target, bs) =>
        //            {
                        
        //            }
        //        })
        //    }
        //}

    }
}
