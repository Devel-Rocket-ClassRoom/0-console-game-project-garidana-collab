using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

class MagmaTortoise : Monster
{
    public bool IsShellOn = false; // 껍질 열림? => false
    public int ShellDuration = 3;

    Random random = new();
    public MagmaTortoise()
    {
        Name = "용암 거북";
        MaxHp = 1500;
        Hp = MaxHp;
        Attack = 20;
        CritChance = 0.05;
        EvadeChance = 0.02;
        HeatDamage = 5;
        // 0
        mskills.Add(new Skill
        {
            Name = "용암 박치기",
            Description = "용암 거북이 용암이 튀는 박치기를 시전합니다."
        });
        // 1
        mskills.Add(new Skill
        {
            Name = "껍질 틀어박기",
            Description = "용암 거북이 껍질속으로 숨습니다."
        });
        // 2
        mskills.Add(new Skill
        {
            Name = "육중한 돌진",
            Description = "껍질에 숨은 채 무서운 속도로 돌진해옵니다...\n돌진이 끝나자 껍질이 열렸습니다!"
        });
        // 3
        mskills.Add(new Skill
        {
            Name = "용암 분사",
            Description = "용암 거북의 껍질에서 엄청난 양의 용암이 터져나옵니다..."
        });
        // 4
        mskills.Add(new Skill
        {
            Name = "지각 진동",
            Description = "땅을 세게 내리쳐 땅을 울립니다..."
        });

    }

    public override void ExecuteSkill(Character target, BattleSystem bs)
    {
        if (!IsShellOn) // 껍질이 벗겨진 상태는 기본 공격만 
        {
            // 3턴 마다 껍질 틀어박기
            if (bs.Round % 3 == 0 && bs.Round > 0)
            {
                IsShellOn = true;
                Console.WriteLine($"{Name}이 {mskills[1].Name}를 시전했습니다. {mskills[1].Description}");
                this.statusEffects.Add(new StatusEffect
                {
                    Name = "껍질 닫기",
                    Duration = 3,
                    OnTakeDamage = (damage) =>
                    {
                        return damage *= 0.1;
                    }
                });
            }
            else
            {
                // 기본 공격 : 용암 박치기
                bs.MonsterDealDamage(this, target, 1);
            }
        }
        else // IsShellOn = true;
        { 
            // 육중한 돌진 
            if (bs.Round % 3 == 0)
            {
                this.statusEffects.RemoveAll(se => se.Name == "껍질 닫기");
                Console.WriteLine($"{Name}이 {mskills[2].Name}를 시전했습니다. {mskills[2].Description}");
                bs.MonsterDealDamage(this, target, 1);
                if (random.NextDouble() < 0.6) // 기절 발동 확률
                {
                    if (!target.statusEffects.Any(se => se.Name == "기절"))
                    {
                        target.statusEffects.Add(new StatusEffect
                        {
                            Name = "기절",
                            Duration = 2,
                            OnTurnStart = (target, bs) =>
                            {
                                target.IsIncap = true;
                            }
                        });
                        Console.WriteLine($"{mskills[3].Name}에 의해 화상을 입었습니다. 열기 데미지를 2배로 입습니다...");
                    }
                    
                }
                IsShellOn = false;
                return;  // 육중한 돌진 사용후엔 무조건 return
            }
            // 용암 분사 (40% 확률로 화상 부여)
            if (random.NextDouble() < 0.4)
            {
                Console.WriteLine($"{Name}이 {mskills[3].Name}를 시전했습니다. {mskills[3].Description}");
                if (random.NextDouble() < 0.5)
                {
                    // 이미 화상 효과 보유시 스킵
                    if (!target.statusEffects.Any(se => se.Name == "화상")) // 화상 효과 중첩 방지
                    {
                        target.statusEffects.Add(new StatusEffect
                        {
                            Name = "화상",
                            Duration = 5,
                            OnTurnStart = (target, bs) =>
                            {
                                
                            }
                        });
                        Console.WriteLine($"{mskills[3].Name}에 의해 화상을 입었습니다. 열기 데미지를 심각하게 입습니다...");
                    }
                    else // target이 화상효과 보유시 일반 데미지
                    {
                        bs.MonsterDealDamage(this, target, 1.5);
                    }
                   
                }
                // 나머지는 높은 데미지의 공격
                else
                {
                    bs.MonsterDealDamage(this, target, 3);
                }
                
            }

            // 지각 진동
            else
            {
                Console.WriteLine($"{Name}이 {mskills[4].Name}를 시전했습니다. {mskills[4].Description}");
                if (!target.statusEffects.Any(se => se.Name == "어지러움"))
                {
                    target.statusEffects.Add(new StatusEffect
                    {
                        Name = "어지러움",
                        Duration = 2,
                        OnExpire = (target, bs) =>
                        {
                            target.EvadeChance += 0.2;
                        }
                    });
                    target.EvadeChance -= 0.2;
                    Console.WriteLine($"{target.Name}이 진동에 의해 어지러워 합니다. 공격을 회피하기 어렵습니다...");
                }
            }
        }
    }
}