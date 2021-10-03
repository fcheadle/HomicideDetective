# _Homicide Detective_
_A Procedurally Generated Murder-Mystery Game_

![screenshot](https://i.imgur.com/gS5WIyC.gif "an image of how the game will look when it's done (roughly)")

Investigate small-town murders. These homicides are procedurally generated from ranges given in the .json files^(Coming Soon). This readme also serves as the design document.

[Subreddit](https://www.reddit.com/r/HomicideDetective )

### Playing

There is currently no content and nothing to do. To play it, you must run it in visual studio / rider / other C# ide.

## Features
***NOT DONE***

- Normal game mode is Crime Scene Investigation
	- Search crimes scenes carefully for clues:
		- Photograph scenes or items
		- Dust for Prints/Check for tracks
			- Fingerprints are proc-genned arrays of glyphs
			- partial fingerprints (only a small subset of the whole) will be left on most items a person touches, and can be collected as evidence
			- Combine the partial prints they've collected. (Do this carelessly and you'll destroy evidence)
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

## HomicideDetective


### Mystery
An class which holds references to all people, places, and things related to a single murder. 
- Only `Mystery` knows how a muder occurs and how to generate one procedurally.
- A `Mystery` should be the only thing which instantiates most of the other classes. 
- A Mystery's ids are the integer we get from decoding the detective's name to a base 36 number, and a Case Number which serve as seeds for deterministic generation
- Therefore, two people playing a game with a detective named "John Anderson" should have the exact same cases.

### Substantive
a Substantive is a person, place, or thing. This should be the object that is built from json strings, and it should be this object that we pull most of our printable strings from. Substantives require nothing else in order to work properly.

### Markings
A marking is some sort of mark, modification, or blemishes on the features of a person, place, or thing. Markings are small collections of strings and potentially further markings that the entity being marked might leave.

### Place
A Place is a location. It contains a `Substantive` called `Info`, a `Region` called `Area`, and a `MarkingCollection`. It generates a private `RogueLikeEntity` for use as a backing etity. Regions are denoted by a string (their `Name`) and can be identified from an entity's position on the map.

### Person
A person is a `RogueLikeEntity` with the following components:
- Substantive
- Thoughts
- Speech
- Memories
- MarkingCollection
  - Hair
  - Fingerprint
- (more as time goes on)

### Thing
A thing is a `RogueLikeEntity` with the following components:
- Substantive
- MarkingCollection

## Design Philosophy
We're using an `Entity Component-System` here, which means that the logic for components should be kept within the components themselves. To keep things as straightforward as possible, we will make some rules about what can access what:
- Everything can access Substantive, and Substantive can access nothing.
- Everything can access Thoughts, and Thoughts can access nothing
- The Thoughts component should be an intermediary between the personhood-related components. If some component needs info from another component, they should raise an event in Thoughts, and the relevant component should see that it is needed from the Thoughts events, and pass back through the requested info.
- Neither Places nor Things should have components which need information from other components.

## Strings Style Guide
In order to maintain a forever-readable format going into the future, a unified style for the string representations is required. More TBD

## Credits

[GoRogue](https://github.com/Chris3606/GoRogue )

[SadConsole](https://github.com/SadConsole/SadConsole )

___Special Thanks___ to

* Jeremiah Hamilton
* The SadConsole and GoRogue Discord communities
* JetBrains for their Open Source License