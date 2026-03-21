using System;


// 아티팩트 티어 이넘
public enum ArtifactTier {Common, Heroic, Legendary}


public class Artifacts
{
    public string Name;
    public string Description;

    public ArtifactTier Tier;
    // 턴 시작시 액션
    public Action<Character, BattleSystem> OnTurnStart;
    // 피격시 펑션
    public Func<double, double> OnTakeDamage;
    // 효과 만료시
    public Action<Character, BattleSystem> OnExpire;
    // 전투 시작시 
    public Action<Player, BattleSystem> OnBattleStart;
    // 몬스터 처치시
    public Action<Character ,BattleSystem> OnMonsterDeath;
    // 공격시
    public Action<Player,BattleSystem> OnDealDamage;
    // 아티팩트 장착시 즉시 효과 적용
    public Action<Player> OnEquip;



    public Artifacts()
    {
    }
}
