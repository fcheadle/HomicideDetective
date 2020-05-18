# _Homicide Detective_
_A Procedurally Generated Murder-Mystery Game_ where you are investigate small-town murders. These homicides are procedurally generated from ranges given in the .json files^(Coming Soon).

[This project has a board on Trello for development](https://trello.com/invite/b/qngR0CGL/35e762327185af78bdd2959332b87e0d/homicide-detective). This board is up to date, but perhaps not granular enough on what I'm working on _right now_.

[And a subreddit!](https://www.reddit.com/r/HomicideDetective)

### Playing
Clone it, run it. More later.

## Features
***NOT DONE***

- Normal game mode is Crime Scene Investigation
	- Search crimes scenes carefully for clues:
	- Photograph scenes or items
	- Dust for Prints/Check for tracks
		- Fingerprints are proc-genned arrays of glyphs
		- partial fingerprints (only a small subset of the whole) will be left on most items a person touches, and can be collected as evidence
		- if you choose, you can combine the partial prints they've collected. If you do this carelessly, you will destroy evidence, and give the killer precious time to evade justice
	- Check for tracks
	- Stake out and see if anyone returns to the scene
	- bring in the K9 Unit to search by scent
		- Scent spreads around and decays quickly, so hurry while the scent is strong!
	- perpetrators who cover their tracks
		- different perpetrators have varying degrees of attention to detail
	
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

- Secret Skills
	- Hide/Disguise
	- Imitate someone else's voice
	- Conduct effective interrogations
	- Read Body Language / Send Specific Body Language
	- Estimate time of death
	- Figure out when evidence has been tampered with
	- Detect when someone is lying to you

## Engine
![design photo](https://i.imgur.com/v94gmcf.png "ugly, possibly incorrect diagram of how i think ECS works")
- Handles the Game-y components
- Render to the screen
- Pathfinding
- tracking status over time
- handles keyboard / mouse input and accessibility options
- handles saving and loading
- has all utilities necessary to interface with our gamey libraries so our game itself doesn't have to
- needs to accept implementations of interfaces

## HomicideDetective
![old way of thinking about the design](https://i.imgur.com/JtDiwpL.png "Still relevant today")
- Handles the statistical modeling/simulation, as well as evidence, witnesses, dialogue, so on

### ___[Case](https://i.imgur.com/uKsEYwv.png "Cases contain the Who, What, Where, Why, When, and How of the murder")___ 
An `object` which holds references to all people, places, and things related to a single murder. All of a case's data must be contained within a single `map`
	- Cases work by creating generic arrays of items, scenes, and persons, and then creating arrays of `Relationships` between them. These `Relationships` should contain only the keys of the various members to make them easier to search.
	- All persons/items/scenes have a `key` property: the case number they are associated with, and the other being an incrementing id. We should be able to access these by calling `game.cases[caseNumber].items[itemId]`
	- A Case's ids are the integer we get from decoding the detective's name to a base 36 number, and their place in the array.
	- Therefore, two people playing a game with a detective named "John Anderson" should have the exact same cases.
	
### ___[Scene](https://i.imgur.com/5RAWscZ.png "Scenes, once generated according to the map, are really only important for building strings that are given to the player")___ 
A scene is a place where something happened, or a place where you can go to talk to witnesses or other persons of interest. Scenes contain items and connect to other scenes. A scene's file will contain all printable strings, as well as size descriptions, what items are contained within, and what other scenes are connected to this scene. These will (hopefully) not expand in functionality very much beyond the initial game skeleton.

A 'Scene' contains:
- A list of items contained within that are unrelated to the case, but generate based on the json definitions 
  - An office will contain a desk, chair, clutter, things to go on the desk, things to go in it's drawers, etc
- A list of connections to other scenes 
  - (i.e. a bedroom has a hallway, bathroom, and closet connections)
  
### ___[Person](https://i.imgur.com/9VhsjTs.png "a person can be thought of as a collection of decision-making components and metabolic components")___ 
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
  

### ___Item___ 
Everything from furniture and murder weapons to signs of struggle and blood spatter. an Item  contains  _very_ basic physics (mass & volume) and a collection of 0 or more markings such as blood, dirt, hair, scuff marks, and [Fingerprints](https://i.imgur.com/W5tyVW3.png "use spiraling cellular automata to generate a fingerprint")
- Should not change very much after initial game outline is implemented
- An 'Item' contains:
	- The physical ranges (minimum, maximum, mean, mode) that the item type is normally
	- the printable name and description of the item
	- general information about hardness, hollowness, and points that an item contains
	- information about what things can fit inside
	- zero or more fingerprints of people who touched the item


#### dialogue
This contains the markov corpus that powers speech, as well as truth value assessment words, speech affectations, and so on. For each language that the game must be translated into, you need a new corpus in that language.

## Design Philosophy

To reduce the amount of clutter in my abstractions, I've decided to enforce a few things in my personal development:

- ___[Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development)___ - in the event that I figure out some new feature that I wish to implement in a method, I should first write a unit test that makes sure it does what I expect in the way I expect it to. This has already been implemented and is paying off tremendously in the amount of time it takes to debug some weird shit. There is currently some decent amount of technical debt to overcome
- ___Procedural Generate Everything___ - every single thing in the game should be procedural generated - the people, the places, and the things. 
- __Fully Moddable___ - All text that is displayed to the screen should be defined in some json files somewhere. By adding new files, editing or deleting existing ones, someone should be able to "reskin" the game to be about anything. Proof-of-concept should include periodic "official mods" with themes such as "Forensic Computer Science Mod", "Lovecrafting Horror Mod", "X-Files Mod", "Twilight Zone Mod", "DNA Evidence Mod"
- ___Depth over Graphics___ - inspired in this regard by Dwarf Fortress. By ignoring graphical development entirely, I can focus on implementing _deep, rich_ features that _really_ give the player a feeling of realism. Writing is one of my strong suits, so this should be a fun challenge. The text that is sent to the player should read like pulp noir, and shouldn't really _feel_ like it was generated. 
- __Accessible from day One___ - Accessibility options for the blind, color blind, deaf, and people with limited motor skills in their hands should all be active from day one. The necessary text will be output to the console, and a screenreader will simply act through that.
- ___Soundtrack___ - the soundtrack should be a lot of ambient, shoe-gazey stuff that is patterned like a less-energetic The Sims soundtrack - constantly changing, unpredictable, potentially, dare I say it... generated somehow?
- ___Let other people do the heavy lifting___ - other people smarter than I have spent a _lot_ of time trying to solve the tedious challenges. Build on their work whenever possible! this goes for rendering libraries (SAD console), certain game-features (GoRogue), generated text (MarkhovSharp), natural language processing, and so on.
- __Keep it Simple, Stupid___ - In my earlier iterations, I quickly found myself several layers of inheritance deep, and constantly needing to reexamine and simplify. Thanks to the concept of an [entity-component system](https://en.wikipedia.org/wiki/Entity_component_system ), It is now trivially easy to keep all of my code within a component that lives on an entity, and new components can be added quickly. It is possible that I may create three new classes (`Creature : BasicEntity`, `Item : BasicEntity`, and `Tile : BasicTerrain`), but to stick with this concept, I'm not going to do that until absolutely necessary.
- ___Open Source___ - any Tom, Dick, and Harry and [check out the code for themselves](https://github.com/fcheadle/HomicideDetective ), and it is published under the MIT open source license.
- ___Play the long game___ - since I'm developing this for fun and not really with intentions to sell it, then it's more important that I do things _the right way now_ than _quickly now and refactor later_. Refactors will always be necessary periodically, but I can reduce their frequency by adhering to best practices always.