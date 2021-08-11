"# Neo.Tools.StateMachine" 

Welcome to Neo.Tools.Statemachine!  

As the name suggests this is just taking an old tool and making it new again.  Neo.Tools.Statemachine seperates the transition logic from the statemachine logic.  Allowing
you to then script the transition logic via our simple scripting language.  

Features:
Simple drop n drag use in the editor
Trigger transitions with external events
Setup delays/timers for transitions
Operators allowed in expression: &, |, !, ^, ()
Final bool on the interpreter stack tells system to transition or not

Transition Expression Conditionals:

OnEvent - returns whether the named event has fired.  When an event fires, it's considered fired for the rest of the time in the current state.  Entering the state resets this.

OnDelay - returns whether time from when state was entered has exceeded the specified delay amount.  Sometimes used to avoid rapid ping-ponging state changes.

OnState - represents a C# handler that takes no arguments and returns a boolean.  Object instance that holds the handler must be added to state machines associated objects.

Example
-------------------------------------------------
{
   
   OnEvent onFadeComplete( OnFadeComplete );
   
   OnState isMovieComplete( CheckMovieComplete );
   
   OnDelay onDelay( 2.0 );
   
}

(onFadeComplete | isMovieComplete) & onDelay

-------------------------------------------------

Uses Antlr to generate parser and lexer

I have used this to manage menus in projects for Great Wolf Lodge and Kennedy Space Center

Also included:
HierarchyTextHighlighter: 
Let's you color text in hierarchy
To help keep game objects organized.
Neo/HighlightManager menu option allows user to adjust position of highlights

DataStructureLibrary:
Generic pool for data structures.  Allows one to 
Checkout and Return data structures to help reuse memory

Various type extensions

------------------------------------------------
I am also currently working on a debugger.  Debugging this can be a pain in the ass sometimes.  
8/4/2021 - Checked in a second pass of debugger.  Added search features.  You can search code for a specified string.  Can also search for what transition holds onto a state or 
what state holds onto a transition.  Added ability to search states OnEnter/OnExit events for game object reference

------------------------------------------------
8/11/21 - Added ability to see what transition was used for state change in debugger.  Would like to be able to set a break point on a state and possibly a transition.
Also cleaned up debugger layout.
