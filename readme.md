Companion demonstration for [Working with Functional Data Structures, Practical F# Application](http://www.siliconvalley-codecamp.com/Sessions.aspx?ForceSortBySessionTime=true&AttendeeId=8689) at Silicon Valley Code Camp 2012.

#Demo1

1) Recursing through a list with active pattern to format the data according to Eric Lippert's "Comma Quibbling".

2) Doing the same with a LazyList takes more time and more Garbage Collection.

3) But if active pattern cuts short recursion before covering the whole list LazyList actually saves resources (time).(Especially useful if calculation or other resources involved.)

#Demo2

1) Demonstrate bifurcating a heap by referencing its internal structure.

2) Demonstrate counting elements faster than O(n).

3) Remove inexed item by re-shaping structure to structure that supports remove.

#Demo3

Create RandomStack data structure that internally implements 2 parts:

1) IComparable type consisting of a random integer and value.

2) Heap of the IComparable items.


