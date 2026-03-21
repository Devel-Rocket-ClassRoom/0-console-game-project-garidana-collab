using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class Ranger : Player
{
    public Ranger(string name)
    {
        Name = name;
        MaxHp = 80;
        Hp = MaxHp;
        MaxMp = 100;
        Mp = MaxMp;
        Attack = 18;
        CritChance = 0.20;
        EvadeChance = 0.20;

        skills.Add(new Skill
        {
            Name = "관통 화살",
            Description = "화살로 적을 관통하는 궁수의 기본 공격입니다.",
            ManaCost = 0,
            CoolDown = 0,
            Effect = (player, monster, bs) => bs.PlayerDealDamage(player, monster, 1)
        });
        skills.Add(new Skill
        {
            Name = "암살자의 발재간",
            Description = "현란한 발놀림으로 다음 2턴 동안 회피 확률을 20% 높입니다.",
            ManaCost = 20,
            CoolDown = 4,
            Effect = (player,monster, bs) =>
            {
                player.EvadeChance += 0.2; // 회피 증가 // 원래 Duration 2
                player.statusEffects.Add(new StatusEffect
                {
                    Name = "암살자의 발재간",
                    Duration = 2,
                    OnExpire = (character, bs) =>
                    {
                        // 지속효과 만료시 회피 확률 원복
                        character.EvadeChance -= 0.2; 
                    }
                });
            }
        });
        skills.Add(new Skill
        {
            Name = "심장 터뜨리기",
            Description = "상대방의 심장을 터뜨려 적에게 다음 3턴동안 내출혈을 일으킵니다.",
            ManaCost = 15,
            CoolDown = 3,
            Effect = (player,monster,bs) =>
            {
                bs.PlayerDealDamage(player, monster, 1);
                monster.statusEffects.Add(new StatusEffect
                {
                    Name = "내장 파열",
                    Duration = 3,
                    OnTurnStart = (character, bs) =>
                    {
                        // 공격력 만큼의 출혈 피해
                        character.Hp -= player.Attack;
                        Console.WriteLine($"{character.Name}은 {player.Attack}의 출혈 피해를 입고있다");
                        Console.WriteLine();
                    }
                });
            }
        });
        skills.Add(new Skill
        {
            Name = "은신",
            Description = "몸을 은신하고 모든 공격을 회피합니다. 은신에서 벗어날시 100% 확률로 치명타를 터뜨립니다.",
            ManaCost = 25,
            CoolDown = 4,
            Effect = (player,monster,bs) =>
            {
               

                player.statusEffects.Add(new StatusEffect
                {
                    Name = "은신",
                    Duration = 1,
                    OnTakeDamage = (damage) =>
                    {
                        return damage *= 0;

                    },
                    OnExpire = (character, bs) =>
                    {
                        double originalCritChance = character.CritChance;
                        character.CritChance = 1.0;
                        character.statusEffects.Add(new StatusEffect
                        {
                            Name = "은신:약점 공격",
                            Duration = 1,
                            OnExpire = (character1, bs) =>
                            {
                                character1.CritChance = originalCritChance;
                            }
                        });
                    }

                });
                
               
            }
        });
        skills.Add(new Skill
        {
            Name = "예리한 눈",
            Description = "사냥꾼의 예리한 감각으로 다음 2턴동안 치명타 확률을 20% 증가시킵니다.",
            ManaCost = 30,
            CoolDown = 3,
            Effect = (player, monster, bs) =>
            {
                player.CritChance += 0.2;
                player.statusEffects.Add(new StatusEffect
                {
                    Name = "예리한 눈",
                    Duration = 2,
                    OnExpire = (player, bs) =>
                    {
                        player.CritChance -= 0.2;
                    }
                });
            }
        });
    }
}
