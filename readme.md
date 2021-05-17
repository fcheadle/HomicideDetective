# _Homicide Detective_
_A Procedurally Generated Murder-Mystery Game_

![screenshot](https://i.imgur.com/gS5WIyC.gif "an image of how the game will look when it's done (roughly)")

Investigate small-town murders. These homicides are procedurally generated from ranges given in the .json files^(Coming Soon). This readme also serves as the design document.

[Subreddit](https://www.reddit.com/r/HomicideDetective )

### Playing

There is currently no content and nothing to do. To get it to build on your system,

1. Clone [TheSadRogue.Integration](https://github.com/thesadrogue/TheSadRogue.Integration ) locally
2. Open in Visual Studio or Rider
3. Right Click `TheSadRogue.Integration` project > `Properties...`
4. Click `Nuget > Generate Nuget Package During Build`
5. Give it the version, `1.0.0-alpha01-debug`, and build the solution
6. Find the output nuget package (at `Solution Folder/TheSadRogue.Integration/bin/Debug`)
7. Add that file location to your IDE's package sources. [Visual Studio](https://support.microsoft.com/en-us/topic/how-to-use-a-local-package-repository-79a79bd8-090e-78f6-b396-8d03cee41981 ) [Rider](https://www.jetbrains.com/help/rider/Using_NuGet.html#sources )
8. Close your IDE, and open Homicide Detective.
9. Restore Nuget Packages if it isn't building.

## Features
***NOT DONE***

- Normal game mode is Crime Scene Investigation
	- Search crimes scenes carefully for clues:
		- Photograph scenes or items
		- Dust for Prints/Check for tracks
			- Fingerprints are proc-genned arrays of glyphs
			- partial fingerprints (only a small subset of the whole) will be left on most items a person touches, and can be collected as evidence
			- if you choose, you can combine the partial prints they've collected. Do this carelessly and you'll destroy evidence, giving the killer precious time to evade justice
		- Check for tracks
		- Stake out and see if anyone returns to the scene
	- Bring in the K9 Unit to search by scent
		- Scent spreads around and decays quickly, so hurry while the scent is strong!
	- Perpetrators who cover their tracks
		- different perpetrators have varying degrees of attention to detail
	- Interview Witnesses
		- Input is basic for now, will become much more complicated as time goes on
		- NPCs will figure out what you mean and respond in character
		- NPCs will have an ability to detect lies that varies according to skill
		- NPCs will have multiple different reasons why they might lie
		- Body language is a second aspect to dialogue. NPCs might not like you if you are stiff the whole time, and may be less likely to talk to you.
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
	- Detect Lies
	- Lie
	- Estimate time of death
	- Figure out when evidence has been tampered with

## HomicideDetective


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
![scene](https://i.imgur.com/GBLuvQE.png "A diagram of how I think scenes will work")
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


### ___[Item](https://trello-attachments.s3.amazonaws.com/5c8c0c2d11f97f1fd31710c9/5c8c0cb9c05578143e226e16/88ba490057f725bcb97ba1719ce293f9/design_item.png "Items are a fusion of game logic and vivid descriptions from the json")___
![items](https://trello-attachments.s3.amazonaws.com/5c8c0c2d11f97f1fd31710c9/5c8c0cb9c05578143e226e16/88ba490057f725bcb97ba1719ce293f9/design_item.png "a diagram showing how I'm thinking of Items")
Everything from furniture and murder weapons to signs of struggle. an Item  contains _very_ basic physics (mass & volume) and a collection of 0 or more markings such as blood, dirt, hair, scuff marks, and [Fingerprints](https://i.imgur.com/W5tyVW3.png "use spiraling cellular automata to generate a fingerprint")
- Should not change very much after initial game outline is implemented
- An 'Item' contains:
	- The physical ranges (minimum, maximum, mean, mode) that the item type is normally
	- the printable name and description of the item
	- general information about hardness, hollowness, and points that an item contains
	- information about what things can fit inside
	- zero or more fingerprints of people who touched the item

## Design Philosophy
![ecs](https://trello-attachments.s3.amazonaws.com/5c8c0c2d11f97f1fd31710c9/5c8c0cb9c05578143e226e16/00baadc7739e13c733fb3b2f797048c0/design_engine.png "A crude drawing of the concept of ECS")

To reduce the amount of clutter in my abstractions, I've decided to enforce a few things in my personal development:

- ___[Test Driven Development](https://en.wikipedia.org/wiki/Test-driven_development )___- in the event that I figure out some new feature that I wish to implement in a method, I should first write a unit test that makes sure it does what I expect in the way I expect it to. This has already been implemented and is paying off tremendously in the amount of time it takes to debug some weird shit. There is currently some decent amount of technical debt to overcome
- ___Procedural Generate Everything___ - every single thing in the game should be procedural generated - the people, the places, and the things.
- __Fully Moddable___ - All text that is displayed to the screen should be defined in some json files somewhere. By adding new files, editing or deleting existing ones, someone should be able to "reskin" the game to be about anything. Proof-of-concept should include periodic "official mods" with themes such as "Forensic Computer Science Mod", "Lovecraftian Horror Mod", "X-Files Mod", "Twilight Zone Mod", "DNA Evidence Mod"
- ___Depth over Graphics___ - inspired in this regard by Dwarf Fortress. By ignoring graphical development entirely, I can focus on implementing _deep, rich_ features that _really_ give the player a feeling of realism. Writing is one of my strong suits, so this should be a fun challenge. The text that is sent to the player should read like pulp noir, and shouldn't really _feel_ like it was generated.
- __Accessible from day One___ - Accessibility options for the blind, color blind, deaf, and people with limited motor skills in their hands should all be active from day one. The necessary text will be output to the console, and a screenreader will simply act through that.
- ___Soundtrack___ - the soundtrack should be a lot of ambient, shoe-gazey stuff that is patterned like a less-energetic The Sims soundtrack - constantly changing, unpredictable, potentially, dare I say it... generated somehow?
- ___Let other people do the heavy lifting___ - other people smarter than I have spent a _lot_ of time trying to solve the tedious challenges. Build on their work whenever possible! this goes for rendering libraries (SAD console), certain game-features (GoRogue), generated text (MarkhovSharp), natural language processing, and so on.
- __Keep it Simple, Stupid___ - In my earlier iterations, I quickly found myself several layers of inheritance deep, and constantly needing to reexamine and simplify. Thanks to the concept of an [entity-component system](https://en.wikipedia.org/wiki/Entity_component_system ), It is now trivially easy to keep all of my code within a component that lives on an entity, and new components can be added quickly. It is possible that I may create three new classes (`Creature : BasicEntity`, `Item : BasicEntity`, and `Tile : BasicTerrain`), but to stick with this concept, I'm not going to do that until absolutely necessary.
- ___Open Source___ - any Tom, Dick, and Harry and [check out the code for themselves](https://github.com/fcheadle/HomicideDetective ), and it is published under the MIT open source license.
- ___Play the long game___ - since I'm developing this for fun and not really with intentions to sell it, then it's more important that I do things _the right way now_ than _quickly now and refactor later_. Refactors will always be necessary periodically, but I can reduce their frequency by adhering to best practices always, for example, I'm highly unlikely to ever refactor away from ECS unless some new world-changing paradigm emerges.

## Credits

[GoRogue](https://github.com/Chris3606/GoRogue )

[SadConsole](https://github.com/SadConsole/SadConsole )

___Special Thanks___ to

* Jeremiah Hamilton
* The SadConsole and GoRogue Discord communities
