# Coach

>I am aware that parts of code are violating many principles(some methodes should be in separate classes), but as whole project was just a proof of concept.

Unusefull and pointless code that will folow your progress in CS:GO, and hopefully will be able to help you train.

Created using [Json.NET Newtonsoft](https://www.newtonsoft.com/json) (for parsing and serializing json data), and [CSGSI](https://github.com/rakijah/CSGSI) (for game state integration in CS:GO).

## How to use
You have to place a "gamestate_integration_gocoach.cfg" file into your Steam\steamapps\common\Counter-Strike Global Offensive\csgo\cfg folder.

>Make sure nothing is blocking connection on localhost:3000 or change port in "gamestate_integration_gocoach.cfg" and constructor of MainProgram ( GameStateListener(your_port) ).

Run program with Administrator privileges.

Run CS:GO

When you're done playing press "Save to file" button.

>Safe to use alongside VAC, Faceit Anticheat, ESEA or EAC.

## What it does
It will create folder C:\Training in which data from every day will be saved. 




