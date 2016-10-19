Megavaders 5000 
The project is deeply inspired by Space Invaders(TM).  

Basic Features:
* Attractor Mode entry screen indicating High Score and Enemy Value
* End Game High Scores Collection
* Basic retro-space-shooter mechanics

Project Setup:
* Input Manager:  
  Required input axis setup: Fire1, Horizontal, Vertical
  Optional: Fire2, Horizontal2,Vertical2, Exit
  When loaded, the project will provide a debug warning if any values are missing. 
* Tags:  In the Tag manager, Enemy and Shield must both be declared.  If these are not declared, the
  game will most likely still run in the Editor by pressing "Play", however it will throw exception
  errors if played via Build
* Build Settings:
  Make sure the following scenes exist in the Build Settings->Scenes In Build window: 
     GameStart, GamePlay, GameOver; They are found in the SCENES folder.	
  GameStart is the entry/first Scene.
* Player Settings:
  Aspect Ratios of 4x3, 16x9, and 16x10 are supported; others are untested.



Additional Notes:
* High Scores may be cleared by using the Megavaders 5000/Clear High Scores menu option from the Editor Window.
* Megavaders 5000 is a shared game on the Lexitron, a custom built arcade cabinet.  As such,
  there are a couple play-continuity features:
  ** Auto Kill Timer: Auto-quits the game after a set period of time (90 seconds)
     Auto-kill engaged if AUTO_KILL_GAME is defined in:  Player Settings->Configuration->Scripting Define Symbols
  ** The High Scores collection also has a timer on it, where if input is not captured
     prior to timeout, then the high score text is auto-saved and the game continues to the intro screen.

    

Known Issues:
* When playing in Unity 5.3x Editor, SPACEBAR may drop down the Displays menu.  This will happen if
  you poke the Display drop down in the Game Window, and is an obvious problem if SPACEBAR is setup
  for use in the Input Manager.
  http://forum.unity3d.com/threads/spacebar-in-game-window-in-5-3.374214/
  https://www.reddit.com/r/Unity3D/comments/3x349v/unity_53_problem_pressing_space_brings_up_multi/
  To Fix:  Close the project and reopen it.
  

Credits / Thanks To:
* RunJumpDev crew for allowing Megavaders 5000 onto the Lexitron
* Press Start 2P Regular Font maker Cody Codeman38 Boisclair
