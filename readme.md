# _Homicide Detective_

_A Procedurally Generated Murder-Mystery Game_

## A game about a homicide detective in 1970

Homicide Detective is a text-based game where you are a detective who investigates homicides. These homicides are generated somewhat randomly from ranges given in the .json files. (Coming Soon)

[This project has a board on Trello for development](https://trello.com/invite/b/qngR0CGL/35e762327185af78bdd2959332b87e0d/homicide-detective). This board is up to date, but perhaps not granular enough on what I'm working on _right now_.

[And a subreddit!](https://www.reddit.com/r/HomicideDetective)

### Playing
Clone it, run it. More later.

### Features

***NOT DONE***

- Normal game mode is Crime Scene Investigation
	- Search crimes scenes carefully for clues:
	- Photograph scenes or items
	- Dust for Prints
		- Fingerprints are proc-genned arrays of glyphs
		- partial fingerprints, only a small subset of the whole, will be left on most items a person touches
		- partial fingerprints can be collected as evidence
		- if the player chooses, they can combine the partial prints they've collected
		- if a player does this carelessly, they will destroy evidence, and give the killer precious time to book town
	- Check for tracks
	- Stake out and see if anyone returns to the scene
	- bring in the K9 Unit
	- perpetrators who cover their tracks with varying degrees of attention to detail
	
- Intricate NPC Dialogue:
	- Input is basic for now, will become much more complicated as time goes on
	- NPCs will figure out what you mean and respond in character
	- NPCs will have an ability to detect lies that varies according to skill
	- NPCs will have multiple different reasons why they might lie
	- NPCs will remember who/what they saw at any given point that they've walked before (maybe not correctly)

- Customizeable Settings:
	- Blind mode / screen reader
	- language selector
	- view scoreboard
	- graphics settings
	- sound settings

#### cases
A case is a single murder and all related information, people, items, and scenes.

A 'Case' contains:
- A victim
- A murderer
- A murder weapon
- A scene where the body is found (probably victim's house or work)
- A scene where the murder happened (probably same as where the body is found)

#### text 
These are person names, written text, and menu options. Menu files are there to support translations.

#### scenes 
A scene is a place where something happened, or a place where you can go to talk to witnesses or other persons of interest. Scenes contain items and connect to other scenes. A scene's file will contain all printable strings, as well as size descriptions, what items are contained within, and what other scenes are connected to this scene. These will (hopefully) not expand in functionality very much beyond the initial game skeleton.

A 'Scene' contains:
- A list of items contained within that are unrelated to the case, but generate based on the json definitions 
  - An office will contain a desk, chair, clutter, things to go on the desk, things to go in it's drawers, etc
- A list of connections to other scenes 
  - (i.e. a bedroom has a hallway, bathroom, and closet connections)

#### persons 
A person requires certain tissues to be functional and a certain amount of bodily fluids to keep from dying. They must also be kept at a decent temperature, or they can die from exposure. A person file in the json will contain skin colors, eye colors, special words that get inserted into conversation more often than others, facial feature descriptions, and more. These will become more complex as more features are added.

A 'Person' contains:
- A name
- Facial Feature Descriptions
- A generated fingerprint
- A collection of scenes that is the person's home
- A collection of Scenes that is the person's work
- A collection of connections to other people (family)
- A collection of connections to other people (friends)
- A collection of connections to other people (coworkers)
- A collection of connections to other people (adversaries)
- A collection of connections to other people (lovers)
- A group of percentile values that represent how likely some trait is to become a motive
  - Jealousy, Rage, heartbreak, etc

#### dialogue
This contains the markov corpus that powers speech, as well as truth value assessment words, speech affectations, and so on. For each language that the game must be translated into, you need a new corpus in that language.

#### items
the types of items in the game, from furniture and murder weapons to signs of struggle and blood spatter. Should not change very much after initial game outline is implemented

An 'Item' contains:
- The physical ranges (minimum, maximum, mean, mode) that the item type is normally
- the printable name and description of the item
- general information about hardness, hollowness, and points that an item contains
- information about what things can fit inside
- zero or more fingerprints of people who touched the item
