# Unity Project for BCI Kart Game

This is the repositories of the Unity game used in the [Motor Imagery Experiment](https://github.com/xEvheMary/MI-BCI-UnityKart.git)

The game is a modified version of [Unity's Karting Microgame](https://learn.unity.com/project/karting-template).

![](https://github.com/xEvheMary/MI-BCI-UnityKart/blob/main/UnityBCIKart%20(2).gif)

# Software

Unity 2021.3.20f1

# Details
## How to use the build
Details on how to use the game can be found in this [repository](https://github.com/xEvheMary/MI-BCI-UnityKart.git)

## Modification
### Customized GameManager --> TestManager
TestManager Prefab has additional objects
* GameHUD has the optional bar and arrow feedback
* Above feedback visibility can be toggled in the in-game menu
* The in-game menu is also customized (sound and feedback toggle, remove control image toggle)
* Add a waiting screen (wait for either a space button or an event from OpenVibe)

TestManager codes:
* Add wait state
* Add button to reset position (usually for test scene), button is R

### Kart
Kart prefab has additional objects
* LSL Input converts LSL signal to commands
* Path Steer is for lane following.

### LSL Communication
Use this LSL asset from OpenVibe's [gitlab](https://gitlab.inria.fr/openvibe/unity-games/LSL4Unity)

Example applications can be seen in the same GitLab.

LSL Communication Prefab has modifications in the object:
* Controller script is modified from LSL's controller which sends the event to the Kart LSL script.

### Lane Following
Use lane creator unity library : [Asset](https://api.unity.com/v1/oauth2/authorize?client_id=asset_store_v2&locale=en_US&redirect_uri=https%3A%2F%2Fassetstore.unity.com%2Fauth%2Fcallback%3Fredirect_to%3D%252Fpackages%252Ftools%252Futilities%252Fb-zier-path-creator-136082&response_type=code&state=a0dbba14-00a0-45a5-bb72-ae1a0b88aa50)

Path prefab is made to follow the custom track.

### Environment Toggle (Test Only)
For the free mode toggle, remove all environment-related objects, making the game free-roam.

### Auxiliary
* Camera control: change to first person PoV (optional), button is C, press again to go back to first person PoV
* Change of environment
* Customized track
* Customized menu, 2 type of game modes
* Invisible barriers to prevent the kart going to the wrong part of the track (test only)


