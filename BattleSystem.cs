using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Linq;
using static System.Console;

// BattleSystem.cs


public class BattleSystem
{
    private Random _random = new Random();
    public int Round = 0;
    public bool IsCrit;
    public bool IsEvade;


    //몬스터 아티팩트 드랍 딕셔너리의 딕셔너리
    Dictionary<string, Dictionary<ArtifactTier, string>> DropPool = new()
    {
        {
            "화산 두더지", new Dictionary<ArtifactTier, string>
            {
                {ArtifactTier.일반, "두더지 발톱" },
                {ArtifactTier.영웅, "두더지의 어깨뼈" },
                {ArtifactTier.전설, "대자연의 어머니의 유품" },
                {ArtifactTier.권능, "나상욱의 가호" }
            }
        },
        {
            "유황 슬라임", new Dictionary<ArtifactTier, string>
            {
                {ArtifactTier.일반, "유황 점액 덩어리" },
                {ArtifactTier.영웅, "불사조의 깃털" },
                {ArtifactTier.전설, "대자연의 아버지의 유품" },
                {ArtifactTier.권능, "나상욱의 가호" }
            }
        },
        {
            "불의 정령", new Dictionary<ArtifactTier, string>
            {
                {ArtifactTier.일반, "과열된 마나 정수" },
                {ArtifactTier.영웅, "이글거리는 갑옷" },
                {ArtifactTier.전설, "고대 엘프 망토" },
                {ArtifactTier.권능, "나상욱의 가호" }
            }
        },
        {
            "용암 거북", new Dictionary<ArtifactTier, string>
            {
                {ArtifactTier.일반, "거북이 골수" },
                {ArtifactTier.영웅, "용암 등딱지" },
                {ArtifactTier.전설, "대지의 심장" },
                {ArtifactTier.권능, "나상욱의 가호" }
            }
        },
    };


