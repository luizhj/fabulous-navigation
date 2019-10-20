// Copyright 2018-2019 Fabulous contributors. See LICENSE.md for license.
namespace fabulous_navigation

open System.Diagnostics
open Fabulous
open Fabulous.XamarinForms
open Fabulous.XamarinForms.LiveUpdate
open Xamarin.Forms

module App = 
   
   type Model =
       {
           PageStack: string option list
       }
   
   type msg =
       | GoHomePage
       | PagePopped
       | PopPage
       | PushPage of string
       | ReplacePage of string

   let initmodel = { PageStack = [Some "Home"] }

   let init() = initmodel,Cmd.none

   let update msg model = 
       match msg with 
           | GoHomePage -> { model with PageStack = [ Some "Home" ] }, Cmd.none
           | PagePopped -> 
               if model.PageStack |> List.exists Option.isNone then 
                   { model with PageStack = model.PageStack |> List.filter Option.isSome }, Cmd.none
               else
                   { model with PageStack = (match model.PageStack with [] -> model.PageStack | _ :: t -> t) }, Cmd.none
           | PopPage -> { model with PageStack = (match model.PageStack with [] -> model.PageStack | _ :: t -> None :: t) }, Cmd.none
           | PushPage page -> { model with PageStack = Some page :: model.PageStack}, Cmd.none
           | ReplacePage page -> { model with PageStack = (match model.PageStack with [] -> Some page :: model.PageStack | _ :: t -> Some page :: t) }, Cmd.none

   let mainpage model dispatch =
       dependsOn model.PageStack (fun model pageStack ->
           View.NavigationPage( pages = [
                       for page in List.rev pageStack do 
                           match page with 
                               | Some "Home" -> 
                                   yield
                                       View.ContentPage(title="home",
                                           content = View.FlexLayout(direction = FlexDirection.Column, alignItems = FlexAlignItems.Center, justifyContent = FlexJustify.SpaceEvenly, 
                                              children = [
                                                   View.Label( text = "Main Page Hellow World Fabulous")
                                                   View.Button( text= "go to next page",command = (fun () -> dispatch (PushPage "Second")))
                                              ]))
                               | Some "Second" -> 
                                   yield 
                                       View.ContentPage(title="second",
                                           content = View.FlexLayout(direction = FlexDirection.Column, alignItems = FlexAlignItems.Center, justifyContent = FlexJustify.SpaceEvenly, 
                                              children = [
                                                   View.Label( text = "Second page")
                                                   View.Button( text = "back" ,command = (fun () -> dispatch PopPage))
                                              ]))
                               | _ -> 
                                 ()  ], 
                       popped=(fun args -> dispatch PagePopped) , 
                       poppedToRoot=(fun args -> dispatch GoHomePage)  ))
          
   let view model dispatch =
       mainpage model dispatch

   // Note, this declaration is needed if you enable LiveUpdate
   let program = Program.mkProgram init update view

type App () as app = 
   inherit Application ()

   let runner = 
       App.program
       |> XamarinFormsProgram.run app
       
