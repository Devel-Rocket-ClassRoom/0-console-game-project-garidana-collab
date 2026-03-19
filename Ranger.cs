using System;
using System.Collections.Generic;
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
        Attack = 15;
        CritChance = 0.15;
        EvadeChance = 0.20;

        skills.Add(new Skill
        {
            Name = "관통 화살",
            Description = "화살로 적을 관통하는 궁수의 기본 공격입니다.",
            ManaCost = 0,
            CoolDown = 0,
        });
        skills.Add(new Skill
        {
            Name = "암살자의 발재간",
            Description = "현란한 발놀림으로 다음 2턴 동안 회피 확률을 20% 높입니다.",
            ManaCost = 20,
            CoolDown = 4
        });
        skills.Add(new Skill
        {
            Name = "심장 터뜨리기",
            Description = "심장을 관통해 적에게 다음 3턴동안 내출혈을 일으킵니다.",
            ManaCost = 15,
            CoolDown = 3
        });
        skills.Add(new Skill
        {
            Name = "은신",
            Description = "몸을 은신하고 모든 공격을 회피합니다. 은신에서 벗어날시 100% 확률로 치명타를 터뜨립니다.",
            ManaCost = 25,
            CoolDown = 4
        });
        skills.Add(new Skill
        {
            Name = "예리한 눈",
            Description = "사냥꾼의 예리한 감각으로 다음 2턴동안 치명타 확률을 20% 증가시킵니다.",
            ManaCost = 30,
            CoolDown = 3
        });
    }
}
