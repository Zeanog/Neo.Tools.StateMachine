"# Neo.Tools.StateMachine" 

Welcome to Neo.Tools.Statemachine!  

As the name suggests this is just taking an old tool and making it new again.  Neo.Tools.Statemchine seperates the transition logic from the statemchine logic.  Allowing
you to then script the transition logic via our simple scripting language.  

Features:
Simple drop n drag use in the editor
Trigger transitions with external events
Setup delays/timers for transitions
Operators allowed in expression: &, |, !, ^, ()
Final bool on the interpreter stack tells system to transition or not

Uses Antlr to generate parser and lexer

Also included:
HierarchyTextHighlighter: 
Let's you color text in hierarchy
To help keep game objects organized.
Neo/HighlightManager menu option allows user to adjust position of highlights

DataStructureLibrary:
Generic pool for data structures.  Allows one to 
Checkout and Return data structures to help reuse memory

Various type extensions