    // 전투 실행
    public bool RunBattle(Player player, Monster monster)
    {
        
        // 전투 시작시 모든 지속효과 삭제
        foreach (var se in player.statusEffects)
        {
            if (se.OnExpire != null)
            {
                se.OnExpire(player,this);
            }
        }
        // 전투 시작시 쿨타임 리셋
        foreach (var skill in player.skills)
        {
            skill.CurrentCD = 0;
        }
        player.statusEffects.Clear();

        // 새로운 전투시 체력 및 마나 회복
        if (player.Hp <= player.MaxHp * 0.5 && player.Hp > 0)
        {
            player.Hp += 30;
            if (player.Hp > player.MaxHp)
            {
                player.Hp = player.MaxHp;
            }
        }
        if (player.Mp <= player.MaxMp * 0.5 && player.Mp > 0)
        {
            player.Mp += 30;
            if (player.Mp > player.MaxMp)
            {
                player.Mp = player.MaxMp;
            }
        }

        Clear();
        if (monster.IsFinalBoss)
        {
            WriteLine("태초의 아티팩트가 위험을 감지한듯 공명하기 시작합니다...");
            WriteLine("최후의 전투를 준비합니다....");
            WriteLine("마음을 굳게 먹고 \"모든 준비는 끝났다\"를 입력하세요...");
            WriteLine();
        }
        else
        {
            WriteLine("저 멀리 무언가 보입니다.");
            WriteLine("전투를 준비합니다... 아무키나 누르세요");
            WriteLine();
        }
        WriteLine("== 현재 능력치 ==");
        WriteLine($"체력: {player.Hp}/{player.MaxHp}  마나: {player.Mp}/{player.MaxMp}");
        WriteLine($"공격력 : {player.Attack}  치명타 : {player.CritChance * 100:F0}%  회피율 : {player.EvadeChance * 100:F0}%");
        WriteLine();
        // 보유 아티팩트 목록
        if (player.artifacts.Count > 0)
        {
            WriteLine($"== 보유 아티팩트 ==");

            foreach (var artifact in player.artifacts)
            {
                WriteLine($"- [{artifact.Tier}] {artifact.Name}\n   {artifact.Description}");
            }
        }
        Write(">> ");
        ReadLine();
       
        Console.Clear();
        Console.WriteLine($"\n>>>>> {monster.Name} 출현! <<<<<\n");
        // 불의 저령이후 몬스터들의 전용 출현 메시지
        if (monster.HeatDamage > 0)
        {
            WriteLine("-------------------------------------");
            WriteLine("엄청난 열기가 느껴집니다. 중심부에 가까워지는 느낌입니다.");
            WriteLine($"라운드 마다 {monster.HeatDamage}의 열기 피해를 입습니다.");
        }
        else if (monster.IsFinalBoss)
        {
            WriteLine("-------------------------------------");
            WriteLine("견디지 못할것 같은 열기입니다. 거대한 괴물이 용암속에서 등장합니다.");
            WriteLine($"라운드 마다 {monster.HeatDamage}의 열기 피해를 입습니다.");
        }

        while (player.IsAlive && monster.IsAlive)
        {
           

            WriteLine("-------------------------------------");
            if (Round == 0)
            {
                WriteLine("<전투 시작>\n");
            }
            else
            {
                WriteLine($"<{Round} 라운드>\n");
            }
            // 플레이어 턴

            // 현재 상태 출력
            Console.WriteLine($"[{player.Name}] HP: {player.Hp}/{player.MaxHp} MP: {player.Mp}/{player.MaxMp}");

            Console.WriteLine($"[{monster.Name}] HP: {monster.Hp:F0}/{monster.MaxHp:F0}");
            WriteLine();

            // 플레이어 지속효과 지속시간 감소 
            foreach (var se in player.statusEffects)
            {
                if (se.Duration > 0)
                {
                    se.Duration--;
                }
            }
            var pExpired = player.statusEffects.Where(se => se.Duration <= 0).ToList();

            // OnExpire 플레이어 만료된 지속효과 
            foreach (var se in pExpired)
            {
                if (se.Duration <= 0 && se.OnExpire != null)
                {
                    se.OnExpire(player, this);
                }
            }
            // 플레이어 지속시간이 다 되면 지속효과 삭제
            player.statusEffects.RemoveAll(se => se.Duration <= 0);

            // 플레이어 현재 적용 되는 지속효과 출력
            player.PrintStatusEffects();
            player.IsIncap = false;
            // 플레이어 잔여 지속효과 체크 후 적용
            foreach (StatusEffect effect in player.statusEffects)
            {
                if (effect.OnTurnStart != null)
                {
                    effect.OnTurnStart(player, this);
                }
            }
            // OnTurnStart 아티팩트 효과 발동
            foreach (var artifact in player.artifacts)
            {
                if (artifact.OnTurnStart != null)
                {
                    artifact.OnTurnStart(player, this);
                }
            }
            // 플레이어 전투불능 상태 일시 메세지 출력 후 턴 스킵
            if (player.IsIncap)
            {
                WriteLine($"{player.Name}은 현재 전투 불능 상태이다.. 아무것도 하지 못한다..");
                WriteLine();
                Thread.Sleep(500);
            }
            else
            {
                // 스킬 선택
                player.PrintSkills(); // 플레이어 스킬 목록 출력
                WriteLine();
                Console.Write("선택 >> ");
                string input = Console.ReadLine(); // 스킬 선택 입력 받음
                Console.WriteLine();
                // 스킬 선택 입력 tryparse
                if (!int.TryParse(input, out int idx) || idx < 0 || idx > player.skills.Count)
                {
                    WriteLine($"올바른 숫자를 입력하세요");
                    continue;
                }
                // 인벤토리 선택 
                else if (idx == 0)
                {
                    // 인벤토리 비어있음
                    if (player.Inv.Count == 0)
                    {
                        WriteLine("인벤토리가 비어있습니다.");
                        continue;
                    }

                    else
                    {
                        int inum = 1;
                        WriteLine("-- 인벤토리 --");
                        WriteLine();
                        foreach (var i in player.Inv)
                        {
                            WriteLine($"{inum}. {i.Key} ({i.Value}개)");
                            inum++;
                        }
                        Write(">> ");
                       
                    }
                    string choice = ReadLine();
                    if (!int.TryParse(choice, out int itemChoice) || itemChoice < 0 || itemChoice > player.Inv.Count)
                    {
                        WriteLine("올바른 숫자를 입력해주세요");
                        WriteLine("아이템 외의 입력을 하면 스킬 선택으로 돌아갑니다.");
                        continue;
                    }
                    string selectedItemName = "";
                    int Inum = 1;
                    foreach (var i in player.Inv)
                    {
                        if (Inum == itemChoice)
                        {
                            selectedItemName = i.Key;
                            break;
                        }
                        Inum++;
                    }
                    WriteLine($"[{selectedItemName}]을 사용했습니다.");
                    WriteLine();

                    // 사용한 아이템 갯수 감소, 제거 및 효과 적용
                    Items.All[selectedItemName].Effect(player, this); // 효과 적용
                    player.Inv[selectedItemName]--; // 사용후 갯수 감소
                    if (player.Inv[selectedItemName] <= 0)
                    {
                        player.Inv.Remove(selectedItemName); // 갯수 =0 일때 컬렉션에서 제거
                    }
                    //continue;

                }
                else if (player.skills[idx - 1].CurrentCD > 0)
                {
                    WriteLine("스킬이 쿨타임 중입니다.");
                    continue;
                }
                else if (player.Mp < player.skills[idx - 1].ManaCost)
                {
                    WriteLine("마나가 부족합니다.");
                    continue;
                }
                // 사용가능 한 스킬 사용
                else
                {
                    // 입력받은 스킬 사용
                    Skill selectedSkill = player.skills[idx - 1];
                    // 사용할 스킬 정보 출력
                    WriteLine($"[{selectedSkill.Name}] 사용 ({selectedSkill.Description})");
                    WriteLine();
                    selectedSkill.Effect(player, monster, this);

                    // 마나 차감과 쿨타임 갱신
                    player.Mp -= selectedSkill.ManaCost;
                    selectedSkill.CurrentCD = selectedSkill.CoolDown;
                }
            }
            
            if (!monster.IsAlive)
            {
                // 몬스터 사망 발동 아티팩트 OnMonsterDeath
                foreach (var artifact in player.artifacts)
                {
                    if (artifact.OnMonsterDeath != null)
                    {
                        artifact.OnMonsterDeath(player, this);
                    }
                }
                break;
            }

            // 몬스터 턴
            // 몬스터 지속효과 지속시간 감소 및 효과 삭제
            foreach (var se in monster.statusEffects)
            {
                if (se.Duration > 0)
                {
                    se.Duration--;
                }

            }
            // 몬스터 만료된 지속효과 OnExpire 호출
            var mExpired = monster.statusEffects.Where(se => se.Duration <= 0).ToList();
            foreach (var se in mExpired)
            {
                if (se.Duration <= 0 && se.OnExpire != null)
                {
                    se.OnExpire(monster, this);
                }
            }
            monster.statusEffects.RemoveAll(se => se.Duration <= 0);

            if (monster.IsAlive)
            {
                monster.IsIncap = false;
                // 턴 시작시 적용할 지속효과가 있으면 적용
                foreach (StatusEffect effect in monster.statusEffects)
                {
                    if (effect.OnTurnStart != null)
                    {
                        effect.OnTurnStart(monster, this);
                    }
                }
                // 전투 불능 검사
                if (monster.IsIncap)
                {
                    WriteLine($"{monster.Name}은(는) 전투불능 상태이다. 아무것도 하지 못했다..");
                    
                }
                // 공격 게시
                else
                {
                    monster.ExecuteSkill(player, this);
                }
            }

            // 맵의 열기 데미지
            if (monster.HeatDamage > 0 && !player.IsHeatImmune)
            {
                int IncreasedHeat = monster.HeatDamage * 2;
                if (player.statusEffects.Any(se => se.Name == "화상"))
                {
                    player.Hp -= IncreasedHeat;
                }
                else
                {
                    player.Hp -= monster.HeatDamage;
                }
            }
            // 한 라운드 끝
            Round++; // 라운드 증가

            // 한 라운드가 끝나고 처리해야할 사항
            // 스킬 남은 쿨다운 감소
            foreach (var skill in player.skills)
            {
                if (skill.CurrentCD > 0)
                {
                    skill.CurrentCD--;
                }
            }
           

            // 마나 회복
            // Math.Min(int a, int b) 는 둘중 더 작은 값을 반환
            player.Mp = Math.Min(player.Mp + 5, player.MaxMp); // 현재 마나는 최대마나를 넘길 수 없음
            if (!player.IsAlive) break;

            //Console.Clear();
        }

        if (player.IsAlive)
        {
            Console.WriteLine($"{monster.Name}을(를) 성공적으로 처치했다....");
            Thread.Sleep(300);
            WriteLine(".");
            Thread.Sleep(300);
            WriteLine(".");
            Thread.Sleep(300);
            WriteLine(".");
            //WriteLine("전리품을 얻었습니다. 획득하려면 아무키나 누르세요");
            //ReadLine();
            DropArtifact(player,monster);
            WriteLine("전리품을 챙기고 더 나아갑니다.");
            WriteLine();
            Thread.Sleep(500);
            WriteLine("터벅");
            Thread.Sleep(500);
            WriteLine("터벅");
            Thread.Sleep(500);
            WriteLine("터벅");
            Thread.Sleep(800);
            WriteLine("터벅 X 100");
            Thread.Sleep(500);
            Round = 0;
            monster.HeatDamage = 0;
            return true;
        }
        else
        {
            if (monster.IsFinalBoss)
            {
                Console.WriteLine("절대적인 존재에겐 당해 낼 수 없었다...");
            }
            else
            {
                Console.WriteLine($"{monster.Name} 에게 플레이어가 살해당했다...");
            }
            Console.WriteLine($"GAME OVER...");
            ReadLine();
            return false;
        }
    }
    












