//#if INTERACTIVE
//#time;;
//#r "FSharp.PowerPack.dll";;
//#endif

open System
open System.Text

[<EntryPoint>]
let main argv = 

    //Eric Lippert's Comma Quibbling
    //http://blogs.msdn.com/b/ericlippert/archive/2009/04/15/comma-quibbling.aspx

    //from IEnumerable
    //format: {a, b, c and d}

    //Nick Palladinos
    //http://fssnip.net/6F
    let (<+>) (first : StringBuilder) (second : string) = first.Append(second)

    let npCommaQuibbling (words : seq<string>) =
        let sb (value : string) = new StringBuilder(value)
        let rec format (words : LazyList<string>) acc =
            match words with
            | LazyList.Nil -> sb ""
            | LazyList.Cons(first, LazyList.Nil) -> sb first
            | LazyList.Cons(first, LazyList.Cons(second, LazyList.Nil)) -> acc <+> first <+> " and " <+> second
            | LazyList.Cons(first, rest) ->  acc <+> first <+> ", " |> format rest 
          
        let listOfWords = LazyList.ofSeq words  
        sprintf "{%s}" <| (format listOfWords (sb "")).ToString()

    //but why LazyList? 
    //list has more readable pattern discrimination, executes faster, and fewer gen 0, gen 1, gen2 garbage collections

    let jfCommaQuibbling (words : seq<string>) =
        let sb (value : string) = new StringBuilder(value)
        let rec format (words) acc =
            match words with
            | [] -> sb ""
            | first::[] -> sb first
            | first::second::[] -> acc <+> first <+> " and " <+> second
            | first::rest ->  acc <+> first <+> ", " |> format rest 
          
        let listOfWords = List.ofSeq words  
        sprintf "{%s}" <| (format listOfWords (sb "")).ToString() 

    (*

> {1..100000} |> Seq.map string |> npCommaQuibbling |>ignore;;
Real: 00:00:00.358, CPU: 00:00:00.421, GC gen0: 7, gen1: 4, gen2: 1
val it : unit = ()

> {1..100000} |> Seq.map string |> jfCommaQuibbling |>ignore;;
Real: 00:00:00.186, CPU: 00:00:00.187, GC gen0: 2, gen1: 1, gen2: 0
val it : unit = ()

    *)
    (*http://blogs.msdn.com/b/maoni/archive/2004/06/03/148029.aspx
“# Gen 0 Collections”
“# Gen 1 Collections”
“# Gen 2 Collections”
 
They show the number of collections for the respective generation since the process started. Note that a Gen1 collection collects both Gen0 and Gen1 in one pass

If you are seeing a lot of Gen2 GCs, it means you have many objects that live for too long but not long enough for them to always stay in Gen2.
    *)

    //...unless you are exploiting the reason for LazyList
    let npCommaQuibblingRealLazy (words : seq<string>) =
        let sb (value : string) = new StringBuilder(value)
        let rec format (words : LazyList<string>) acc =
            match words with
            | LazyList.Nil -> sb ""
            | LazyList.Cons(first, LazyList.Nil) -> sb first
            | LazyList.Cons(first, LazyList.Cons(second, LazyList.Nil)) -> acc <+> first <+> " and " <+> second
            | LazyList.Cons(first, LazyList.Cons(second, LazyList.Cons(third, _))) when first = "27"-> acc <+> first <+> ", " <+> second <+> " and " <+> third
            | LazyList.Cons(first, rest) ->  acc <+> first <+> ", " |> format rest 
          
        let listOfWords = LazyList.ofSeq words  
        sprintf "{%s}" <| (format listOfWords (sb "")).ToString()


/////explore comma quibbling...

    ["ABC"; "DEF"; "G"; "H" ] |> npCommaQuibbling |>ignore
    ["ABC"; "DEF" ] |> npCommaQuibbling |>ignore
    ["ABC"] |> npCommaQuibbling |>ignore
    [] |> npCommaQuibbling |>ignore
    {1..100000} |> Seq.map string |> npCommaQuibbling |>ignore
    {1..100000} |> Seq.map string |> jfCommaQuibbling |>ignore
    {1..100000} |> Seq.map string |> npCommaQuibblingRealLazy |>ignore

    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
