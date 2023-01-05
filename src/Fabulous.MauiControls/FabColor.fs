namespace Fabulous.Maui

open System.Runtime.CompilerServices
open Microsoft.Maui.Graphics

/// Fabulous specific representation of a Color for Maui.Controls.
/// Note that it is limited to 4 bytes (vs other representations that might use float for a channel)
/// to optimize allocations and CPU cache misses in diffing algorithm.
/// For all practical use-cases 4 bytes is likely to be more than enough to represent a color,
/// however you can use Maui.Controls.Color directly if you need absolute color precision with custom declared attributes.
[<Struct>]
type FabColor =
    { RGBA: uint32 }

    member inline x.R: uint8 = (x.RGBA &&& 0xFF000000u) >>> 24 |> uint8

    member inline x.G: uint8 = (x.RGBA &&& 0x00FF0000u) >>> 16 |> uint8

    member inline x.B: uint8 = (x.RGBA &&& 0x0000FF00u) >>> 8 |> uint8

    member inline x.A: uint8 = x.RGBA &&& 0x000000FFu |> uint8


[<Extension>]
type ColorConversion() =

    [<Extension>]
    static member inline ToXFColor(this: FabColor) : Color =
        Color.FromRgba(int this.R, int this.G, int this.B, int this.A)

    [<Extension>]
    static member inline ToFabColor(this: Color) : FabColor =
        let r = uint(uint8(this.Red * 255.0f)) <<< 24
        let g = uint(uint8(this.Green * 255.0f)) <<< 16
        let b = uint(uint8(this.Blue * 255.0f)) <<< 8
        let a = uint(uint8(this.Alpha * 255.0f)) <<< 0
        { RGBA = (a ||| r ||| g ||| b) }

module FabColor =
    /// Converts hex string into FabColor.
    /// It uses Maui.Controls.Color to parse, thus it expects the same format.
    /// Expected format: "AARRGGBB","RRGGBB", "ARGB" or "RGB"
    let inline fromHex (hex: string) : FabColor = Color.FromArgb(hex).ToFabColor()

    /// Creates a FabColor from 4 byte size components. Expects RGBA ordering.
    let inline fromRGBA (r: uint8) (g: uint8) (b: uint8) (a: uint8) : FabColor =
        { RGBA = ((uint32 r <<< 24) ||| (uint32 g <<< 16) ||| (uint32 b <<< 8) ||| uint32 a) }
