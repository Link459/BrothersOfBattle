using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using JetBrains.Annotations;
using Logger = Modding.Logger;

// Taken and modified from https://github.com/KayDeeTee/HK-NGG/blob/master/src/FsmUtil.cs

namespace BrothersOfBattle
{
    internal static class FsmUtil
    {
        [PublicAPI]
        public static FsmState GetState(this PlayMakerFSM fsm, string stateName)
        {
            return fsm.FsmStates.FirstOrDefault(t => t.Name == stateName);
        }

        [PublicAPI]
        public static FsmStateAction GetAction(this PlayMakerFSM fsm, string stateName, int index)
        {
            return fsm.GetState(stateName).Actions[index];
        }

        [PublicAPI]
        public static T GetAction<T>(this PlayMakerFSM fsm, string stateName, int index) where T : FsmStateAction
        {
            return GetAction(fsm, stateName, index) as T;
        }

        [PublicAPI]
        public static T GetAction<T>(this PlayMakerFSM fsm, string stateName) where T : FsmStateAction
        {
            return fsm.GetState(stateName).Actions.FirstOrDefault(x => x is T) as T;
        }

        [PublicAPI]
        public static void InsertAction(this PlayMakerFSM fsm, string stateName, FsmStateAction action, int index)
        {
            FsmState t = fsm.GetState(stateName);

            List<FsmStateAction> actions = t.Actions.ToList();

            actions.Insert(index, action);

            t.Actions = actions.ToArray();

            action.Init(t);
        }

        [PublicAPI]
        public static void InsertAction(this PlayMakerFSM fsm, string state, int ind, FsmStateAction action)
        {
            InsertAction(fsm, state, action, ind);
        }

        [PublicAPI]
        public static void RemoveTransition(this PlayMakerFSM fsm, string stateName, string transition)
        {
            FsmState t = fsm.GetState(stateName);

            t.Transitions = t.Transitions.Where(trans => transition != trans.ToState).ToArray();
        }

        [PublicAPI]
        public static void InsertMethod(this PlayMakerFSM fsm, string stateName, int index, Action method)
        {
            InsertAction(fsm, stateName, new InvokeMethod(method), index);
        }
    }

    ///////////////////////
    // Method Invocation //
    ///////////////////////

    public class InvokeMethod : FsmStateAction
    {
        private readonly Action _action;

        public InvokeMethod(Action a)
        {
            _action = a;
        }

        public override void OnEnter()
        {
            _action?.Invoke();
            Finish();
        }
    }
}