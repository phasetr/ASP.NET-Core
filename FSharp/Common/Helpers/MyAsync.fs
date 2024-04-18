namespace Common.Helpers

open System
open System.Threading.Tasks

module MyAsync =
  let awaitValueTask (valueTask: ValueTask<'T>) : Async<'T> =
    Async.FromContinuations(fun (cont, eCont, cCont) ->
      valueTask
        .AsTask()
        .ContinueWith(fun (task: Task<'T>) ->
          match task.Status with
          | TaskStatus.RanToCompletion -> cont task.Result
          | TaskStatus.Faulted -> eCont task.Exception.InnerException
          | TaskStatus.Canceled -> cCont (OperationCanceledException())
          | _ -> eCont (InvalidOperationException("Unexpected task status")))
      |> ignore)
