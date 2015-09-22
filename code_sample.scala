import scala.collection.mutable

class Point(xc: Int, yc: Int) {
  val x: Int = xc
  val y: Int = yc
}

/*
object Even {
  def unapply(x: Int): Option[Int] = if (x%2 == 0) Some(x) else None
}
object Odd {
  def unapply(x: Int): Option[Int] = if (x%2 != 0) Some(x) else None
}

5 match {
  case Even(i) => println(s"$i is even")
  case Odd(i) => println(s"$i is even")
}

object Greeting {
  def unapply(str: String): Option[String] = {
    val parts = str.split(' ')
    if (List("Hi", "Hello", "Bye").contains(parts(0))) Some(parts(1))
    else None
  }
}

object Split {
  def unapply(str: String): Option[(String,String)] = {
    val parts = str.split(' ')
    if (parts.length == 2) Some(parts(0).toUpperCase(), parts(1).toUpperCase())
    else None
  }
}

"Hi Robert" match {
  case Greeting(name) => println(s"Greet $name")
  case Split(_,"ROBERT") => println("Welcome back, Robert!")
  case "magicword" => println("Bingo!")
  case _ => throw new Exception("Unknown")
}
*/
object Test extends App {

  class Person(val name: String, val age: Int) {}
  def selectDataRows() = {
    val rnd = scala.util.Random
    for (i <- Stream.from(1))
      yield Map("name" -> s"Person$i",
                "age" -> rnd.nextInt(99))
  }

  selectDataRows()
    .map(row => new Person(row("name").asInstanceOf[String],
                           row("age").asInstanceOf[Int]))
    .filter(_.name.startsWith("P"))
    .take(20)
    .sortBy(_.age)
    .toList

  /*
  def reverse(x: Int, y: Double, z: String, u: List[Int]) = (u, z, y, x)
  var myTuple = (1, 2., "hola", List(1,2,3))
  val (_,_,_,_li) = myTuple
  //reverse(myTuple)


/*
  reverse myTuple
    let myBigTuple = 1, "hola", 2., 4, [1;2;3]    // Parens can be omitted
  let (_,_,_,_,li) = myBigTuple
*/

  def myFunction(x: Int, y: Int) = x + y

  def myFunction2(x: Int, y: Int) = {
    def privateFunction(z: Int) = z * 2
    privateFunction(myFunction(x, y))
  }

  println(myFunction2(2, 2))

  def recursiveFuncion(x: Int): Int =
    if (x == 1) 1 else x * (recursiveFuncion(x - 1))

  def curryFunction(x: Int)(y: Int) = x * y

  def highOrderFunction(f: Int => Int, y: Int) = f(y)

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

  //println(myFunction(2, 2))
  //println(myFunction2(2, 2))
  println(recursiveFuncion(3))
  println(highOrderFunction(curryFunction(2), 2))
  println(highOrderFunction(curryFunction(4), 2))
  //println(memoizeFunction(8))
  //println(memoizeFunction(8))
  */
}

