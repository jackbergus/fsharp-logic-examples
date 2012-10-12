﻿// ========================================================================= //
// Copyright (c) 2003-2007, John Harrison.                                   //
// Copyright (c) 2012 Eric Taucher, Jack Pappas, Anh-Dung Phan               //
// (See "LICENSE.txt" for details.)                                          //
// ========================================================================= //

#load "initialization.fsx"

open FSharpx.Books.AutomatedReasoning.lib
open FSharpx.Books.AutomatedReasoning.intro
open FSharpx.Books.AutomatedReasoning.formulas
open FSharpx.Books.AutomatedReasoning.prop
open FSharpx.Books.AutomatedReasoning.fol
open FSharpx.Books.AutomatedReasoning.skolem
open FSharpx.Books.AutomatedReasoning.unif
open FSharpx.Books.AutomatedReasoning.tableaux
open FSharpx.Books.AutomatedReasoning.resolution
open FSharpx.Books.AutomatedReasoning.prolog

// pg. 208
// ------------------------------------------------------------------------- //
// A Horn example.                                                           //
// ------------------------------------------------------------------------- //

let p32 =
    hornprove (parse @" 
        (forall x. P(x) /\ (G(x) \/ H(x)) ==> Q(x)) /\ 
        (forall x. Q(x) /\ H(x) ==> J(x)) /\ 
        (forall x. R(x) ==> H(x)) 
        ==> (forall x. P(x) /\ R(x) ==> J(x))");;

// pg. 208
// ------------------------------------------------------------------------- //
// A non-Horn example.                                                       //
// ------------------------------------------------------------------------- //

// System.Exception: non-Horn clause. - This is the expected result.
hornprove (parse @"(p \/ q) /\ (~p \/ q) /\ (p \/ ~q) ==> ~(~q \/ ~q)");;

// pg. 210
// ------------------------------------------------------------------------- //
// Ordering example.                                                         //
// ------------------------------------------------------------------------- //

let lerules = ["0 <= X"; "S(X) <= S(Y) :- X <= Y"];;

simpleprolog lerules @"S(S(0)) <= S(S(S(0)))";;

simpleprolog lerules @"S(S(0)) <= S(0)";;

let env = simpleprolog lerules @"S(S(0)) <= X";;
apply env "X";;

// pg. 211
// ------------------------------------------------------------------------- //
// Example again.                                                            //
// ------------------------------------------------------------------------- //
   
prolog lerules @"S(S(0)) <= X";;

// pg. 211
// ------------------------------------------------------------------------- //
// Append example, showing symmetry between inputs and outputs.              //
// ------------------------------------------------------------------------- //

let appendrules = [
    @"append(nil,L,L)";
    @"append(H::T,L,H::A) :- append(T,L,A)";];;

prolog appendrules @"append(1::2::nil,3::4::nil,Z)";;

prolog appendrules @"append(1::2::nil,Y,1::2::3::4::nil)";;

prolog appendrules @"append(X,3::4::nil,1::2::3::4::nil)";;

prolog appendrules @"append(X,Y,1::2::3::4::nil)";;

// pg. 211
// ------------------------------------------------------------------------- //
// However this way round doesn't work.                                      //
// ------------------------------------------------------------------------- //

// Process is terminated due to StackOverflowException. - This is the expected result.
//prolog appendrules "append(X,3::4::nil,X)";;

// pg. 212
// ------------------------------------------------------------------------- //
// A sorting example (from Lloyd's "Foundations of Logic Programming").      //
// ------------------------------------------------------------------------- //

let sortrules = [
    @"sort(X,Y) :- perm(X,Y),sorted(Y)";
    @"sorted(nil)";
    @"sorted(X::nil)";
    @"sorted(X::Y::Z) :- X <= Y, sorted(Y::Z)";
    @"perm(nil,nil)";
    @"perm(X::Y,U::V) :- delete(U,X::Y,Z), perm(Z,V)";
    @"delete(X,X::Y,Y)";
    @"delete(X,Y::Z,Y::W) :- delete(X,Z,W)";
    @"0 <= X";
    @"S(X) <= S(Y) :- X <= Y"; ];;

prolog sortrules
  @"sort(S(S(S(S(0))))::S(0)::0::S(S(0))::S(0)::nil,X)";;

// Not it book
// ------------------------------------------------------------------------- //
// Yet with a simple swap of the first two predicates...                     //
// ------------------------------------------------------------------------- //

let badrules = [
    @"sort(X,Y) :- sorted(Y), perm(X,Y)";
    @"sorted(nil)";
    @"sorted(X::nil)";
    @"sorted(X::Y::Z) :- X <= Y, sorted(Y::Z)";
    @"perm(nil,nil)";
    @"perm(X::Y,U::V) :- delete(U,X::Y,Z), perm(Z,V)";
    @"delete(X,X::Y,Y)";
    @"delete(X,Y::Z,Y::W) :- delete(X,Z,W)";
    @"0 <= X";
    @"S(X) <= S(Y) :- X <= Y"; ];;

//** This no longer works

// Process is terminated due to StackOverflowException. - This is the expected result.
//prolog badrules
//  @"sort(S(S(S(S(0))))::S(0)::0::S(S(0))::S(0)::nil,X)";;

