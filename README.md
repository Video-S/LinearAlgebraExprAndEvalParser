# C# recursive-descent interpreter for linear algebra

## Description
Simple linear algebra recursive-descent interpreter for C#. Will be used in another simple 2D vector math visualizer. 

Tokenizer deals with lexicon. Parser deals with syntax. Structs and expressions handle the representation and evaluation. LangConfig allows the language to be configured (—and to be broken!)

There are two types of values in the language: 
* A `Vec2` with syntax `[ x, y ]` where `x` and `y` are a `Number`,
* and a `Number` with syntax `n` where `n` is digit(s) 0 to 9 (0-9+) and `n` is a positive `n` or a negative `-n` and `n` is an integer `n` or a decimal `n.n`.

Both `Vec2` and `Number` can do any arbitrary arithmetic. Available operations are: Addition `+`, subtraction `-`, multiplication `*` and division `\`.

A `Variable` stores either a `Vec2` or a `Number` to later read or write to. They have a syntax of `var` where 'var' is character(s) a to z (a-z+). A value can be assigned using operator `=`.

A `Group` is used to group operations, and has syntax `( expr )` where 'expr' is any arithmetic expression using `Number` and `Vec2`. Can be used to manipulate precedence.

## Example usage:
``` csharp
a = [0.5, 2] // Output: [0.5, 2]
b = 2 // Output: 2
c = a * b // Output: [1, 4]
```

---
## Some notes
Cobbled together using: https://www2.lawrence.edu/fast/GREGGJ/CMSC270/parser/parser.html

Language grammar: 

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

---

Know issues:
* A `Variable` — (consistent with grammar) — cannot be used within a `Vec2` and that is a bit of a waste.
* Dementia (for diagnosis refer to repo title).

To-be-done:
- [ ] The language might get expanded with more operators, types, and perhaps rudamentary logical capabilities. 
