using System;
using System.Collections.Generic;
using System.Text;

// StatusEffect.cs
public class StatusEffect
{
    // 버프 디버프 관리 클래스
    public string Name;
    public int Duration; // 지속효과 남은 지속 턴
    public double Value; // 효과 수치
    public Action<Character, BattleSystem> OnTurnStart; // 턴 시작시 효과
    public Func<double, double> OnTakeDamage; // 피해받을 시 효과
}