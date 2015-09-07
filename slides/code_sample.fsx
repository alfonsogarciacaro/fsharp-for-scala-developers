//#r """C:\Users\Alfonso\Documents\Nuget\FSharp.Data.2.1.0\lib\net40\FSharp.Data.dll"""
//open FSharp.Data
//
//let apiUrl = "http://api.openweathermap.org/data/2.5/weather?q="
//type Weather = JsonProvider<"http://api.openweathermap.org/data/2.5/weather?q=London">
//let sf = Weather.Load(apiUrl + "San Francisco")
//sf.Sys.Country
//sf.Wind.Speed
//sf.Main.

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
