# <img src="logo/logo-title.png" height="120px" alt="Fabulous.MauiControls" />

*F# Functional App Development, using declarative dynamic UI.*

[![build](https://github.com/fabulous-dev/Fabulous.MauiControls/actions/workflows/build.yml/badge.svg)](https://github.com/fabulous-dev/Fabulous.MauiControls/actions/workflows/dotnet.yml) [![Fabulous.MauiControls NuGet version](https://badge.fury.io/nu/Fabulous.MauiControls.svg)](https://badge.fury.io/nu/Fabulous.MauiControls) [![Discord](https://img.shields.io/discord/716980335593914419?label=discord&logo=discord)](https://discord.gg/bpTJMbSSYK)

Never write a ViewModel class again! Conquer the world with clean dynamic UIs!

Fabulous allows you to combine the power of functional programming and the simple Model-View-Update architecture to build any kind of mobile and desktop applications with an expressive, dynamic and clean UI DSL. Go cross-platform with Fabulous for Xamarin.Forms and target iOS, Android, Mac, WPF and more!

## Documentation

Documentation is available at https://docs.fabulous.dev

## About Fabulous

Fabulous aims to provide all the tools to let you create your own mobile and desktop apps using only F# and the [Model-View-Update architecture](https://guide.elm-lang.org/architecture/) (shorten to MVU), with a great F# DSL for building dynamic UIs.  
The combination of F# and MVU makes for a great development experience.

Note that Fabulous itself does not provide UI controls, so you'll need to combine it with another framework like Microsoft.Maui.

### Fabulous for Maui.Controls

You will be able to combine MVU with an SwiftUI-inspired DSL in F#, while targeting several platforms (iOS, Android, and more) using the new single project format.

With Fabulous for Maui.Controls, you will be able to write complete applications in F# like this:
```fsharp
type Model = { Text: string }
type Msg = ButtonClicked

let init () = { Text = "Hello Fabulous!" }

let update msg model =
    match msg with
    | ButtonClicked -> { model with Text = "Thanks for using Fabulous!" }

let view model =
    Application() {
        NavigationPage() {                
            ContentPage("Counter",
                VStack(spacing = 16.) {
                    Image(Aspect.AspectFit, "fabulous.png")
                    Label(model.Text)
                    Button("Click me", ButtonClicked)
                }
            )
        }
    )
```

## Credits
This repository is inspired by [Elmish.WPF](https://github.com/Prolucid/Elmish.WPF), [Elmish.Forms](https://github.com/dboris/elmish-forms) and [elmish](https://github.com/elmish/elmish).
 
