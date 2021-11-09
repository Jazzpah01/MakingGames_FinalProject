# MakingGames_FinalProject
The final project for making games.

Features:
-Combat state: waves of enemies come to attack and player can defend.
-Strategy state: player can rotate and place structures.

Controls:
-Strategy:
--B: Enter combat mode.
--I: Select wall to place.

-Combat:
--RMB: Single attack.
--Space: AOE attack.

-------------------------
Unity version used: Unity 2020.3.21
Can be downloaded here: https://unity3d.com/get-unity/download/archive

Don't use other versions of 2020.3, because we can't downgrade.

-------------------------
GIT INFORMATION:
The main branch is used only when we are making official builds. When the code has been tested and is ready.
The develop branch is where we push the features we make.

Whenever you are developing on a feature, create a branch from develop called feature/x (where x is what you are trying to make). 
Only when the feature is fully implemented will you do a pull request to develop. Example: If I want to create a movement feature, 
I will create a new branch which is a copy of develop and call it feature/movement. When I am finished making this, I will make a 
pull request and merge develop and feature/movement.

DON'T CHANGE THE MAIN SCENE. Everyone can create a personal folder called Personal inside the Scenes folder. Here you can create 
scenes that only you will be able to access. Only tech lead will be changing the main scene.

-------------------------
How to get a remote branch NAME to your computer.

git fetch origin

git checkout -b NAME origin/NAME

---
How to upload a branch NAME that is only local.

git push -u origin NAME

---
To list remove branches: git branch -v -a

https://stackoverflow.com/questions/1783405/how-do-i-check-out-a-remote-git-branch

-------------------------
Git cheat-sheet: https://www.atlassian.com/git/tutorials/atlassian-git-cheatsheet
