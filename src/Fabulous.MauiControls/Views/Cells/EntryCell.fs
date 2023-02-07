namespace Fabulous.Maui

open System
open System.Runtime.CompilerServices
open Fabulous
open Microsoft.Maui
open Microsoft.Maui.Controls
open Microsoft.Maui.Graphics

type IFabEntryCell =
    inherit IFabCell

/// Microsoft.Maui doesn't provide an event for textChanged the EntryCell, so we implement it
type FabEntryCell() =
    inherit EntryCell()

    let mutable oldText = ""

    let textChanged = Event<EventHandler<TextChangedEventArgs>, _>()

    [<CLIEvent>]
    member _.TextChanged = textChanged.Publish

    override this.OnPropertyChanged(propertyName) =
        base.OnPropertyChanged(propertyName)

        if propertyName = EntryCell.TextProperty.PropertyName then
            textChanged.Trigger(this, TextChangedEventArgs(oldText, this.Text))

    override this.OnPropertyChanging(propertyName) =
        base.OnPropertyChanging(propertyName)

        if propertyName = EntryCell.TextProperty.PropertyName then
            oldText <- this.Text

module EntryCell =
    let WidgetKey = Widgets.register<FabEntryCell>()

    let HorizontalTextAlignment =
        Attributes.defineBindableEnum<TextAlignment> EntryCell.HorizontalTextAlignmentProperty

    let Keyboard =
        Attributes.defineBindableWithEquality<Keyboard> EntryCell.KeyboardProperty

    let Label = Attributes.defineBindableWithEquality<string> EntryCell.LabelProperty

    let LabelColor = Attributes.defineBindableWithEquality EntryCell.LabelColorProperty
    
    let LabelFabColor = Attributes.defineBindableWithEquality EntryCell.LabelColorProperty

    let OnCompleted =
        Attributes.defineEventNoArg "EntryCell_Completed" (fun target -> (target :?> EntryCell).Completed)

    let Placeholder =
        Attributes.defineBindableWithEquality<string> EntryCell.PlaceholderProperty

    let TextWithEvent =
        Attributes.defineBindableWithEvent "EntryCell_TextChanged" EntryCell.TextProperty (fun target -> (target :?> FabEntryCell).TextChanged)

    let VerticalTextAlignment =
        Attributes.defineBindableEnum<TextAlignment> EntryCell.VerticalTextAlignmentProperty

[<AutoOpen>]
module EntryCellBuilders =
    type Fabulous.Maui.View with
        /// <summary>Create an EntryCell with a label, a text, and listen to text changes</summary>
        /// <param name="label">The label value</param>
        /// <param name="text">The text value</param>
        /// <param name="onTextChanged">Message to dispatch</param>
        static member inline EntryCell<'msg>(label: string, text: string, onTextChanged: string -> 'msg) =
            WidgetBuilder<'msg, IFabEntryCell>(
                EntryCell.WidgetKey,
                EntryCell.Label.WithValue(label),
                EntryCell.TextWithEvent.WithValue(ValueEventData.create text (fun args -> onTextChanged args.NewTextValue |> box))
            )

[<Extension>]
type EntryCellModifiers =
    /// <summary>Set the horizontal text alignment</summary>
    /// <param name="this">Current widget</param>
    /// <param name="alignment">The horizontal text alignment</param>
    [<Extension>]
    static member inline horizontalTextAlignment(this: WidgetBuilder<'msg, #IFabEntryCell>, alignment: TextAlignment) =
        this.AddScalar(EntryCell.HorizontalTextAlignment.WithValue(alignment))

    /// <summary>Set the keyboard type</summary>
    /// <param name="this">Current widget</param>
    /// <param name="keyboard">The keyboard type</param>
    [<Extension>]
    static member inline keyboard(this: WidgetBuilder<'msg, #IFabEntryCell>, keyboard: Keyboard) =
        this.AddScalar(EntryCell.Keyboard.WithValue(keyboard))
        
    /// <summary>Set the color of the label</summary>
    /// <param name="this">Current widget</param>
    /// <param name="value">The color of the label</param>
    [<Extension>]
    static member inline labelColor(this: WidgetBuilder<'msg, #IFabEntryCell>, value: Color) =
        this.AddScalar(EntryCell.LabelColor.WithValue(value))
        
    /// <summary>Set the color of the label</summary>
    /// <param name="this">Current widget</param>
    /// <param name="value">The color of the label</param>
    [<Extension>]
    static member inline labelColor(this: WidgetBuilder<'msg, #IFabEntryCell>, value: FabColor) =
        this.AddScalar(EntryCell.LabelFabColor.WithValue(value))

    /// <summary>Listen to the Completed event</summary>
    /// <param name="this">Current widget</param>
    /// <param name="onCompleted">Message to dispatch</param>
    [<Extension>]
    static member inline onCompleted(this: WidgetBuilder<'msg, #IFabEntryCell>, onCompleted: 'msg) =
        this.AddScalar(EntryCell.OnCompleted.WithValue(onCompleted))

    /// <summary>Set the placeholder text</summary>
    /// <param name="this">Current widget</param>
    /// <param name="placeholder">The placeholder</param>
    [<Extension>]
    static member inline placeholder(this: WidgetBuilder<'msg, #IFabEntryCell>, placeholder: string) =
        this.AddScalar(EntryCell.Placeholder.WithValue(placeholder))

    /// <summary>Set the vertical text alignment</summary>
    /// <param name="this">Current widget</param>
    /// <param name="alignment">The vertical text alignment</param>
    [<Extension>]
    static member inline verticalTextAlignment(this: WidgetBuilder<'msg, #IFabEntryCell>, alignment: TextAlignment) =
        this.AddScalar(EntryCell.VerticalTextAlignment.WithValue(alignment))

    /// <summary>Link a ViewRef to access the direct EntryCell control instance</summary>
    /// <param name="this">Current widget</param>
    /// <param name="value">The ViewRef instance that will receive access to the underlying control</param>
    [<Extension>]
    static member inline reference(this: WidgetBuilder<'msg, IFabEntryCell>, value: ViewRef<EntryCell>) =
        this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))
