
using System;
using static System.Console;

// 화산 두더지 스탯 (hp 150 att  15)
// 유황 슬라임 스탯 (hp 190 att 15)
// 불의 정령 스탯 (hp 250 att 18)
// 용암 거북 스탯 (hp 320 att 20)
// 지구 지렁이 스탯 (hp 400 att 25)

WriteLine("=== 김경일의 모험 ver 0.0.2 ===");
WriteLine("화산 동굴에 입장하였습니다.");
BattleSystem BS = new();
Warrior w = new("김경일");
Mage m = new Mage("김경일");
Ranger r = new("김경일");
VolcanoMole vc = new();
//SulfuricSlime ss = new();
//FireSpirit fs = new();
//MagmaTorToise mt = new();
//EarthWorm ew = new();
BS.RunBattle(m, vc);
//BS.RunBattle(w,ss);
//BS.RunBattle(w,fs);
//BS.RunBattle(w,mt);
//BS.RunBattle(w.ew);

