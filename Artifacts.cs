using System;


// 아티팩트 티어 이넘
public enum ArtifactTier {태초, 일반, 영웅, 전설, 권능}


public class Artifacts
{
    public string Name;
    public string Description;

    public ArtifactTier Tier;
    // 턴 시작시 액션
    public Action<Player, BattleSystem> OnTurnStart;
    // 피격시 펑션 (용암 등딱지 아티팩트 전용 효과)
    public Action<Player, Monster, BattleSystem> OnReflect;
    // 몬스터 처치시
    public Action<Player ,BattleSystem> OnMonsterDeath;
    // 공격시
    public Action<Player,BattleSystem> OnDealDamage;
    // 아티팩트 장착시 즉시 효과 적용
    public Action<Player> OnEquip;

    public static Dictionary<string, Artifacts> All = new()
    {
        // 두발
        {"두더지 발톱", new Artifacts
                {
                    Name = "두더지 발톱",
                    Description = "치명타 확률이 5% 증가합니다.",
                    Tier = ArtifactTier.일반,
                    OnEquip = (player) =>
                    {
                        player.CritChance += 0.05;
                    }
                }
        },
        // 유황
        {"유황 점액 덩어리", new Artifacts
                {
                    Name = "유황 점액 덩어리",
                    Description = "매 턴 체력을 5 회복합니다.",
                    Tier = ArtifactTier.일반,
                    OnTurnStart = (player, bs) =>
                    {
                        player.Hp += 5;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                    }
                }
        },
        // 정수
        {"과열된 마나 정수", new Artifacts
                {
                    Name = "과열된 마나 정수",
                    Description = "매 턴 마나를 10 회복합니다",
                    Tier = ArtifactTier.일반,
                    OnTurnStart = (player, bs) =>
                    {
                        player.Mp += 10;
                        if (player.Mp > player.MaxMp)
                        {
                            player.Mp = player.MaxMp;
                        }
                    }
                }
        },
        // 거심
        {"거북이 골수", new Artifacts
                {
                    Name = "거북이 골수",
                    Description = "최대 체력이 100 증가하고, 매 턴 체력을 15 회복합니다.",
                    Tier = ArtifactTier.일반,
                    OnEquip = (player) =>
                    {
                        player.MaxHp += 100;
                    },
                    OnTurnStart = (player, bs) =>
                    {
                        player.Hp += 15;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                    }
                }
        },
        // 어깨뼈
        {"두더지의 어깨뼈", new Artifacts
                {
                    Name = "두더지의 어깨뼈",
                    Description = "공격력이 5증가하고, 매 턴 체력을 5 회복합니다",
                    Tier = ArtifactTier.영웅,
                    OnEquip = (player) =>
                    {
                        player.Attack += 5;
                    },
                    OnTurnStart = (player, bs) =>
                    {
                        player.Hp += 5;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                    }
                }
        },
        // 불깃
        {"불사조의 깃털", new Artifacts
                {
                    Name = "불사조의 깃털",
                    Description = "치명타확률이 10% 증가하고, 치명타 발동시 체력을 15 회복합니다.",
                    Tier = ArtifactTier.영웅,
                    OnEquip = (player) =>
                    {
                        player.CritChance += 0.10;
                    },
                    OnDealDamage = (player, bs) =>
                    {
                        if (player.IsCrit)
                        {
                            player.Hp += 15;
                            if (player.Hp > player.MaxHp)
                            {
                                player.Hp = player.MaxHp;
                            }
                        }
                        
                    }
                }
        },
        // 이갑
        {"이글거리는 갑옷", new Artifacts
                {
                    Name = "이글거리는 갑옷",
                    Description = "열기 데미지에 면역이 됩니다.",
                    Tier = ArtifactTier.영웅,
                    OnEquip = (player) =>
                    {
                        player.IsHeatImmune = true;
                    }
                }
        },
        // 용딱
        {"용암 등딱지", new Artifacts
                {
                    Name = "용암 등딱지",
                    Description = "최대 체력이 50 증가하고, 피격시 공격력만큼 피해를 반사합니다.",
                    Tier = ArtifactTier.영웅,
                    OnEquip = (player) =>
                    {
                        player.MaxHp += 50;
                    },
                    OnReflect = (player, monster, bs) =>
                    {
                        bs.TakeDamage(player, monster, player.Attack);
                    }
                }
        },
        // 어머니
        {"대자연의 어머니의 유품", new Artifacts
                {
                    Name = "대자연의 어머니의 유품",
                    Description = "공격력이 15 증가하고, 턴 당 체력을 10 회복하며, 몬스터 처치시 모든 체력을 회복합니다.",
                    Tier = ArtifactTier.전설,
                    OnEquip = (player) =>
                    {
                        player.Attack += 15;
                    },
                    OnTurnStart = (player,bs) =>
                    {
                        player.Hp += 10;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                    },
                    OnMonsterDeath = (player, bs) =>
                    {
                        player.Hp = player.MaxHp;
                    }
                }
        },
        // 아버지
        {"대자연의 아버지의 유품", new Artifacts
                {
                    Name = "대자연의 아버지의 유품",
                    Description = "회피 확률이 10% 증가하고, 턴 당 마나를 15 회복하며, 몬스터 처치시 모든 마나를 회복합니다.",
                    Tier = ArtifactTier.전설,
                    OnEquip = (player) =>
                    {
                        player.EvadeChance += 0.10;
                    },
                    OnTurnStart = (player,bs) =>
                    {
                        player.Mp += 15;
                        if (player.Mp > player.MaxMp)
                        {
                            player.Mp = player.MaxMp;
                        }
                    },
                    OnMonsterDeath = (player, bs) =>
                    {
                        player.Mp = player.MaxMp;
                    }
                }
        },
        // 망토
        {"고대 엘프 망토", new Artifacts
                {
                    Name = "고대 엘프 망토",
                    Description = "최대 체력이 100 증가하고 치명타와 회피 확률이 20% 증가합니다.",
                    Tier = ArtifactTier.전설,
                    OnEquip = (player) =>
                    {
                        player.MaxHp += 100;
                        player.EvadeChance += 0.20;
                        player.CritChance += 0.20;
                    }
                }
        },
        // 대심
        {"대지의 심장", new Artifacts
                {
                    Name = "대지의 심장",
                    Description = "최대 체력이 200 증가하고, 턴 당 체력을 30 회복합니다.",
                    Tier = ArtifactTier.전설,
                    OnEquip = (player) =>
                    {
                        player.MaxHp += 200;
                    },
                    OnTurnStart = (player, bs) =>
                    {
                        player.Hp += 30;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                    }
                }
        },
        // 가호
        {"나상욱의 가호", new Artifacts
                {
                    Name = "나상욱의 가호",
                    Description = "최대 체력과 마나가 1000 증가하고 즉시 1000씩 회복합니다. 공격력이 100, 치명타/회피 확률이 100% 증가하며, 턴 당 체력을 100 회복합니다.",
                    Tier = ArtifactTier.권능,
                    OnEquip = (player) =>
                    {
                        player.Attack += 100;
                        player.MaxHp += 1000;
                        player.Hp += 1000;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                        player.Mp += 1000;
                        if (player.Mp > player.MaxMp)
                        {
                            player.Mp = player.MaxMp;
                        }
                        player.CritChance += 1;
                        player.EvadeChance += 1;
                    },
                    OnTurnStart = (player, bs) =>
                    {
                        player.Hp += 100;
                        if (player.Hp > player.MaxHp)
                        {
                            player.Hp = player.MaxHp;
                        }
                    }
                }
        },
    };

}
