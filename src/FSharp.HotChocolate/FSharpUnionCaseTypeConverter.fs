namespace HotChocolate

open System
open HotChocolate.Utilities


/// Routes conversions from generated F# union case types through converters registered for the union base type.
type internal FSharpUnionCaseTypeConverter() =

    interface IChangeTypeProvider with

        member this.TryCreateConverter
            (source: Type, target: Type, root: ChangeTypeProvider, converter: byref<ChangeType>)
            =
            match Reflection.tryGetFSharpUnionBaseType source with
            | Some unionType ->
                match root.Invoke(unionType, target) with
                | true, unionConverter ->
                    converter <- unionConverter
                    true
                | false, _ -> false
            | None -> false
