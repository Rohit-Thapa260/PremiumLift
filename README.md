# Aurum Lift: Dual Elevator Control System (CIS016-2, Assignment One C#)

A Windows Forms app (.NET 8) that controls two lifts in a two-floor building.
It logs every operation to a SQLite database.

## How to run
1. Open `PremiumLift.sln` in **Visual Studio 2022** (with the ".NET desktop development" workload installed).
2. Build. On the first build, NuGet downloads `System.Data.SQLite.Core`.
3. Run with F5. The log database is at `bin\Debug\net8.0-windows\Data\ElevatorLog.db`.
   If that file is deleted, the app creates it again on startup.

## Project structure
| Folder / file | What it does |
|---|---|
| `MainForm.Designer.cs` | The GUI layout (Task 1) |
| `MainForm.cs` | Event handlers, drawing each animation frame and the log view. It is an **observer** of both lifts. |
| `UI/` | Custom premium controls: `LuxButton`, `DigitalDisplay`, `ElevatorCar`, `ShaftPanel`, `GlassPanel`, `Theme` |
| `Models/Elevator.cs` | One lift. It is the **subject** in the Observer pattern and the **context** in the State pattern. It owns the animation Timer and the `AnimationStep` delegate. |
| `Models/States/` | **State pattern**: `IdleState`, `MovingState`, `DoorsOpeningState`, `DoorsOpenState`, `DoorsClosingState` |
| `Models/ElevatorEvents.cs` | `IElevatorObserver` interface, event types and event arguments |
| `Models/ElevatorDispatcher.cs` | Sends each hall call to the lift with the **shortest estimated response time** |
| `DataAccess/ElevatorLogRepository.cs` | All SQLite code: relative path, `SQLiteDataAdapter.Update()`, try/catch in one place |
| `DataAccess/DatabaseWorker.cs` | Runs database jobs on a **BackgroundWorker** (Task 5.1) |
| `DataAccess/ElevatorLogger.cs` | An observer that turns lift events into log rows |
| `Data/ElevatorLog.db` | The SQLite database, shipped with the project |

## Where each marking-matrix item is
| Task | Requirement | Where |
|---|---|---|
| 1 | Complete GUI | `MainForm.Designer.cs`, `UI/` |
| 1 | Event-handler skeleton for all buttons | `MainForm.cs`: `btnCall0_Click` … `btnClearLog_Click` |
| 2 | Handlers work as specified | `CallLiftToFloor`, `SendLiftToFloor`, `Render` |
| 3 | Database designed and connected | `ElevatorLogRepository.EnsureDatabase` (table `ElevatorLog`) |
| 3 | Log retrieved and shown in GUI | `LoadAll` → `RefreshLog` → `dgvLog` |
| 3 | Log stored in DB | `ElevatorLogger.Log` → `Save` |
| 3 | Disconnected model (`DataAdapter.Update()`) | `Save` and `ClearAll` both use `adapter.Update(table)` |
| 3 | Relative path, no logic errors, try-catch | `AppDomain.CurrentDomain.BaseDirectory` + `Data\ElevatorLog.db`; `UseAdapter` catch; `DataAccessException` |
| 3 | No duplication in DB handlers | Every query goes through `UseAdapter<T>()`, and every handler goes through `DatabaseWorker.Enqueue` |
| 4 | Animation with delegate + timer | `Elevator.Timer_Tick` → `AnimationStep step = _state.Tick; step(this);` → observers → `MainForm.Render` |
| 5.1 | BackgroundWorker | `DatabaseWorker` (queue of jobs, results come back on the GUI thread) |
| 5.2 | Two lifts, State + Observer, nearest lift, no disruption | `Models/States/*`, `IElevatorObserver`, `ElevatorDispatcher`, `Elevator.EstimateTimeTo` |
| 5.2 | Inside requests only for that lift | `SendLiftToFloor(_liftA, …)` calls the lift directly and bypasses the dispatcher |

## Behaviour summary
* **Floor names:** the ground floor is shown as **G** on every button, display and log row.
  Internally it is floor `0` (that is also what the database stores), so the logic stays simple.
* **Hall call:** the dispatcher asks each lift how long it would take, using `EstimateTimeTo`.
  The estimate covers the lift's current trip and its queued requests, so ongoing work is never
  interrupted. The fastest lift is sent; on a tie, Lift A goes. If a lift is already heading to
  that floor, it takes the call, so two cars are never sent.
* **Hall call to a floor where a lift is already open:** that lift holds its doors open.
* **Doors:** they open automatically on arrival and close after 2.5 s. **Open** holds them open
  (or re-opens them while closing). **Close** shuts them straight away. Both are refused, with a
  beep, while the car is moving.
* **Lit buttons:** a button stays lit until the request is served, when the doors open at that floor.
* **On close:** the app waits for the BackgroundWorker to finish saving before it exits.
