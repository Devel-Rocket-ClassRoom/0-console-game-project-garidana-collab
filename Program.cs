Console.WriteLine("Hello, World!");
using System;
using static System.Console;

// 화산 두더지 스탯 (hp  80 att  12)
// 유황 슬라임 스탯 (hp 100 att 15)
// 불의 정령 스탯 (hp 120 att 18)
// 용암 거북 스탯 (hp 150 att 20)
// 지구 지렁이 스탯 (hp 400 att 25)

WriteLine("=== 김경일의 모험 ver 0.0.2 ===");
WriteLine("화산 동굴에 입장하였습니다.");
BattleSystem BS = new();
Warrior p = new("김경일");
VolcanoMole vc = new();
BS.RunBattle(p, vc);
//Monster m2 = new("유황 슬라임", 190, 20, ConsoleColor.DarkMagenta);
//BS.RunBattle(p, m2);


