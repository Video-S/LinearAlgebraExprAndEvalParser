Simple linear algebra recursive-descent interpreter for C#. Will be used in another simple 2D vector math visualizer. 

Tokenizer deals with lexical parsing. Parser deals with syntax. Structs and expressions handle the representation and evaluation (*yes* — i’m really just making up words at this point).

Example usage:
``` csharp
a = [0.5, 2]
b = 2
c = a * b

// Expected output: [1, 4]
```

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
  
<variable> ::= [a-z]+
```

This tool was invaluable: https://bnfplayground.pauliankline.com/

Can be used for validating grammars in Backus-Nauer form. Can also generate possible phrases your language can express.

Known issues:
* A variable cannot be used within a group expression (`( var )`).

To-be-done:
- [ ] Get rid of the console front-end and add a proper interface.
- [ ] The language might get expanded with more operators, types, and perhaps rudamentary logical capabilities. 
