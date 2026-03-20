
using System;
using static System.Console;

WriteLine("=== 김경일의 모험 ver 0.0.3 ===");
WriteLine("화산 동굴에 입장하였습니다.");
BattleSystem BS = new();
Warrior w = new("김경일");
Mage m = new Mage("김경일");
Ranger r = new("김경일");
VolcanoMole vc = new();
SulfuricSlime ss = new();
FireSpirit fs = new();
//MagmaTorToise mt = new();
//EarthWorm ew = new();
BS.RunBattle(m, fs);
//BS.RunBattle(w,ss);
//BS.RunBattle(w,fs);
//BS.RunBattle(w,mt);
//BS.RunBattle(w.ew);