    // 플레이어가 데미지 주는 메서드
    public void PlayerDealDamage(Character attacker, Character defender, double multiplier, double bonusDmg = 0)  // 스킬내부에서 DealDamage계산
    {
        double damage = attacker.Attack * multiplier + bonusDmg;
        // 치명타
        if (_random.NextDouble() <= attacker.CritChance)
        {
            attacker.IsCrit = true;
            damage *= 2;
        }

        WriteLine(attacker.IsCrit ? $"[{attacker.Name}]의 치명타 공격!" : $"[{attacker.Name}]의 공격!");

        // 위의 내용으로 TakeDamage 호출
        TakeDamage(attacker, defender, damage);
        // 불사조의 깃털 아티팩트 치명타시 회복 
        if (attacker is Player p)
        {
            foreach (var artifact in p.artifacts)
            {
                if (artifact.OnDealDamage != null)
                {
                    artifact.OnDealDamage(p, this);
                }
            }
        }
        attacker.IsCrit = false;
    }

    // 몬스터가 데미지 주는 메서드 (치명타 데미지 배율이 다름
    public void MonsterDealDamage(Character attacker, Character defender, double multiplier)  // 스킬내부에서 DealDamage계산
    {
        double damage = attacker.Attack * multiplier;
        // 치명타
        if (_random.NextDouble() <= attacker.CritChance)
        {
            attacker.IsCrit = true;
            damage *= 1.5;
        }
        WriteLine(attacker.IsCrit ? $"[{attacker.Name}]의 치명타 공격!" : $"[{attacker.Name}]의 공격!");

        // 위의 내용으로 TakeDamage 호출
        TakeDamage(attacker, defender, damage);
        attacker.IsCrit = false;
    }

