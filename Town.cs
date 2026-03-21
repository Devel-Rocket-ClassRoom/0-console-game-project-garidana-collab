using System;
using System.Net;
using static System.Console;


public class Town // 천호 마을
{
    List<Artifacts> artifacts = new();


    TownShop TS = new();
    public Town()
    {
        artifacts.Add(new Artifacts
        {
            Name = "거인의 정수",
            Description = "최대 체력이 20 증가합니다.",
            Tier = ArtifactTier.Common,
            OnEquip = (player) =>
            {
                player.MaxHp += 20;
                player.Hp += 20;
            }
        });
        artifacts.Add(new Artifacts
        {
            Name = "응징의 낙인",
            Description = "공격력이 5 증가합니다.",
            Tier = ArtifactTier.Common,
            OnEquip = (player) =>
            {
                player.Attack += 5;
            }
        });
        artifacts.Add(new Artifacts
        {
            Name = "기회의 열쇠",
            Description = "치명타 확률과 회피 확률이 5% 증가합니다.",
            Tier = ArtifactTier.Common,
            OnEquip = (player) =>
            {
                player.CritChance += 0.05;
                player.EvadeChance += 0.05;
            }
        });
    }
    // 직업 선택 및 아티팩트 선택 메서드
    public Player Enter()
    {
        Player p = null;

        while (p == null)
        {
            Clear();
            WriteLine("====== 김경일의 모험 ver 0.0.5 ======");
            WriteLine();
            WriteLine("          평화로운 천호 마을            ");
            WriteLine();
            WriteLine("=====================================");
            WriteLine();
            WriteLine("<<   직업을 선택하세요   >>");
            WriteLine();
            WriteLine("[1] 전사 (쉬움)");
            WriteLine("\t생존 중심. 피해를 버티며 분노 강타로 역전을 노린다.");
            WriteLine("[2] 마법사 (쉬움)");
            WriteLine("\t공격력 중심. 마나 관리로 마나 작렬 타이밍을 노리는 전략형.");
            WriteLine("[3] 궁수 (어려움)");
            WriteLine("치명타/회피 중심. 은신으로 확정 치명타를 노리는 공격형.");
            WriteLine();
            Write(">> ");
            string selection = Console.ReadLine();
            WriteLine();
            if (selection == "1")
            {
                WriteLine("[전사]");
                WriteLine("HP : 120 / MP : 80 / ATT : 18 \n치명타 10% / 회피 10%");
                WriteLine("스킬 : 베기 / 방어 태세 / 전투 함성 / 분노 강타");
                WriteLine();
            }
            else if (selection == "2")
            {
                WriteLine("[마법사]");
                WriteLine("HP : 100 / MP : 120 / ATT : 20 \n치명타 10% / 회피 10%");
                WriteLine("스킬 : 비전구 / 마나 실드 / 마나 작렬 / 저주 : 대혼란 / 명상");
                WriteLine();
            }
            else if (selection == "3")
            {
                WriteLine("[궁수]");
                WriteLine("HP : 80 / MP : 100 / ATT : 18 \n치명타 20% / 회피 20%");
                WriteLine("스킬 : 관통 화살 / 암살자의 발재간 / 심장 터뜨리기 / 은신 / 예리한 눈");
                WriteLine();
            }
            else
            {
                WriteLine("올바른 숫자를 입력하세요");
                continue;
            }
            WriteLine("[1] 직업 확정");
            WriteLine("[2] 다시 선택");
            WriteLine();
            Write(">> ");
            string finalSelect = Console.ReadLine();
            if (finalSelect == "1")
            {
                if (selection == "1")
                {
                    p = new Warrior("김경일");

                }
                if (selection == "2")
                {
                    p = new Mage("김경일");
                }
                if (selection == "3")
                {
                    p = new Ranger("김경일");
                }

            }
            else if (finalSelect == "2") 
            {
                continue;
            }
            else
            {
                WriteLine("올바른 숫자를 입력하세요");
                continue;
            }

        }
        TS.Enter(p);
        WriteLine();
        Artifacts selectedArtifact = null;
        while (selectedArtifact == null)
        {
            Clear();
            WriteLine("<<     아티팩트를 선택하세요     >>");
            WriteLine();
            for (int i = 0; i < artifacts.Count; i++)
            {
                WriteLine($"[{i + 1}] {artifacts[i].Name} - {artifacts[i].Description}");
            }
            WriteLine();
            Write(">> ");

            string artiSelect = Console.ReadLine();
            if(int.TryParse(artiSelect, out int artiChoice) && artiChoice >= 1 && artiChoice <= artifacts.Count)
            {
                Artifacts selected = artifacts[artiChoice - 1];
                selected.OnEquip(p);
                
                p.artifacts.Add(selected);
                selectedArtifact = selected;
                WriteLine($"{selectedArtifact.Name}을(를) 선택하였습니다.\n{selectedArtifact.Description}");
            }
            else
            {
                WriteLine("올바른 입력을 하세요");
                continue;
            }

        }
        
        return p;
    }
}



