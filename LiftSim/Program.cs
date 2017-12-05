using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using LiftSim.States;
using Newtonsoft.Json;

namespace LiftSim
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SettingsStorage.Load("./settings.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error of load settings was occurred. Default settings will be used");
            }

            Logger.MaxLogLevel = args.Length > 0 && args[0] == "debug" ? LogInfo.Debug : LogInfo.Info;

            StateStorage stateStorage = new StateStorage();

            StateMachine liftStateMachine = new StateMachine(new Idle(), stateStorage, Logger.MaxLogLevel == LogInfo.Debug);

            Thread inputThread = new Thread(() =>
            {
                string usage = "Please use call <up\\down> <Floor Number> or go <Floor Number> or i for info";
                string incorrectUsage = "Invalid command. " + usage;
                Console.WriteLine(usage);
                while (true)
                {
                    string command = Console.ReadLine();

                    if (!command.StartsWith("call") && !command.StartsWith("go") && !command.StartsWith("i"))
                    {
                        Console.WriteLine(incorrectUsage);
                        continue;
                    }

                    string[] commandArr = command.Split(' ');

                    uint Floor;
                    switch (commandArr[0])
                    {
                        case "i":
                            lock (stateStorage)
                            {
                                Console.WriteLine(stateStorage);
                            }
                            break;
                        case "call":
                            if (commandArr.Length != 3)
                            {
                                Console.WriteLine(incorrectUsage);
                                continue;
                            }
                            if (commandArr[1] != "up" && commandArr[1] != "down")
                            {
                                Console.WriteLine(incorrectUsage);
                                continue;
                            }

                            LiftCallInfo liftCallInfo = commandArr[1] == "up" ? LiftCallInfo.Up : LiftCallInfo.Down;

                            if (!uint.TryParse(commandArr[2], out Floor))
                            {
                                Console.WriteLine(incorrectUsage);
                                continue;
                            }

                            lock (stateStorage)
                            {
                                stateStorage.LiftCalls.Add(new LiftCall() {Floor = Floor, Direction = liftCallInfo});
                            }

                            liftStateMachine.Refrech();
                            break;
                        case "go":
                            if (commandArr.Length != 2)
                            {
                                Console.WriteLine(incorrectUsage);
                                continue;
                            }

                            if (!uint.TryParse(commandArr[1], out Floor))
                            {
                                Console.WriteLine(incorrectUsage);
                                continue;
                            }

                            lock (stateStorage)
                            {
                                stateStorage.FloorCalls.Add(Floor);
                            }

                            liftStateMachine.Refrech();
                            break;
                        default:
                            Console.WriteLine(incorrectUsage);
                            break;
                    }
                }
            });

            inputThread.Start();

            Enviroment enviroment = new Enviroment(stateStorage);

            var enviromentThread = new Thread(() =>
            {
                while (true)
                {
                    lock (stateStorage)
                    {
                        enviroment.ProcessPhysics();
                        liftStateMachine.Refrech();
                    }
                    Thread.Sleep(SettingsStorage.Settings.ENVIROMENT_STEP);
                }
            });

            enviromentThread.Start();

            Thread watcherThread = new Thread(() =>
            {
                uint? previousFloor = null;
                while (true)
                {
                    lock (stateStorage)
                    {
                        uint nearestFloor = (uint)Math.Round(stateStorage.YPosition / SettingsStorage.Settings.FLOOR_Y_SIZE);
                        if (previousFloor == null) previousFloor = nearestFloor;

                        if (nearestFloor == previousFloor) continue;

                        Logger.Log(string.Format("Lift has changed floor from {0} to {1}. The buttons are on: {2}. Floors are wating for: {3}",
                                previousFloor,
                                nearestFloor,
                                String.Join(",", stateStorage.FloorCalls.ToArray()),
                                String.Join(",", stateStorage.LiftCalls.Select(lc => lc.Floor).ToArray())
                                ), 
                            LogInfo.Info);

                        previousFloor = nearestFloor;
                    }
                }
            });

            watcherThread.Start();
        }
    }
}
