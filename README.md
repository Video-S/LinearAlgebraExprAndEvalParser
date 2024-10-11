Simple linear algebra recursive-descent interpreter for C#. Will be used in another simple 2D vector math visualizer. 

Tokenizer deals with lexical parsing. Parser deals with syntax. Structs and expressions handle the representation and evaluation (*yes* — i’m really just making up words at this point).

Cobbled together using: https://www2.lawrence.edu/fast/GREGGJ/CMSC270/parser/parser.html

Grammar used: 

```ebnf
<statement> ::= <assignment> | <sum>
  
<assignment> ::= <variable> "=" <sum>
  
<sum> ::= <product> ( ("+" | "-") <product> )*
  
<product> ::= <term> ( ("*" | "/") <term> )*
  
<term> ::= <number> | <vec2> | <variable> | <group>
  
<group> ::= "(" <sum> ")"
  
<number> ::= "0" | [1-9] [0-9]* ( "." [0-9]+ )?
  
<vec2> ::= "[" <number> "," <number> "]"
  
<variable> ::= [a-z]+`
```

This tool was invaluable: https://bnfplayground.pauliankline.com/

Can be used for validating grammars in Backus-Nauer form. Can also generate possible phrases your language can express.

Still to-be-done: 
* Actually make it nice to use, with proper feedback for the user.
* Iron out some of the uglies. The code gets *creative* sometimes.