    // 피격자 데미지 받는 메서드 // 후에 스킬 내부에서 데미지 처리
    public void TakeDamage(Character attacker, Character defender, double damage)
    {
        // 전투불능 상태에서 회피 무효
        if (defender.IsIncap)
        {
            defender.IsEvade = false;
        }
        // 피격자 회피
        else if (_random.NextDouble() <= defender.EvadeChance)
        {
            defender.IsEvade = true;
            damage = 0;
        }
        // defender가 플레이어일 때 OnTakeDamage StatusEffect를 보유하고 있는지
        if (defender is Player p)
        {
            foreach (var effect in p.statusEffects)
            {
                if (effect.OnTakeDamage != null)
                {
                    damage = effect.OnTakeDamage(damage);
                }
            }
            // 용암 등딱지 공격 반사 발동
            foreach (var artifact in p.artifacts)
            {
                if (artifact.OnReflect != null && attacker is Monster mon)
                {
                    artifact.OnReflect(p, mon, this);
                }
            }
        }
        // defender가 플레이어일 때 artifacts.OnTakeDamage가 있는지
        
        // defender가 몬스터일 때 OnTakeDamage 있는지
        if (defender is Monster m)
        {
            foreach (var effect in m.statusEffects)
            {
                // 몬스터도 데미지 감소 효과 적용 
                if (effect.OnTakeDamage != null)
                {
                    damage = effect.OnTakeDamage(damage);
                }
            }
        }

        defender.Hp -= damage;
        WriteLine(defender.IsEvade ? $"[{defender.Name}]이(가) 공격을 회피했다! 아무런 피해가 없었다.." : $"[{defender.Name}]이(가) {damage:F0} 피해를 입었다.");
        defender.IsEvade = false;
        WriteLine();



    }

