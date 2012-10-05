open FSharpx.DataStructures
open System

//for implimenting IComparable see http://stackoverflow.com/questions/5557899/f-implement-icomparable-for-hashseta
type RandomItem (rndInt : int, a : 'a) =
    member this.rndInt = rndInt
    member this.value = a
    interface IComparable<RandomItem> with
        member this.CompareTo otherRnd =
            compare this.rndInt otherRnd.rndInt
    interface IComparable with
        member this.CompareTo otherRnd =
            compare this.rndInt (otherRnd :?> RandomItem).rndInt
    interface IEquatable<RandomItem> with
        member this.Equals otherRnd =
            this.rndInt = otherRnd.rndInt
    override this.Equals otherRnd =
        (this :> IEquatable<_>).Equals (otherRnd :?> RandomItem)
    override this.GetHashCode () =
        hash (this.rndInt, this.value)
        

type RandomStack<'b when 'b : comparison> (h : LeftistHeap<'a>) =
    member this.h = h
    member this.push (item : 'a) =
        let r = new System.Random()
        let n = RandomItem(r.Next(), item)
        RandomStack(h.Insert n)
    member this.pop =
        h.Head().value, h.Tail()
    static member ofSeq (s:seq<'a>) =
        let r = new System.Random()
        //note useful catamorphism in Seq module http://en.wikipedia.org/wiki/Catamorphism
        let s', _ = Seq.fold (fun (acc, (r':System.Random)) elem -> RandomItem(r'.Next(), elem)::acc, r') ([], r) s
        RandomStack(LeftistHeap.ofSeq false s')

    interface Collections.IEnumerable with
        member this.GetEnumerator() = 
            //another useful catamorphism 
            let e = this.h |> Seq.collect (fun (item : RandomItem) -> item.value)
            e.GetEnumerator() :> Collections.IEnumerator

[<EntryPoint>]
let main argv = 

    let rs1 = RandomStack.ofSeq ["a";"b";"c";"e";"f";"g";"h";"i";"j";"k"]
    let rs2 = RandomStack.ofSeq ["a";"b";"c";"e";"f";"g";"h";"i";"j";"k"]
      
    printfn "random stack 1: %A" rs1
    printfn "random stack 2: %A" rs2
    printfn "random stack 2 again: %A" rs2
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
