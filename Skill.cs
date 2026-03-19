using System;
using System.Collections.Generic;
using System.Text;

public class Skill
{
    public string Name;  // 스킬 명
    public string Description; // 스킬 설명
    public int ManaCost;  // 필요 마나
    public int CoolDown;  // 쿨타임
    public int CurrentCD;  // 남은 쿨타임
    public bool IsReady => CurrentCD == 0;  // 스킬 사용가능 여부
    public bool IsUltimate; // 궁극기
    public int EffectSpan; // 지속효과 지속기간.
    public int CurrentSpan; // 지속효솨 남은 지속기간.

    public Action<Player, Monster, BattleSystem> Effect;
}