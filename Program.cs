
using System;
using static System.Console;

WriteLine("====== 김경일의 모험 ver 0.0.4 ======");
WriteLine();
WriteLine("<<화산 동굴>>에 입장하였습니다.");
WriteLine();
WriteLine("동굴 내부의 온도가 꽤나 높습니다...");
BattleSystem BS = new();
Warrior w = new("김경일");
Mage m = new Mage("김경일");
Ranger r = new("김경일");
VolcanoMole vm = new();
SulfuricSlime ss = new();
FireSpirit fs = new();
MagmaTortoise mt = new();
//EarthWorm ew = new();
//BS.RunBattle(r, vm);
//BS.RunBattle(r,ss);
BS.RunBattle(r,mt);
//BS.RunBattle(w,mt);
//BS.RunBattle(w.ew);


