- title : F# for Scala Developers
- description : Introduction to F# for Scala developers
- author : Alfonso Garcia-Caro
- theme : night
- transition : default

***

<style>
strong, thead {
  color: burlywood
}
</style>

### F# for Scala Developers
Walking into the dark side

<div>
  <img style="display: inline-block;vertical-align:middle" src="images/scala.png" />
  <span style="white-space: pre">   meets   </span>
  <img style="display: inline-block;vertical-align:middle" src="images/fsharp.png" />
</div>

> [Scala](http://www.meetup.com/Scala-Programming-Madrid/) and
> [F#](http://www.meetup.com/madrid-fsharp/) Madrid Meetup groups

***

### How much do Scala and F# look like?

- Bring (non-strict) **Functional Programming** to Java and .NET
- Full **compatibility** with their host platforms
- Built-in functional libraries
- **Static safety** with type inference
- Mostly **expression** based (side-effects also allowed)
- **Open source** projects with vibrant communities

***

### How much do Scala and F# differ?

#### Scala

- Embraces both **Object Oriented** and Functional Programming
- Designed not to scare OOP developers: **curly-brace**
- Very powerful and **flexible syntax**
- Very rich **class system**
- Language team works separately from Java team

---

### How much do Scala and F# differ?

#### F#

- Multi-paradigm but **functional-first**
- Inherited from Ocaml: **indentation sensitive**
- Less flexible syntax, more focused on **consistency**
- Two flavors: project-linked and **scripts**
- Language team works together (more or less) with .NET team

> Functional features like generics and tail-call
> instructions are native to the platform

***

#### Let's see some code examples...

***

### Constants, Variables and null

#### Scala

    [lang=scala]
    val a = 5                 // Type inferred constant declaration
    val b: Double = 5.0       // Explicitly typed constant declaration

    var c = 5                 // Variable declaration
    c = 10                    // Mutation

    class Point(xc: Int, yc: Int) {
      val x: Int = xc
      val y: Int = yc
    }

    val p: Point = null       // null allowed for custom classes

---

#### F#

    let a = 5                 // Type inferred constant declaration
    let b: float = 5.         // Explicitly typed constant declaration
    let a = 2                 // Shadowing

    // Verbosity penalty for mutability
    let mutable c = 5         // Variable declaration
    c <- 10                   // Mutation

    // Reference cells: used mainly before F# 4.0
    // to capture mutable variables in closures
    let r = ref 5
    printfn "%i" !r           // Accessing cell content
    r := 2                    // Mutating cell content

    type Point(x: int, y: int) =
      member val X = x
      member val Y = y

    // Doesn't compile, null not allowed in custom classes
    //let p: Point = null

***

### Functions

#### Scala

    [lang=scala]
    def myFunction(x: Int, y: Int) = {
      def privateFunction(z: Int) = z * 2
      privateFunction(x + y)
    }

    def curryFunction(x: Int)(y: Int) = x * y

    def highOrderFunction(f: Int => Int, y: Int) = f(y)

    def recursiveFuncion(x: Int): Int =
      if (x == 1) 1 else x * (recursiveFuncion(x - 1))

---

#### Scala

Memoize Pattern

    [lang=scala]
    val memoizeFunction = {
      val cache = scala.collection.mutable.HashMap.empty[Int,Int]
      (x: Int) => {
        if (cache.contains(x) == false) {
          println(s"Adding $x to cache...")
          cache += (x -> x * 2)
        }
        cache(x)
      }
    }

---

#### F#

    module MyMod =
      let myFunction x y = x + y

      let myFunction2 x y =
        let privateFunction z = z * 2
        // let privateFunction = fun z -> z * 2   // Same effect
        privateFunction (x + y)

      let highOrderFunction f y = f y

      let rec recursiveFuncion x =
        if x = 1 then x else x * (recursiveFuncion (x - 1))

      let memoizeFunction =
        let cache = System.Collections.Generic.Dictionary<int, int>()
        fun x ->
          if cache.ContainsKey x |> not then
            cache.Add(x, x * 2)
          cache.[x]

---

#### F#

Optional and rest parameters are only accepted in non-curried class methods

    open System

    type MyClass =
      static member MyFunction(?x, ?y) =
        (defaultArg x 5) + (defaultArg y 10)
      static member MyFunction2([<ParamArray>] rest: int array) =
        Array.reduce (+) rest

    MyClass.MyFunction(y = 4)
    MyClass.MyFunction2(1, 2, 3)

***

### Imperative loops

#### Scala

    [lang=scala]
    for (index <- 1 to 5) {
      println(s"$index times 5 is ${index * 5}")
    }

    var index = 1
    while (index < 6) {
      println(s"$index times 5 is ${index * 5}")
      index = index + 1
    }

---

#### F#

As in Scala, **break** and **continue** are missing from the language.
Recursion or stream functions are preferred.

    for index = 1 to 5 do
      printfn "%i times 5 is %i" index (index * 5)

    let mutable index = 1
    while index < 6 do
      printfn "%i times 5 is %i" index (index * 5)
      index <- index + 1

***

### Classes

#### Scala

Classes are very powerful in Scala and different from F#:

* Singleton objects
* Traits and abstract types
* Compound types and mixins

---

#### F#

* F# doesn't focus on classes
* Their main purpose is compatibility with .NET Base Class Library
* Mostly same functionality as C# with different Syntax and some additional
  features (like **primary constructors**)
* Interfaces are just abstract classes without default method implementations
* No mixins, only multiple interface implementation is possible
  (extension methods are allowed)
* **Object Expressions** allow dynamic implementation of interfaces

---

### Abstract classes and interfaces

    [<AbstractClass>]                         // If at least one member lacks
    type AbstractBaseClass() =                // implementation, the class must
      abstract member Add: int -> int -> int  //  be marked as abstract
      abstract member Pi: float
      default this.Add x y = x + y            // Default implementation

    type MyInterface =                        // Interfaces are just abstract
      abstract member Square: float -> float  // classes without implementations

---

    type DerivedClass(param1, param2) =
       inherit AbstractBaseClass()                // Inheritance
       let mutable area = 0                       // Private field
       new(param1) = DerivedClass(param1, 5)      // Secondary constructor

       override this.Add _ _ = param1 + param2
       override this.Pi = 3.14                    // Getter-only property

       member this.Area
          with get() = area                       // Getter-Setter property
          and set(v) = area <- v
       member val Area2 = 0 with get, set         // Auto implemented property

       static member StaticValue = 5              // Static members are allowed

       interface MyInterface with                 // Interface implementation
          member this.Square x = x * x            // (always explicit)

---

### Object expressions

```
let o1 = DerivedClass(4)          // new keyword is optional
//o1.Square 5.                    // Cannot access interface methods implicitly
let o2: MyInterface = upcast o1   // Casting is automatic when passing arguments
printfn "%f" (o2.Square 5.)

let o3 =                          // Object expressions create an anonymous object
  { new MyInterface with          // implementing the interface
    member __.Square x = x ** 2. }
printfn "%f" (o3.Square 5.)

```

***

### Tuples and Records

In F#, tuples, records (lightweight classes) and discriminated unions (ADT)
are usually preferred, with logic separated in module functions.

---

#### Tuples in Scala
```
[lang=scala]
def reverse(x: Int, y: Double, z: String, u: List[Int]) = (u, z, y, x)
val myTuple = (1, 2., "hola", List(1,2,3))
val (_,_,_,_li) = myTuple              // Destructuring
myTuple._4                             // Direct access to members
//reverse(myTuple)                     // Error
```

#### Tuples in F#
```
let reverse(x, y, z, u) = (u, z, y, x)
let myTuple = 1, 2., "hola", [1;2;3]   // Parens can be omitted
let (_,_,_,li) = myTuple               // Destructuring
//myTuple._4                           // No direct acccess to members but...
reverse myTuple                        // can be destructured in function args
```

---

#### F# Records

Named tuples or lightweight classes, if you must

```
// Type definition
type MyRecord = { id: int; qt: float; name: string; li: int list }

// Construction
let myRecord = { id = 1; qt = 2.; name = "hola"; li = [1;2;3] }

myRecord.id                                 // Member access
let { id = id2; name = name2 } = myRecord   // Destructuring
let myRecord2 = { myRecord with qt = 5. }   // Copying
```
***

### Algebraic Data Types and Pattern Matching

#### Scala

    [lang=scala]
    abstract class Term
    case class Var(name: String) extends Term
    case class Fun(arg: String, body: Term) extends Term
    case class App(f: Term, v: Term) extends Term

    def formatTerm(term: Term): String = term match {
      case Var(n)    => n
      case Fun(x, b) => s"^$x.${formatTerm(b)}"
      case App(f, v) => s"(${formatTerm(f)} ${formatTerm(v)})"
    }

    println(formatTerm(Fun("x", Fun("y", App(Var("x"), Var("y"))))))

---

#### F#

    type Term =
      | Var of string
      | Fun of string * Term
      | App of Term * Term

    // Compiler warns you if the matching is not comprehensive
    let rec formatTerm term =
      match term with
      | Var(n)    -> sprintf "%s" n
      | Fun(x, b) -> sprintf "^%s.%s" x (formatTerm b)
      | App(f, b) -> sprintf "(%s %s)" (formatTerm f) (formatTerm b)

    // Pipelining is very idiomatic in F#
    Fun("x", Fun("y", App(Var("x"), Var("y"))))
    |> formatTerm
    |> printfn "%s"

---

#### F# (a more contrived example)

```
type Shape =
  | Rectangle of width: float * length: float
  | Circle of radius: float
  | Prism of width: float * float * height: float

let rec matchShapeList = function             // Shortcut
  | [] -> None                                // Empty list
  | [shape] ->                                // Single item
    match shape with                          
    | Rectangle(length, 10.) -> Some(length)  // Constant
    | Circle r when r > 5.0 -> Some(r)        // Guard
    | _ -> failwith "Unknown shape"
  | _::rest -> matchShapeList rest            // Wildcard
```

***

#### Scala Extractor Objects

```
[lang=scala]
object Even {
  def unapply(x: Int) =
    if (x % 2 == 0) Some(x) else None
}

object Odd {
  def unapply(x: Int) =
    if (x % 2 == 0) None else Some(x)
}

println(5 match {
  case Even(_) => "Hello"
  case Odd(_) => "Goodbye"
})
```
---

#### F# Exhaustive Active Patterns
```
let (|Even|Odd|) i =
  if i % 2 = 0 then Even else Odd
  
match 5 with Even -> "Hello" | Odd -> "Goodbye"
|> printfn "%s"
```

#### F# Partial Active Pattern
```
let (|Integer|_|) (str: string) =
  match System.Int32.TryParse str with
  | (true, i) -> Some i | (false, _) -> None

let (|Float|_|) (str: string) =
  match System.Double.TryParse str with
  | (true, f) -> Some f | (false, _) -> None

match "Hola" with
| Integer i -> printfn "%d : Integer" i
| Float f -> printfn "%f : Floating point" f
// Appease the compiler
| _ as str -> printfn "%s : Not matched" str
```

---

#### F# Parameterized Active Patterns

Specializing the pattern by passing extra parameters

```
open System.Text.RegularExpressions

let (|ParseRegex|_|) regex str =
   math Regex.Match(str, regex) with
   | m when not m.Success -> None
   | m -> [for x in m.Groups -> x.Value] |> List.tail |> Some

let parseDate str =
   match str with
     | ParseRegex @"^(\d{1,2})/(\d{1,2})/(\d{1,2})$"
                  [Integer m; Integer d; Integer y]
       -> System.DateTime(y + 2000, m, d)
       
     | ParseRegex @"^(\d{1,2})/(\d{1,2})/(\d{3,4})$"
                  [Integer m; Integer d; Integer y]
       -> System.DateTime(y, m, d)
       
     | ParseRegex @"^(\d{1,4})-(\d{1,2})-(\d{1,2})$"
                  [Integer y; Integer m; Integer d]
       -> System.DateTime(y, m, d)
       
     | _ -> System.DateTime()
```

***

### Kitty Break

![Kitty Break](images/KittyBreak.jpg)

***

### Generics

F# generics are very similar to Scala. The main characteristics are:

* **Automatic Generalization**<br />
  If the function has no dependency on the specific type of a parameter, the type is inferred to be generic

* **Constraints**<br />
  Similar to Scala Type Bounds

* **Statically Resolved Type Parameters**<br />
  Type parameter replaced with actual types at compile time instead of at run time

---

```
// Automatic Generalization
let (|>) x f = f x
// val ( |> ) : x:'a -> f:('a -> 'b) -> 'b

// Constraints
let max x y = if x > y then x else y
// val max : x:'a -> y:'a -> 'a when 'a : comparison

// Syntax for statically resolved type parameters is not beautiful
// and it's mainly intented for core library functions. However it
// can be used with inline functions for neat tricks like duck typing
let inline makeNoise (animal: ^a when ^a : (member MakeNoise: unit->unit)) =
  (^a: (member MakeNoise: unit->unit) animal)
  
type Dog() =
  member __.MakeNoise() = printfn "Guau!"

type Cat() =
  member __.MakeNoise() = printfn "Miau!"
  
makeNoise(Dog())
makeNoise(Cat())
```

***

### Collections

WIP

- Array (ResizeArray), List, Lazy Sequence
- Transformation functions
- Comprehensions

***

### Scala Comprehensions and F# Computation Expressions

WIP

***

#### And now for a couple of unique F# features...

***

### Measure Units

    [<Measure>] type km
    [<Measure>] type mi
    [<Measure>] type h

    type Vector3D<[<Measure>] 'u> = {
      x: float<'u>
      y: float<'u>
      z: float<'u>
    }

    let v1 = 3.1<km/h>
    let v2 = 2.7<km/h>
    let v3 = 1.6<mi/h>
    v1 + v2
    //v1 + v3           // Doesn't compile

Measure annotations disappear after compilation
and thus they have no performance penalty

***

### Type Providers

Static types generated dynamically

![CSV Type Provider ](images/type-provider-csv.gif)

---

#### JSON

![JSON Type Provider ](images/type-provider-json.gif)

---

#### World Bank API REST

![Worl Bank Type Provider ](images/type-provider-wb.gif)

Watch [this presentation](http://sergey-tihon.github.io/Talks/typeproviders#/) to know more about type providers

***

### Flagship Projects

WIP

| Scala          |        | F#             |
| :------------: | ------ | :------------: |
| Akka           |        | Akka.net       |
| Spark          |        | Mbrace         |

***

### Other platforms

WIP

- JavaScript
- Mobile platforms

***

### Want more F#?

* [F# foundation](http://fsharp.org/)
* [F# for Fun and Profit](http://fsharpforfunandprofit.com/)
