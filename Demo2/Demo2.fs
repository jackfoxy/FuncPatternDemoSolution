open FSharpx.DataStructures

[<EntryPoint>]
let main argv = 

    (*
    demonstrate bifurcating a heap by referencing its internal structure
    *)
    let h = LeftistHeap.ofSeq false ["v";"n";"g";"a";"z";"m";"k";"d";"q";"e";"r"]

    let hTail = h.Tail

    let decompose  : LeftistHeap<'a> -> 'a * LeftistHeap<'a> * LeftistHeap<'a> = function
        | T(_, _, _, head, h1, h2) -> head, h1, h2
        | E(b) -> "", E(b), E(b)

    let head, h1, h2 = decompose h
    //head and separate heaps may be processed in different threads

    //reassemble
    let h3 = h1.Insert head |> LeftistHeap.merge h2

    let a = h3.Head()
    printfn "head of new heap is %s" a

    (*
    demonstrate counting elements faster than O(n), O(log n) to be sure
    data represented internally by list of trees representing binary digits
    *)
    let b = BinaryRandomAccessList.ofSeq ["v";"n";"g";"a";"z";"m";"k";"d";"q";"e";"r"]

    let rec loop : int * int * list<Digit<'a>> -> int = function
        | len, acc, [] -> len
        | len, acc, Digit.One(_)::ts -> loop ((len + acc), (2 * acc), ts)
        | len, acc, Digit.Zero::ts -> loop (len, (2 * acc), ts)

    let myCount = loop (0, 1, b.randomAccessList)

    printfn "my count %i should equal type length %i" myCount (b.Length())

    (*
    demonstrate composability
    remove inexed item by re-shaping structure to structure that supports remove
    *)
    let b2 = b |> Deque.ofSeq |> Deque.remove 2 |> BinaryRandomAccessList.ofSeq

    printfn "original binary random access list %A" b
    printfn "new binary random access list, item at displacement 2 removed %A" b2


    System.Console.ReadKey() |> ignore
    0 // return an integer exit code