    public void DropArtifact(Player p, Monster m)
    {
        Artifacts acquiredArtifact = null;
        double roll = _random.NextDouble();
        if (roll <= 1)
        {
            string artifactDropped = DropPool[m.Name][ArtifactTier.권능];
            acquiredArtifact = Artifacts.All[artifactDropped];
        }
        else if ( roll <= 0.05) 
        {
            // 티어로 아티팩트 이름 호출 및 변수 할당
            string artifactDropped = DropPool[m.Name][ArtifactTier.전설];
            acquiredArtifact = Artifacts.All[artifactDropped];
            //p.artifacts.Add(acquiredArtifact);
        }
        else if (roll <= 0.25)
        {
            string artifactDropped = DropPool[m.Name][ArtifactTier.영웅];
            acquiredArtifact = Artifacts.All[artifactDropped];
            //p.artifacts.Add(acquiredArtifact);
        }
        else //(roll <= 0.70)
        {
            string artifactDropped = DropPool[m.Name][ArtifactTier.일반];
            acquiredArtifact = Artifacts.All[artifactDropped];
            //p.artifacts.Add(acquiredArtifact);
        }
        p.artifacts.Add(acquiredArtifact);
        WriteLine($"[{acquiredArtifact.Tier.ToString()}] 등급 전리품이 나왔습니다. 확인 하려면 아무키나 누르세요");
        Console.ReadLine();
        WriteLine($"[{acquiredArtifact.Tier.ToString()}] {acquiredArtifact.Name}을(를) 획득했습니다.\n{acquiredArtifact.Description}");
        if (acquiredArtifact.OnEquip != null)
        {
            acquiredArtifact.OnEquip(p);
        }
        WriteLine();
        WriteLine("획득 하려면 아무키나 누르세요");
        WriteLine();
        Write(">> ");
        ReadLine();


    }

}