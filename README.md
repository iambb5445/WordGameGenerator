# WordGameGenerator

Word Game Generator is a Unity project for generating word games.
This was one of the projects for the Game AI course at the University of California, Santa Cruz (Fall 2021). The project was defined, designed, implemented, and presented by Bahar Bateni.

## Approach

### Examples of a word game

For generating a word game, first, we should define word games. For this purpose, I looked at some popular word games and their rules.

- Wordamendt: A word game from Microsoft. The game has a table of characters. A valid word is a word created by connecting a number of these characters in a combination of horizontal, vertical, and diagonal movements. Every two consecutive characters should share at least one corner. No cell can be selected twice in a word. Any meaningful word is accepted and counts toward your score, but some levels have a theme. Words related to that theme are more valuable.
- Word Cookies: A word game that has a number of words on a circle. A valid word is created by connecting some of these characters in any order. No character can be selected twice in a word. Each level has a set of target words.
- Crossword: In this game, the player puts the characters on a grid. Some parts of this grid have a meaningful word in the horizontal or vertical direction. The player should find the words by reading their meaning. While this game is different from the previous two because the characters are selected by the user instead of the game designer, it has a unique way of forming words in which a horizontal/vertical word consists of only horizontal/vertical movement. In other words, the direction of movement is constant for any word.

### Design elemets

Now, based on the examples, we can define some design elements for a word game.
- Level shape:
  - Grid
  - Circle
  - Any Graph (we can consider nodes as character cells and edges as neighbor relations. A graph can also be used for a grid or circle shape.)
- Movement Type:
  - [in a grid] 4/8 directions
    - May change mid-word
  - [in a circle] Any direction
  - [in a circle] clockwise
  - [in a graph] Defined by the edges of the graph
  - [in a graph] Can have layers of edges, so it would be possible to define rules like not changing direction midword. Selected characters should be connected in at least one of the layers.
- Word properties
  - By meaning
  - By theme
  - Any meaningful word

It goes without saying that these elements are not the only design elements of a word game, and they can only create a subset of possible word games.
This approach of creating a subset of games by looking at an "inspiring set" of existing games in the genre is the same as [BlueCap](https://github.com/possibilityspace/bluecap). For more information, refer to this [video](https://www.youtube.com/watch?v=dZv-vRrnHDA).

## Implementation

For implementing the generator, the Unity game engine is used to both present a GUI for choosing the design choices, and to run the generated game.
The target words for the generated level can be either entered as custom words or generated by calling the [Datamuse API](https://www.datamuse.com/api/).
For generating the level, a backtracking approach is used to try to put as many of the selected words as possible in the selected level shape.

## How to use

### Requirements
- Unity Game Engine

### Instructions

Open the main menu scene from *Assets/Scenes/Main Menu*, then run the scene.
Choose a theme, difficulty, how to obtain words, and level shape.
After choosing the level shape, properties like size and movement direction can be selected.
Click on the generate button.
After generating the level, it can be played by starting from some character, holding the left mouse button down, and moving the mouse. If the word is one of the target words, it will show in the level.
You can use the 'show words' button to see the answer.

## Future Work

Some areas of improvement for this work:
- Better APIs
- Graph visualization (for custom graph level shape)
- Generating the visuals and art in the game (based on a theme, etc.)
