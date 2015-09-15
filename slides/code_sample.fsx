//#r """C:\Users\Alfonso\Documents\Nuget\FSharp.Data.2.1.0\lib\net40\FSharp.Data.dll"""
//open FSharp.Data
//
//let apiUrl = "http://api.openweathermap.org/data/2.5/weather?q="
//type Weather = JsonProvider<"http://api.openweathermap.org/data/2.5/weather?q=London">
//let sf = Weather.Load(apiUrl + "San Francisco")
//sf.Sys.Country
//sf.Wind.Speed
//sf.Main.

open System.Text.RegularExpressions

let (|Integer|_|) s =
  let success, i = System.Int32.TryParse s
  if success then Some i else None

// ParseRegex parses a regular expression and returns a list of the strings that match each group in
// the regular expression.
// List.tail is called to eliminate the first element in the list, which is the full matched expression,
// since only the matches for each group are wanted.
let (|ParseRegex|_|) regex str =
   let m = Regex(regex).Match(str)
   if m.Success
   then Some (List.tail [ for x in m.Groups -> x.Value ])
   else None

// Three different date formats are demonstrated here. The first matches two-
// digit dates and the second matches full dates. This code assumes that if a two-digit
// date is provided, it is an abbreviation, not a year in the first century.
let parseDate str =
   match str with
     | ParseRegex "(\d{1,2})/(\d{1,2})/(\d{1,2})$" [Integer m; Integer d; Integer y]
          -> new System.DateTime(y + 2000, m, d)
     | ParseRegex "(\d{1,2})/(\d{1,2})/(\d{3,4})" [Integer m; Integer d; Integer y]
          -> new System.DateTime(y, m, d)
     | ParseRegex "(\d{1,4})-(\d{1,2})-(\d{1,2})" [Integer y; Integer m; Integer d]
          -> new System.DateTime(y, m, d)
     | _ -> new System.DateTime()

let dt1 = parseDate "12/22/08"
let dt2 = parseDate "1/1/2009"
let dt3 = parseDate "2008-1-15"
let dt4 = parseDate "1995-12-28"

module Mod1 =
  type T() =
    member __.Foo() = "foo"

module Mod2 =
  type Mod1.T with
    member __.Bar() = "bar"

let t = Mod1.T()
open Mod2
t.Bar()

let r = ref 5
printfn "%i" !r
r := 2

type Point(x: int, y: int) =
  member val X = x
  member val Y = y

// Doesn't compile, null not allowed in custom classes
//let p: Point = null

for item=1 to 5 do
  printfn "%i" item

type MyClass =
  static member MyFunction(?x, ?y) =
    (defaultArg x 5) + (defaultArg y 10)

  static member MyFunction2([<System.ParamArray>] rest: int array) =
    Array.reduce (+) rest

MyClass.MyFunction(y = 4)
MyClass.MyFunction2(1, 2, 3)

type Term =
  | Var of string
  | Fun of string * Term
  | App of Term * Term

// Compiler warns you if the matching is not comprehensive

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

let rec formatTerm term =
  match term with
  | Var(n)    -> sprintf "%s" n
  | Fun(x, b) -> sprintf "^%s.%s" x (formatTerm b)
  | App(f, b) -> sprintf "(%s %s)" (formatTerm f) (formatTerm b)

Fun("x", Fun("y", App(Var("x"), Var("y"))))
|> formatTerm
|> printfn "%s"

type Shape =
  | Rectangle of width: float * length: float
  | Circle of radius: float
  | Prism of width: float * float * height: float

// Compiler infers the argument type from patterns
let rec matchShapeList = function             // Shortcut
  | [] -> None                                // Empty list
  | [shape] ->                                // Single item
    match shape with
    | Rectangle(length, 10.) -> Some(length)  // Constant
    | Circle r when r > 5.0 -> Some(r)        // Guard
    | _ -> failwith "Unknown shape"
  | _::rest -> matchShapeList rest            // Wildcard

let (|Even|Odd|) input =
  if input % 2 = 0
  then Even
  else Odd

let evenOrOdd i =
  match i with
  | Even -> printfn "%d is even" i
  | Odd  -> printfn "%d is odd" i

let (|Greeting|_|) (str: string) =
  let parts = str.Split(' ')
  if ["Hi"; "Hello"; "Bye"] |> List.contains parts.[0]
  then Some(Greeting parts.[1])
  else None

let (|Split|_|) (str: string) =
  let parts = str.Split(' ')
  if parts.Length = 2
  then Some(parts.[0].ToUpper(), parts.[1].ToUpper())
  else None

match "Hi Robert" with
| Greeting name -> printfn "Greet %s" name
| Split(_, "ROBERT") -> printfn "Welcome back, Robert!"
| "magicword" -> printfn "Bingo!"
| _ -> failwith "Unknown"

[<AbstractClass>]                             // If at least one member lacks implementation,
type AbstractBaseClass() =                    // class must be marked as abstract
  abstract member Add: int -> int -> int
  abstract member Pi: float
  default this.Add x y = x + y                // Default implementation

type MyInterface =                            // Interfaces are just abstract
  abstract member Square: float -> float      // classes without implementations

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
      member this.Square x = x * x

let o1 = DerivedClass(4)            // new keyword is optional
//o1.Square 5.                      // Error: cannot access interface methods implicitly
let o2: MyInterface = upcast o1     // Casting is automatic when passing arguments
printfn "%f" (o2.Square 5.)

let o3 =                            // Object expressions create an anonymous object
  { new MyInterface with            // implementing the interface
    member __.Square x = x ** 2. }
printfn "%f" (o3.Square 5.)

let reverse(x, y, z, u) = (u, z, y, x)
let myTuple = 1, 2., "hola", [1;2;3]   // Parens can be omitted
let (_,_,_,li) = myTuple               // Destructuring
//myTuple._4                           // No direct acccess to values...
reverse myTuple                        // but can be destructured in function args

// Record definition
type MyRecord = { id: int; qt: float; name: string; li: int list }

let myRecord = { id = 1; qt = 2.; name = "hola"; li = [1;2;3] } // Construction
let { id = id2; name = name2 } = myRecord                       // Destructuring
let myRecord2 = { myRecord with qt = 5. }                       // Copying
